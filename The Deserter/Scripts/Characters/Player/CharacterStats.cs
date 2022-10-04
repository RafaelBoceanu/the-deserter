using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private CharacterController characterController { get { return GetComponent<CharacterController>(); } set { characterController = value; } }
    private RagdollManager ragdollManager { get { return GetComponentInChildren<RagdollManager>(); } set { ragdollManager = value; } }
    private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }

    [SerializeField] SpawnPoint[] spawnPoints;

    [Range(0, 100)] public float health = 100;
    public int faction;
    public GameObject alienDeath;
    public bool isDead = false;

    public MonoBehaviour[] scriptsToDisable;

    public CharacterMovement characterMove { get; protected set; }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, 100);
    }

    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {

            if (transform.tag == "Alien" && isDead == false)
            {
                Die();
                StartCoroutine(DestroyNPC());
            }
            else if (transform.tag == "Alien" && isDead == true)
                StartCoroutine(DestroyNPC());

            if (transform.tag == "Player" && isDead == false)
            {
                Die();
                StartCoroutine(Spawn());
            }
            else if (transform.tag == "Player" && isDead == true && this.GetComponent<CharacterStats>().enabled == false)
                StartCoroutine(Spawn());
        }
    }

    public void Die()
    {
        characterController.enabled = false;

        if (scriptsToDisable.Length == 0)
        {
            Debug.Log("All scripts still running on this character but he/she is dead.");
            return;
        }

        foreach (MonoBehaviour script in scriptsToDisable)
            script.enabled = false;

        if (ragdollManager != null)
            ragdollManager.Ragdoll();
    }

    public void SpawnAtNewSpawnpoint()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[spawnIndex].transform.position;
        transform.rotation = spawnPoints[spawnIndex].transform.rotation;
        characterController.enabled = true;
        animator.enabled = true;
        foreach (MonoBehaviour script in scriptsToDisable)
            script.enabled = true;
        if (ragdollManager != null)
            ragdollManager.Start();
        health = 100;
    }

    IEnumerator DestroyNPC()
    {
        yield return new WaitForSeconds(7);
        this.gameObject.SetActive(false);
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3);
        SpawnAtNewSpawnpoint();
    }
}
