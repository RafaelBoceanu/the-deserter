using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission08Trigger003 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public SceneFader sceneFader;
    public Save save;
    public GameObject mission06;
    public GameObject mission07;
    public GameObject mission08;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("PlayerX", 457.08f);
            PlayerPrefs.SetFloat("PlayerY", 27.12411f);
            PlayerPrefs.SetFloat("PlayerZ", 154.37f);
            StartCoroutine(CompleteTask());
        }
    }

    IEnumerator CompleteTask()
    {
        yield return new WaitForSeconds(5);
        sceneFader.FadeTo(4);
        mission06.SetActive(false);
        mission07.SetActive(false);
        mission08.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
