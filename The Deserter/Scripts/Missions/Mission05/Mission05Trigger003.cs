using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission05Trigger003 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject document;
    public Save save;
    public AudioSource phone;
    public AudioSource npcVoice;
    public AudioSource characterVoice;
    public SceneFader sceneFader;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("PlayerX", 35.6f);
            PlayerPrefs.SetFloat("PlayerY", -4.8f);
            PlayerPrefs.SetFloat("PlayerZ", -3.9f);
            StartCoroutine(CompleteTask());
            document.SetActive(false);
        }
    }

    IEnumerator CompleteTask()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        characterVoice.Play();
        yield return new WaitForSeconds(6);
        phone.Play();
        yield return new WaitForSeconds(3);
        phone.Stop();
        npcVoice.Play();
        yield return new WaitForSeconds(15);
        mission.text = "Travel to the asylum";
        yield return new WaitForSeconds(5);
        sceneFader.FadeTo(3);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
