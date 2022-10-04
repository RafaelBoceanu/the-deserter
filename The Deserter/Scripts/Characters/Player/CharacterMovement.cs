using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }
    private CharacterController characterController { get { return GetComponent<CharacterController>(); } set { characterController = value; } }
    private CharacterStats characterStats { get { return GetComponent<CharacterStats>(); } set { characterStats = value; } }
    private RagdollManager ragdollManager { get { return GetComponentInChildren<RagdollManager>(); } set { ragdollManager = value; } }

    [System.Serializable]
    public class AnimationSettings
    {
        public string verticalVelocityFloat = "Forward";
        public string horizontalVelocityFloat = "Strafe";
        public string groundedBool = "isGrounded";
        public string jumpBool = "isJumping";
        public string crouchingBool = "isCrouching";
        public string coverBool = "takesCover";
    }
    [SerializeField]
    public AnimationSettings animations;

    [System.Serializable]
    public class PhysicsSettings
    {
        public float gravityModifier = 9.81f;
        public float baseGravity = 50.0f;
        public float resetGravityValue = 5.0f;
        public LayerMask groundLayers;
        public LayerMask waterLayers;
        public LayerMask coverLayers;
        public float airSpeed = 2.5f;
        public Transform characterMesh;
    }
    [SerializeField]
    public PhysicsSettings physics;

    [System.Serializable]
    public class MovementSettings
    {
        public float jumpSpeed = 6;
        public float jumpTime = 0.25f;
        public bool orientToMovement = true;
    }
    [SerializeField]
    public MovementSettings movement;

    public Quaternion characterMeshOriginalRot;


    Vector3 airControl;
    float forward;
    float strafe;
    bool jumping;
    float gravity;
    bool resetGravity;
    bool inWater;
    bool crouching = false;
    bool cover = false;
    float rotationResetSpeed = 1.0f;

    bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 start = transform.position + transform.up;
        Vector3 dir = Vector3.down;
        float radius = characterController.radius;
        if (Physics.SphereCast(start, radius, dir, out hit, characterController.height / 2, physics.groundLayers))
            return true;
        else
            return false;
    }

    public bool InWater()
    {
        RaycastHit hit;
        Vector3 start = transform.position + transform.up;
        Vector3 dir = Vector3.down;
        float radius = characterController.radius;
        if (Physics.SphereCast(start, radius, dir, out hit, characterController.height / 2, physics.waterLayers))
            return true;
        else
            return false;
    }

    void Awake()
    {
        SetupAnimator();
        characterMeshOriginalRot = physics.characterMesh.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        AirControl(forward, strafe);
        ApplyGravity();
        //isGrounded = characterController.isGrounded;
    }

    //Animates the character and root motion handles the movement
    public void Animate(float forward, float strafe)
    {
        if (cover)
        {
            RaycastHit hit;
            Vector3 v = transform.position + new Vector3(0, .1f, 0) + (strafe > 0 ? physics.characterMesh.right * .5f : -physics.characterMesh.right);
            if (Physics.Raycast(v, this.transform.forward * .5f, out hit))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Cover"))
                {
                    forward = 0;
                    strafe = 0;
                }
            }
        }
        else
        {
            this.forward = forward;
            this.strafe = strafe;
        }
        animator.SetFloat(animations.verticalVelocityFloat, forward);
        animator.SetFloat(animations.horizontalVelocityFloat, strafe);
        animator.SetBool(animations.groundedBool, IsGrounded());
        animator.SetBool(animations.jumpBool, jumping);
        animator.SetBool(animations.crouchingBool, crouching);
        animator.SetBool(animations.coverBool, cover);
    }

    void AirControl(float forward, float strafe)
    {
        if (IsGrounded() == false && InWater() == false)
        {
            airControl.x = strafe;
            airControl.z = forward;
            airControl = transform.TransformDirection(airControl);
            airControl *= physics.airSpeed;

            characterController.Move(airControl * Time.deltaTime);

        }
        else if (IsGrounded() == false && InWater() == true)
        {
            characterStats.Die();
            characterStats.health = 0;
            characterStats.isDead = true;
            foreach (Collider col in ragdollManager.colliders)
            {
                col.isTrigger = true;
            }
        }
    }    

    public void Jump()
    {
        if (jumping)
            return;

        if(IsGrounded())
        {
            jumping = true;
            StartCoroutine(StopJump());
        }     
    }

    public void EnterCover()
    {
        RaycastHit hit;
        Vector3 v = transform.position + new Vector3(0, .1f, 0);
        if (Physics.Linecast(v, v + this.transform.forward * .5f, out hit))
        {
            if (hit.transform.gameObject.tag == "Cover")
            {
                Debug.Log("Cover Found " + hit.transform.name);
                Vector3 coverPos = hit.point;
                this.transform.DOMove(new Vector3(coverPos.x, this.transform.position.y, coverPos.z), .2f);
                Quaternion q = Quaternion.LookRotation(-hit.normal);
                q.x = 0;
                q.z = 0;
                this.transform.DORotateQuaternion(q, .2f);
                cover = true;
                movement.orientToMovement = false;
            }
        }
    }

    public void ExitCover()
    {
        if (!cover)
            return;
        else
        {
            cover = false;
            movement.orientToMovement = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Door")
        {
            Debug.Log("DOOR");
        }
    }

    public void Crouch()
    {
        if (crouching)
            return;

        if(IsGrounded())
        {
            crouching = true;
        }    
    }

    public void StopCrouch()
    {
        if (!crouching)
            return;
        else
            crouching = false;
    }

    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(movement.jumpTime);
        jumping = false;
    }

    void ApplyGravity()
    {
        if (!IsGrounded())
        {
            if (!resetGravity)
            {
                gravity = physics.resetGravityValue;
                resetGravity = true;
            }
            gravity += Time.deltaTime * physics.gravityModifier;
        }
        else
        {
            gravity = physics.baseGravity;
            resetGravity = false;
        }

        Vector3 gravityVector = new Vector3();

        if (!jumping)
        {
            gravityVector.y -= gravity;
        }
        else if (jumping)
        {
            gravityVector.y = movement.jumpSpeed;
        }
        else if (!jumping)
        {
            gravityVector.y = 0.1f;
        }

        characterController.Move(gravityVector * Time.deltaTime);
    }

    //Setup the animator with the child avatar
    void SetupAnimator()
    {
        Animator wantedAnim = GetComponentsInChildren<Animator>()[1];
        Avatar wantedAvatar = wantedAnim.avatar;

        animator.avatar = wantedAvatar;
        Destroy(wantedAnim);      
    }
}
