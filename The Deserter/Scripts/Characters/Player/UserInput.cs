using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public CharacterMovement characterMove { get; protected set; }
    public WeaponHandler weaponHandler { get; protected set; }

    public GameObject aimingObject;
    public static float distanceFromTarget;
    public float toTarget;
    public int shotDamage;

    [System.Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Vertical";
        public string horizontalAxis = "Horizontal";
        public string jumpButton = "Jump";
        public string reloadButton = "Reload";
        public string aimButton = "Fire2";
        public string fireButton = "Fire1";
        public string dropWeaponButton = "DropWeapon";
        public string switchWeaponButton = "SwitchWeapon";
        public string crouchButton = "Crouch";
        public string coverButton = "Cover";
    }
    [SerializeField]
    public InputSettings input;

    [System.Serializable]
    public class OtherSettings
    {
        public float lookSpeed = 5.0f;
        public float lookDistance = 10.0f;
        public bool requireInputForTurn = true;
        public LayerMask aimDetectionLayers;
    }
    [SerializeField]
    public OtherSettings other;

    public Camera TPSCamera;

    public bool debugAim;
    public Transform spine;
    private bool aiming;

    Dictionary<Weapon, GameObject> crosshairPrefabMap = new Dictionary<Weapon, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.LocalPlayer = this;
        characterMove = GetComponent<CharacterMovement>();
        weaponHandler = GetComponent<WeaponHandler>();

        SetupCrosshairs();
    }

    void SetupCrosshairs()
    {
        if (weaponHandler.weaponsList.Count > 0)
        {
            foreach (Weapon wep in weaponHandler.weaponsList)
            {
                GameObject prefab = wep.weaponSettings.crosshairPrefab;
                if (prefab != null)
                {
                    GameObject clone = (GameObject)Instantiate(prefab);
                    crosshairPrefabMap.Add(wep, clone);
                    ToggleCrosshair(false, wep);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CharacterLogic();
        CameraLookLogic();
        WeaponLogic();
    }

    void LateUpdate()
    {
        if (weaponHandler)
        {
            if (weaponHandler.currentWeapon)
            {
                if (aiming)
                    PositionSpine();
            }

        }
    }

    //Handles character logic
    void CharacterLogic()
    {
        if (!characterMove)
            return;

        characterMove.Animate(Input.GetAxis(input.verticalAxis), Input.GetAxis(input.horizontalAxis));

        if (Input.GetButtonDown(input.jumpButton))
        {
            characterMove.Jump();
        }

        if (Input.GetButton(input.crouchButton))
        {
            characterMove.Crouch();
        }
        else
        {
            characterMove.StopCrouch();
        }

        if (Input.GetButton(input.coverButton))
        {
            characterMove.EnterCover();
        }
        else
        {
            characterMove.ExitCover();
        }
    }

    //Handles camera logic
    void CameraLookLogic()
    {
        if (!TPSCamera)
            return;

        other.requireInputForTurn = !aiming;

        if (other.requireInputForTurn)
        {
            if (Input.GetAxis(input.horizontalAxis) != 0 || Input.GetAxis(input.verticalAxis) != 0)
                CharacterLook();
        }
        else
            CharacterLook();
    }

    //Handles all weapon logic
    void WeaponLogic()
    {
        RaycastHit hit;

        if (!weaponHandler)
            return;

        aiming = Input.GetButton(input.aimButton) || debugAim;
        weaponHandler.Aim(aiming);

        if (Input.GetButtonDown(input.switchWeaponButton))
        {
            weaponHandler.SwitchWeapons();
            UpdateCrosshairs();
        }

        if (weaponHandler.currentWeapon)
        {
            Ray aimRay = new Ray(TPSCamera.transform.position, TPSCamera.transform.forward);

            if (Input.GetButton(input.fireButton) && aiming)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
                {
                    toTarget = hit.distance;
                    distanceFromTarget = toTarget;
                    shotDamage = 20;
                    if(toTarget <= 10)
                        hit.transform.SendMessage("HurtNPC", shotDamage, SendMessageOptions.DontRequireReceiver);
                }
                weaponHandler.FireCurrentWeapon(aimRay);
            }
            if (Input.GetButtonDown(input.reloadButton))
                weaponHandler.Reload();
            if (Input.GetButtonDown(input.dropWeaponButton))
            {
                DeleteCrosshair(weaponHandler.currentWeapon);
                weaponHandler.DropCurWeapon();
            }

            if (aiming && weaponHandler.currentWeapon.gameObject.activeInHierarchy == true)
            {
                ToggleCrosshair(true, weaponHandler.currentWeapon);
                PositionCrosshair(aimRay, weaponHandler.currentWeapon);
                aimingObject.SetActive(true);
            }
            else
            {
                ToggleCrosshair(false, weaponHandler.currentWeapon);
                aimingObject.SetActive(false);
            }
        }
        else
        {
            TurnOffAllCrosshairs();
        }
    }

    void TurnOffAllCrosshairs()
    {
        foreach (Weapon wep in crosshairPrefabMap.Keys)
        {
            ToggleCrosshair(false, wep);
        }
    }

    void CreateCrosshair(Weapon wep)
    {
        GameObject prefab = wep.weaponSettings.crosshairPrefab;
        if (prefab != null)
        {
            prefab = Instantiate(prefab);
            ToggleCrosshair(false, wep);
        }
    }

    void DeleteCrosshair(Weapon wep)
    {
        if (!crosshairPrefabMap.ContainsKey(wep))
            return;

        Destroy(crosshairPrefabMap[wep]);
        crosshairPrefabMap.Remove(wep);
    }

    //Positions the crosshair to the aiming point
    void PositionCrosshair(Ray ray, Weapon wep)
    {
        Weapon curWeapon = weaponHandler.currentWeapon;

        if (curWeapon == null)
            return;
        if (!crosshairPrefabMap.ContainsKey(wep))
            return;

        GameObject crosshairPrefab = crosshairPrefabMap[wep];
        RaycastHit hit;
        Transform bSpawn = curWeapon.weaponSettings.bulletSpawn;
        Vector3 bSpawnPoint = bSpawn.position;
        Vector3 dir = ray.GetPoint(curWeapon.weaponSettings.range) - bSpawnPoint;

        if (Physics.Raycast(
            bSpawnPoint, 
            dir, 
            out hit, 
            curWeapon.weaponSettings.range,
            other.aimDetectionLayers))
        {
            if (crosshairPrefab != null)
            {
                ToggleCrosshair(true, curWeapon);
                crosshairPrefab.transform.position = hit.point;
                crosshairPrefab.transform.LookAt(Camera.main.transform);
            }
        }
        else
        {
            ToggleCrosshair(false, curWeapon);
        }
    }

    //Toggles the crosshair prefab on and off
    void ToggleCrosshair(bool enabled, Weapon wep)
    {
        if (!crosshairPrefabMap.ContainsKey(wep))
            return;

        crosshairPrefabMap[wep].SetActive(enabled);
    }

    void UpdateCrosshairs()
    {
        if (weaponHandler.weaponsList.Count == 0)
            return;

        foreach (Weapon wep in weaponHandler.weaponsList)
        {
            if (wep != weaponHandler.currentWeapon)
            {
                ToggleCrosshair(false, wep);
            }
        }
    }

    //Postions spine when aiming
    void PositionSpine()
    {
        if (!spine || !weaponHandler.currentWeapon || !TPSCamera)
            return;

        Transform mainCamT = TPSCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 dir = mainCamT.forward;
        Ray ray = new Ray(mainCamPos, dir);

        spine.LookAt(ray.GetPoint(50));

        Vector3 eulerAngleOffset = weaponHandler.currentWeapon.userSettings.spineRotation;
        spine.Rotate(eulerAngleOffset);
    }

    //Makes the character look at a forward point from the camera
    void CharacterLook()
    {
        Transform mainCamT = TPSCamera.transform;
        Transform pivotT = mainCamT.parent;
        Vector3 pivotPos = pivotT.position;
        Vector3 lookTarget = pivotPos + (pivotT.forward * other.lookDistance);
        Vector3 thisPos = transform.position;
        Vector3 lookDir = lookTarget - thisPos;
        Quaternion lookRot = Quaternion.LookRotation(lookDir);
        lookRot.x = 0;
        lookRot.z = 0;

        if (characterMove.movement.orientToMovement)
        {
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * other.lookSpeed);
            transform.rotation = newRotation;
        }
    }
}
