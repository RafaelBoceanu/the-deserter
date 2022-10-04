using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission09Trigger002 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject hintBox;
    public Text hint;
    public GameObject artifact;
    public AudioSource voice;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CompleteTask());
            artifact.SetActive(false);
        }
    }

    IEnumerator CompleteTask()
    {
        voice.Play();
        yield return new WaitForSeconds(5);
        mission.text = "Collect the remaining artifacts";
        yield return new WaitForSeconds(2);
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "There are 3 remaining artifacts\n scattered throughout the swamp.\n Search thoroughly for them";
        yield return new WaitForSeconds(5);
        hintBox.SetActive(false);
        container.SetActive(true);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
