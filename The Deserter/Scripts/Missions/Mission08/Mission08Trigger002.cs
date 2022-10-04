using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission08Trigger002 : MonoBehaviour
{
    public GameObject container;
    public GameObject visuals;
    public GameObject mission06;
    public GameObject mission07;
    public Text mission;
    public GameObject npc;
    public GameObject npcCamera;
    public GameObject playerCamera;
    public GameObject player;
    public Save save;
    public AudioSource npcVoice;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        playerCamera.SetActive(false);
        container.SetActive(false);
        visuals.SetActive(false);
        player.SetActive(false);
        npcCamera.SetActive(true);
        yield return new WaitForSeconds(1);
        npc.GetComponent<Animator>().Play("PrayingUp");
        yield return new WaitForSeconds(7);
        npc.GetComponent<Animator>().Play("Talking");
        npcVoice.Play();
        yield return new WaitForSeconds(12);
        container.SetActive(true);
        visuals.SetActive(true);
        npcCamera.SetActive(false);
        player.SetActive(true);
        playerCamera.SetActive(true);
        mission.text = "Exit the asylum";
        mission06.SetActive(false);
        mission07.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
