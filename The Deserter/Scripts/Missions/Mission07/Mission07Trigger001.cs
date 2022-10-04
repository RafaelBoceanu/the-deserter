using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission07Trigger001 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject hintBox;
    public Text hint;
    public GameObject obj;
    public GameObject mission06;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        obj.SetActive(true);
        mission.text = "Search for clues about the hostages";
        yield return new WaitForSeconds(5);
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "The second clue must be in the office upstairs";
        yield return new WaitForSeconds(5);
        hintBox.SetActive(false);
        container.SetActive(true);
        this.gameObject.SetActive(false);
        mission06.SetActive(false);
        save.SaveGame();
    }
}
