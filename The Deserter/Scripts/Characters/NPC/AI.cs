using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Animator))]
public class AI : MonoBehaviour
{
    private NavMeshAgent navmesh;
    private CharacterMovement characterMove { get { return GetComponent<CharacterMovement>(); } set { characterMove = value; } }
    private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }
    private CharacterStats characterStats { get { return GetComponent<CharacterStats>(); } set { characterStats = value; } }
    private WeaponHandler weaponHandler { get { return GetComponent<WeaponHandler>(); } set { weaponHandler = value; } }

    public enum AIState { Patrol, Attack, FindEnemy, FindCover }
    public AIState aiState;

    [System.Serializable]
    public class PatrolSettings
    {
        public WaypointBase[] waypoints;
    }
    public PatrolSettings patrolSettings;

    [System.Serializable]
    public class SightSettings
    {
        public LayerMask sightLayers;
        public float sightRange = 30f;
        public float fieldOfView = 120f;
        public float eyeHeight;

    }
    public SightSettings sight;

    [System.Serializable]
    public class AttackSettings
    {
        public float fireChance = 0.1f;
    }
    public AttackSettings attack;

    private float currentWaitTime;
    private int waypointIndex;
    private Transform currentLookTransform;
    private bool walkingToDest;
    private bool setDestination;
    private bool reachedDestination;

    private float forward;
    private float strafe;

    private Transform target;
    private Transform coverObject;
    private Vector3 targetLastKnownPosition;
    private Vector3 lastSeenCoverObject;
    private CharacterStats[] allCharacters;
    private GameObject[] allCoverObjects;

    private bool aiming;

    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponentInChildren<NavMeshAgent>();

        if (navmesh == null)
        {
            Debug.LogError("We need a navmesh to traverse the world with");
            enabled = false;
            return;
        }

        if (navmesh.transform == this.transform)
        {
            Debug.LogError("The navmesh agent should be a child of the character: " + gameObject.name);
            enabled = false;
            return;
        }

        navmesh.speed = 0;
        navmesh.acceleration = 0;
        navmesh.autoBraking = false;

        if (navmesh.stoppingDistance == 0)
        {
            Debug.Log("Auto settings stopping distance to 1.3f");
            navmesh.stoppingDistance = 1.3f;
        }

        GetAllCharacters();
        GetAllCoverObjects();
    }

    void GetAllCharacters()
    {
        allCharacters = GameObject.FindObjectsOfType<CharacterStats>();
    }

    void GetAllCoverObjects()
    {
        allCoverObjects = GameObject.FindGameObjectsWithTag("Cover");
    }

    // Update is called once per frame
    void Update()
    {
        if (aiState == AIState.Patrol)
        {
            characterMove.Animate(forward, 0);
            navmesh.transform.position = transform.position;
        }
        else if(aiState == AIState.Attack || aiState == AIState.FindEnemy)
        {
            characterMove.Animate(forward, strafe);
            navmesh.transform.position = transform.position;
        }
        weaponHandler.Aim(aiming);

        LookForTarget();

        if(target == null && targetLastKnownPosition != Vector3.zero)
            print(targetLastKnownPosition);

        LookForCover();

        switch (aiState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Attack:
                FireAtEnemy();
                break;
            case AIState.FindCover:
                TakeCover();
                break;
            case AIState.FindEnemy:
                FindEnemy();
                break;
        }
    }

    CharacterStats ClosestEnemy()
    {
        CharacterStats closestCharacter = null;
        float minDistance = Mathf.Infinity;
        foreach (CharacterStats c in allCharacters)
        {
            if (c != this.characterStats && c.faction != this.characterStats.faction)
            {
                float distToCharacter = Vector3.Distance(c.transform.position, transform.position);
                if (distToCharacter < minDistance)
                {
                    closestCharacter = c;
                    minDistance = distToCharacter;
                }
            }
        }

        return closestCharacter;
    }

    GameObject ClosestCoverObject()
    {
        GameObject closestObject = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject o in allCoverObjects)
        {
            if (o.tag == "Cover")
            {
                float distToObject = Vector3.Distance(o.transform.position, transform.position);
                if(distToObject < minDistance)
                {
                    closestObject = o;
                    minDistance = distToObject;
                }
            }
        }
        
        return closestObject;
    }

    void LookForTarget()
    {
        if (allCharacters.Length > 0)
        {
            foreach (CharacterStats c in allCharacters)
            {
                if (c != this.characterStats && c.faction != this.characterStats.faction && c == ClosestEnemy())
                {
                    RaycastHit hit;
                    Vector3 start = transform.position + (transform.up * sight.eyeHeight);
                    Vector3 dir = (c.transform.position + c.transform.up) - start;
                    float sightAngle = Vector3.Angle(dir, transform.forward);
                    if (Physics.Raycast(start, dir, out hit, sight.sightRange, sight.sightLayers) &&
                        sightAngle < sight.fieldOfView && hit.collider.GetComponent<CharacterStats>())
                    {
                        target = hit.transform;
                        targetLastKnownPosition = Vector3.zero;
                    }
                    else
                    {
                        if (target != null)
                        {
                            targetLastKnownPosition = target.position;
                            target = null;
                        }
                    }
                }

            }
        }
    }
    
    void LookForCover()
    {
        if (allCoverObjects.Length > 0)
        {
            foreach (GameObject o in allCoverObjects)
            {
                if(o.tag == "Cover" && o == ClosestCoverObject())
                {
                    RaycastHit hit;
                    Vector3 start = transform.position + (transform.up * sight.eyeHeight);
                    Vector3 dir = (o.transform.position + o.transform.up) - start;
                    float sightAngle = Vector3.Angle(dir, transform.forward);
                    if(Physics.Raycast(start,dir, out hit, sight.sightRange, sight.sightLayers) &&
                       sightAngle < sight.fieldOfView && hit.collider.tag == "Cover")
                    {
                        coverObject = hit.transform;
                        lastSeenCoverObject = Vector3.zero;
                    }
                    else
                    {
                        if (coverObject != null)
                        {
                            lastSeenCoverObject = coverObject.position;
                            coverObject = null;
                        }
                    }
                }
            }
        }
    }

    void Patrol()
    {
        if (target == null && targetLastKnownPosition == Vector3.zero)
        {
            PatrolBehaviour();
            if (!navmesh.isOnNavMesh)
                return;

            if (patrolSettings.waypoints.Length == 0)
                return;

            if (!setDestination)
            {
                navmesh.SetDestination(patrolSettings.waypoints[waypointIndex].destination.position);
                navmesh.stoppingDistance = 1.3f;
                setDestination = true;
            }

            if ((navmesh.remainingDistance <= navmesh.stoppingDistance) || reachedDestination && !navmesh.pathPending)
            {
                setDestination = false;
                walkingToDest = false;
                forward = LerpSpeed(forward, 0, 15);
                currentWaitTime -= Time.deltaTime;

                if (patrolSettings.waypoints[waypointIndex].lookAtTarget != null)
                    currentLookTransform = patrolSettings.waypoints[waypointIndex].lookAtTarget;

                if (currentWaitTime <= 0)
                {
                    waypointIndex = (waypointIndex + 1) % patrolSettings.waypoints.Length;
                    reachedDestination = false;
                }
                else
                    reachedDestination = true;
            }
            else
            {
                LookAtPosition(navmesh.steeringTarget);
                walkingToDest = true;
                forward = LerpSpeed(forward, 0.5f, 15);
                currentWaitTime = patrolSettings.waypoints[waypointIndex].waitTime;
                currentLookTransform = null;
            }
        }
        else if (target != null && targetLastKnownPosition == Vector3.zero)
        {
            aiState = AIState.Attack;
        }
        else if (target == null && targetLastKnownPosition != Vector3.zero)
        {
            aiState = AIState.FindEnemy;
            navmesh.SetDestination(targetLastKnownPosition);
            strafe = LerpSpeed(-strafe, strafe, 10);
            forward = LerpSpeed(forward, 0.5f, 10);
        }
        else if (target != null && characterStats.health < 50)
        {
            aiState = AIState.FindCover;
        }
    }

    public void FireAtEnemy()
    {
        if (target != null)
        {
            AttackBehaviour();
            LookAtPosition(target.position);
            Vector3 start = transform.position + transform.up;
            Vector3 dir = target.position - transform.position;
            Ray ray = new Ray(start, dir);
            if (Random.value <= attack.fireChance)
            {
                weaponHandler.FireCurrentWeapon(ray);

                if (!setDestination)
                {
                    navmesh.SetDestination(target.position);
                    navmesh.stoppingDistance = 4.5f;
                    setDestination = true;
                }

                if ((navmesh.remainingDistance > navmesh.stoppingDistance) || reachedDestination && !navmesh.pathPending)
                {
                    strafe = LerpSpeed(-strafe, strafe, 15);
                    forward = LerpSpeed(forward, 0.75f, 15);
                    walkingToDest = true;
                    reachedDestination = true;
                }
                else
                {
                    setDestination = false;
                    forward = LerpSpeed(forward, 0, 15);
                    walkingToDest = false;
                    reachedDestination = false;
                    navmesh.stoppingDistance = 1.3f;
                }
            }
        }
        else
            Patrol();
    }

    void FindEnemy()
    {
        if (target == null && targetLastKnownPosition != Vector3.zero)
        {
            FindEnemyBehaviour();
            LookAtPosition(targetLastKnownPosition);
            Vector3 start = transform.position + transform.up;
            Vector3 dir = targetLastKnownPosition - transform.position;
            Ray ray = new Ray(start, dir);
            RaycastHit hit;

            if (Physics.Raycast(start, dir, out hit))
            {
                if (hit.transform.tag == "Player")
                    targetLastKnownPosition = target.position;
                else
                    StartCoroutine(Wait());

                if (!setDestination)
                {
                    navmesh.SetDestination(targetLastKnownPosition);
                    navmesh.stoppingDistance = 1.3f;
                    setDestination = true;
                }

                if (navmesh.remainingDistance <= targetLastKnownPosition.z + 15)
                {
                    strafe = LerpSpeed(-strafe, strafe, 15);
                    forward = LerpSpeed(forward, 1, 15);
                    walkingToDest = true;
                    currentLookTransform = null;
                    reachedDestination = true;
                }
                else
                {
                    setDestination = false;
                    forward = LerpSpeed(forward, 0, 15);
                    walkingToDest = false;
                    reachedDestination = false;
                }
            }
        }
        else if (target == null && targetLastKnownPosition == Vector3.zero)
            Patrol();
        else if (target != null && targetLastKnownPosition == Vector3.zero)
            FireAtEnemy();

    }

    void TakeCover()
    {
        if (target != null && this.characterStats.health < 50)
        {
            CoverBehaviour();
            if (!navmesh.isOnNavMesh)
                return;
            
            if (allCoverObjects.Length == 0)
                return;

            if (!setDestination)
            {
                navmesh.SetDestination(ClosestCoverObject().transform.position);
                setDestination = true;
            }

            if ((navmesh.remainingDistance <= navmesh.stoppingDistance) || reachedDestination && !navmesh.pathPending)
            {
                setDestination = false;
                walkingToDest = false;
                forward = LerpSpeed(forward, 0, 15);

                if (allCoverObjects.Length != 0 && ClosestCoverObject())
                    characterMove.EnterCover();
                else if (target == null)
                    Patrol();
                else if (target != null && this.characterStats.health >= 50)
                    aiState = AIState.Attack;
            }
        }
    }

    void PatrolBehaviour()
    {
        aiming = false;
        aiState = AIState.Patrol;
    }

    void AttackBehaviour()
    {
        aiming = true;
        walkingToDest = false;
        reachedDestination = false;
        setDestination = false;
        currentLookTransform = null;
        aiState = AIState.Attack;
    }

    void FindEnemyBehaviour()
    {
        aiming = false;
        walkingToDest = false;
        reachedDestination = false;
        setDestination = false;
        currentLookTransform = null;
        aiState = AIState.FindEnemy;
    }

    void CoverBehaviour()
    {
        aiming = false;
        aiState = AIState.FindCover;
    }

    float LerpSpeed(float curSpeed, float destSpeed, float time)
    {
        curSpeed = Mathf.Lerp(curSpeed, destSpeed, Time.deltaTime * time);
        return curSpeed;
    }

    void LookAtPosition(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0;
        lookRot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * 5);
    }

    void OnAnimatorIK()
    {
        if (currentLookTransform != null && !walkingToDest)
        {
            animator.SetLookAtPosition(currentLookTransform.position);
            animator.SetLookAtWeight(1, 0, 0.5f, 0.7f);
        }
        else if (target != null)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist > 3)
            {
                animator.SetLookAtPosition(target.transform.position + transform.right * 0.3f);
                animator.SetLookAtWeight(1, 1, 0.3f, 0.2f);
            }
            else
            {
                animator.SetLookAtPosition(target.transform.position + target.up + transform.right * 0.3f);
                animator.SetLookAtWeight(1, 1, 0.3f, 0.2f);
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        targetLastKnownPosition = Vector3.zero;

    }
}

[System.Serializable]
public class WaypointBase
{
    public Transform destination;
    public float waitTime;
    public Transform lookAtTarget;
}
