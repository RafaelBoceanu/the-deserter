using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission10Trigger001 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject hintBox;
    public GameObject mission09;
    public Text hint;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CompleteTask());
        }
    }

    IEnumerator CompleteTask()
    {
        mission.text = "Find the capsule and insert the artifacts";
        yield return new WaitForSeconds(2);
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "This action will break off the seal";
        yield return new WaitForSeconds(5);
        hintBox.SetActive(false);
        container.SetActive(true);
        mission09.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
