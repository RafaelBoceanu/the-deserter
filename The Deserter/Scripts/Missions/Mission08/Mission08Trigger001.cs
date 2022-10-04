using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission08Trigger001 : MonoBehaviour
{
    public GameObject container;
    public GameObject mission06;
    public GameObject mission07;
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
        mission.text = "Free the hostages";
        yield return new WaitForSeconds(5);
        mission06.SetActive(false);
        mission07.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}