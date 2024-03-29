﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{
    private Collider col { get { return GetComponent<Collider>(); } set { col = value; } }
    private Rigidbody rigidBody { get { return GetComponent<Rigidbody>(); } set { rigidBody = value; } }
    private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }
    public WeaponHandler weaponHandler;
    private SoundController sc;

    public enum WeaponType
    {
        Fist, Pistol, Rifle
    }
    public WeaponType weaponType;

    [System.Serializable]
    public class UserSettings
    {
        public Transform leftHandIKTarget;
        public Vector3 spineRotation;
    }
    [SerializeField]
    public UserSettings userSettings;

    [System.Serializable]
    public class WeaponSettings
    {
        [Header("-Bullet Options-")]
        public Transform bulletSpawn;
        public float damage = 5.0f;
        public float bulletSpread = 5.0f;
        public float fireRate = 0.2f;
        public LayerMask bulletLayers;
        public float range = 200.0f;

        [Header("-Effects-")]
        public GameObject muzzleFlash;
        public GameObject decal;
        public GameObject shell;
        public GameObject clip;
        public GameObject alienBlood;
        public GameObject humanBlood;

        [Header("-Other-")]
        public GameObject crosshairPrefab;
        public float reloadDuration = 2.0f;
        public Transform shellEjectSpot;
        public float shellEjectSpeed = 7.5f;
        public Transform clipEjectPos;
        public GameObject clipGo;

        [Header("-Positioning-")]
        public Vector3 equipPosition;
        public Vector3 equipRotation;
        public Vector3 unequipPosition;
        public Vector3 unequipRotation;

        [Header("-Animation-")]
        public bool useAnimation;
        public int fireAnimationLayer = 0;
        public string fireAnimationName = "Fire";
    }
    [SerializeField]
    public WeaponSettings weaponSettings;

    [System.Serializable]
    public class Ammunition
    {
        public int carryingAmmo;
        public int clipAmmo;
        public int maxClipAmmo;
        public int maxCarryingAmmo;
    }
    [SerializeField]
    public Ammunition ammo;

    WeaponHandler owner;
    bool equipped;
    bool resettingCartridge;

    [System.Serializable]
    public class SoundSettings
    {
        public AudioClip[] gunshotSounds;
        public AudioClip reloadSound;
        [Range(0, 3)] public float pitchMin = 1;
        [Range(0, 3)] public float pitchMax = 1.2f;
        public AudioSource audioS;
    }
    [SerializeField]
    public SoundSettings sounds;

    // Start is called before the first frame update
    void Start()
    {
        GameObject check = GameObject.FindGameObjectWithTag("Sound Controller");

        if (check != null)
        {
            sc = check.GetComponent<SoundController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (owner)
        {
            DisableEnableComponents(false);

            if (equipped)
            {
                if (owner.userSettings.rightHand)
                {
                    Equip();
                }
            }
            else
            {
                if (weaponSettings.bulletSpawn.childCount > 0)
                {
                    foreach (Transform t in weaponSettings.bulletSpawn.GetComponentsInChildren<Transform>())
                    {
                        if (t != weaponSettings.bulletSpawn)
                        {
                            Destroy(t.gameObject);
                        }
                    }
                }

                Unequip(weaponType);
            }
        }
        else //if owner is null
        {
            DisableEnableComponents(true);
            transform.SetParent(null);
        }
    }

    //Fires the weapon
    public void Fire(Ray ray)
    {
        if (ammo.clipAmmo <= 0 || resettingCartridge || !weaponSettings.bulletSpawn || !equipped || weaponHandler.reload)
            return;

        RaycastHit hit;
        Transform bSpawn = weaponSettings.bulletSpawn;
        Vector3 bSpawnPoint = bSpawn.transform.position;
        Vector3 dir = ray.GetPoint(weaponSettings.range) - bSpawnPoint;

        dir += (Vector3)Random.insideUnitCircle * weaponSettings.bulletSpread;

        if (Physics.Raycast(bSpawnPoint, dir, out hit, weaponSettings.range, weaponSettings.bulletLayers))
        {
            HitEffects(hit);
        
            if (hit.collider.GetComponent<CharacterStats>())
                hit.transform.root.SendMessage("Damage", weaponSettings.damage);

            if (hit.transform.tag == "Alien")
                Instantiate(weaponSettings.alienBlood, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            else if (hit.transform.tag == "Player")
                Instantiate(weaponSettings.humanBlood, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        }

        GunEffects();

        if (weaponSettings.useAnimation && weaponHandler.currentWeapon.gameObject.activeInHierarchy == true)
            animator.Play(weaponSettings.fireAnimationName, weaponSettings.fireAnimationLayer);
        if (weaponHandler.currentWeapon.gameObject.activeInHierarchy == true)
        {
            ammo.clipAmmo--;
            resettingCartridge = true;
            StartCoroutine(LoadNextBullet());
        }
    }

    //Loads next bullet into the chamber
    IEnumerator LoadNextBullet()
    {
        yield return new WaitForSeconds(weaponSettings.fireRate);
        resettingCartridge = false;
    }

    void HitEffects(RaycastHit hit)
    {
        if (hit.collider.gameObject.isStatic)
        {
            if (weaponSettings.decal)
            {
                Vector3 hitPoint = hit.point;
                Quaternion lookRotation = Quaternion.LookRotation(hit.normal);
                GameObject decal = Instantiate(weaponSettings.decal, hitPoint, lookRotation) as GameObject;
                Transform decalT = decal.transform;
                Transform hitT = hit.transform;
                decalT.SetParent(hitT);
                Destroy(decal, UnityEngine.Random.Range(15.0f, 20.0f));
            }
        }
    }

    void GunEffects()
    {
        if (weaponSettings.muzzleFlash)
        {
            Vector3 bulletSpawnPos = weaponSettings.bulletSpawn.position;
            GameObject muzzleFlash = Instantiate(weaponSettings.muzzleFlash, bulletSpawnPos, Quaternion.identity) as GameObject;
            Transform muzzleT = muzzleFlash.transform;
            muzzleT.SetParent(weaponSettings.bulletSpawn);
            Destroy(muzzleFlash, 2.0f);
        }

        if (weaponSettings.shell)
        {
            if (weaponSettings.shellEjectSpot)
            {
                Vector3 shellEjectPos = weaponSettings.shellEjectSpot.position;
                Quaternion shellEjectRot = weaponSettings.shellEjectSpot.rotation;
                GameObject shell = Instantiate(weaponSettings.shell, shellEjectPos, shellEjectRot) as GameObject;

                if (shell.GetComponent<Rigidbody>())
                {
                    Rigidbody rigidB = shell.GetComponent<Rigidbody>();
                    rigidB.AddForce(weaponSettings.shellEjectSpot.forward * weaponSettings.shellEjectSpeed, ForceMode.Impulse);
                }

                Destroy(shell, UnityEngine.Random.Range(15.0f, 20.0f));
            }
        }

        PlayGunshotSound();
    }

    void PlayGunshotSound()
    {
        if (sc == null)
            return;

        if (sounds.audioS != null)
        {
            if (sounds.gunshotSounds.Length > 0)
            {
                sc.InstantiateClip(
                    weaponSettings.bulletSpawn.position, //Where we want to play the sound from
                    sounds.gunshotSounds[Random.Range(0, sounds.gunshotSounds.Length)], //What audio clip we will use for this sound
                    2, //How long before we destroy the audio
                    true, //Do we want to randomise the sound?
                    sounds.pitchMin, //The minimum pitch that the sound will use
                    sounds.pitchMax); //The maximum pitch that the sound will use
            }
        }
    }

    //Disables or enabled collider and rigidbody
    void DisableEnableComponents(bool enabled)
    {
        if (!enabled)
        {
            rigidBody.isKinematic = true;
            col.enabled = false;
        }
        else
        {
            rigidBody.isKinematic = false;
            col.enabled = true;
        }
    }

    //Equips this weapon to the hand
    void Equip()
    {
        if (!owner)
            return;
        else if (!owner.userSettings.rightHand)
            return;

        transform.SetParent(owner.userSettings.rightHand);
        transform.localPosition = weaponSettings.equipPosition;
        Quaternion equipRot = Quaternion.Euler(weaponSettings.equipRotation);
        transform.localRotation = equipRot;
    }

    //Unequips the weapon and places it to the desired location
    void Unequip(WeaponType wpType)
    {
        if (!owner)
            return;

        switch (wpType)
        {
            case WeaponType.Fist:
                break;
            case WeaponType.Pistol:
                transform.SetParent(owner.userSettings.pistolUnequipSpot);
                break;
            case WeaponType.Rifle:
                transform.SetParent(owner.userSettings.rifleUnequipSpot);
                break;
        }
        transform.localPosition = weaponSettings.unequipPosition;
        Quaternion unEquipRot = Quaternion.Euler(weaponSettings.unequipRotation);
        transform.localRotation = unEquipRot;
    }

    //Loads the clip and calculates the ammo
    public void LoadClip()
    {
        int ammoNeeded = ammo.maxClipAmmo - ammo.clipAmmo;

        if (ammoNeeded >= ammo.carryingAmmo)
        {
            ammo.clipAmmo = ammo.carryingAmmo;
            ammo.carryingAmmo = 0;
        }
        else
        {
            ammo.carryingAmmo -= ammoNeeded;
            ammo.clipAmmo = ammo.maxClipAmmo;
        }
    }

    //Sets the weapon's equip state
    public void SetEquipped(bool equip)
    {
        equipped = equip;
    }

    //Sets owner of this weapon
    public void SetOwner(WeaponHandler wp)
    {
        owner = wp;
    }
}
