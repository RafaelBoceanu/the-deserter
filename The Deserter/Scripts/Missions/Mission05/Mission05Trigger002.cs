using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission05Trigger002 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject hintBox;
    public Text hint;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        mission.text = "Search the platform for the documents";
        yield return new WaitForSeconds(5);
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "The documents are well hidden and guarded \n by one or more enemies. \nSearch the platform thoroughly to find all five";
        yield return new WaitForSeconds(5);
        hintBox.SetActive(false);
        container.SetActive(true);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
