using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeteCutscene002 : MonoBehaviour
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
    public GameObject mission03;
    public GameObject mission04;
    public Save save;
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
        yield return new WaitForSeconds(13);
        npc.GetComponent<Animator>().Play("Idle");
        npcCamera.SetActive(false);
        playerCamera.SetActive(true);
        missionContainer.SetActive(true);
        statsContainer.SetActive(true);
        yield return new WaitForSeconds(4);
        mission.text = "Take the boat back to the beach";
        mission01.SetActive(false);
        mission02.SetActive(false);
        mission03.SetActive(false);
        mission04.SetActive(true);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}