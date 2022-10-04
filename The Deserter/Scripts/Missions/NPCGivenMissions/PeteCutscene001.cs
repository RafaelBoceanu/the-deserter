using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeteCutscene001 : MonoBehaviour
{
    public GameObject npc;
    public GameObject player;
    public GameObject npcCamera;
    public GameObject playerCamera;
    public AudioSource npcVoice;
    public GameObject statsContainer;
    public GameObject missionContainer;
    public GameObject mission01;
    public GameObject mission02;
    public GameObject mission03trigger002;
    public Text mission;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PeteCutscene());
        }
    }

    IEnumerator PeteCutscene()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
        playerCamera.SetActive(false);
        missionContainer.SetActive(false);
        statsContainer.SetActive(false);
        npcCamera.SetActive(true);
        npcVoice.Play();
        npc.GetComponent<Animator>().Play("Talking");
        yield return new WaitForSeconds(11);
        npc.GetComponent<Animator>().Play("Idle");
        npcCamera.SetActive(false);
        missionContainer.SetActive(true);
        playerCamera.SetActive(true);
        statsContainer.SetActive(true);
        yield return new WaitForSeconds(4);
        mission.text = "Go to the creek";
        mission01.SetActive(false);
        mission02.SetActive(false);
        mission03trigger002.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
