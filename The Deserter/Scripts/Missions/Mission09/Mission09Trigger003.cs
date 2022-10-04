using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission09Trigger003 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject hintBox;
    public Text hint;
    public GameObject artifact;
    public GameObject mission09;
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
        mission.text = "Reach the aliens' headquarters";
        yield return new WaitForSeconds(2);
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "The HQ has been set up in the second mansion";
        yield return new WaitForSeconds(5);
        hintBox.SetActive(false);
        container.SetActive(true);
        save.SaveGame();
        this.gameObject.SetActive(false);
        mission09.SetActive(false);
    }
}
