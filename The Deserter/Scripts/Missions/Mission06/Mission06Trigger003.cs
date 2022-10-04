using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission06Trigger003 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject hintBox;
    public GameObject mission06;
    public Text hint;
    public Save save;
    //public AudioSource voice;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        mission06.SetActive(false);
        mission.text = "Search for clues about the hostages";
        yield return new WaitForSeconds(5);
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "They are well hidden. Search everywhere";
        yield return new WaitForSeconds(5);
        hintBox.SetActive(false);
        container.SetActive(true);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
