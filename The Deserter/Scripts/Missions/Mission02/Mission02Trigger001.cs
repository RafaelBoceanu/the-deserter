using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission02Trigger001 : MonoBehaviour
{
    public GameObject container;
    public GameObject hintBox;
    public GameObject mission01;
    public Text hint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "Hold 'Right click' to aim and press 'Left click'\n to shoot";
        yield return new WaitForSeconds(4);
        hintBox.SetActive(false);
        container.SetActive(true);
        mission01.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
