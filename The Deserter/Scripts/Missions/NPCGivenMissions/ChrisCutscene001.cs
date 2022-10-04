using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChrisCutscene001 : MonoBehaviour
{
    public GameObject npc;
    public GameObject player;
    public GameObject npcCamera;
    public GameObject playerCamera;
    public GameObject mission01;
    public AudioSource npcVoice;
    public GameObject statsContainer;
    public GameObject missionContainer;
    public Text mission;
    public GameObject hintBox;
    public Text hint;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(ChrisCutscene());
        }
    }

    IEnumerator ChrisCutscene()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
        playerCamera.SetActive(false);
        missionContainer.SetActive(false);
        statsContainer.SetActive(false);
        mission.text = "";
        npcCamera.SetActive(true);
        player.SetActive(false);
        npcVoice.Play();
        npc.GetComponent<Animator>().Play("Talking01");
        ///npcCamera.GetComponent<Animator>().Play("ChrisCamAnim001");
        yield return new WaitForSeconds(16);
        npc.GetComponent<Animator>().Play("Leaning");
        npcCamera.SetActive(false);
        playerCamera.SetActive(true);
        player.SetActive(true);
        statsContainer.SetActive(true);
        hintBox.SetActive(true);
        hint.text = "Press 'E' when near a car to enter it";
        yield return new WaitForSeconds(4);
        hintBox.SetActive(false);
        hint.text = "";
        missionContainer.SetActive(true);
        mission.text = "Go to the cabin in the mountains";
        mission01.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
