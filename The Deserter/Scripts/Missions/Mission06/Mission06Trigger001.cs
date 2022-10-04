using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission06Trigger001 : MonoBehaviour
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
        mission.text = "Investigate the two offices";
        yield return new WaitForSeconds(5);
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "There is one office on each story of the asylum\n and they are on the wing opposite the statue in\n the main hallway";
        yield return new WaitForSeconds(5);
        hintBox.SetActive(false);
        container.SetActive(true);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}