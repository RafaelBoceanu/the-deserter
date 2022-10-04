using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission03Trigger002 : MonoBehaviour
{
    public GameObject container;
    public GameObject mission01;
    public GameObject mission02;
    public Text mission;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        mission.text = "Enter the house and find Pete";
        yield return new WaitForSeconds(4);
        mission01.SetActive(false);
        mission02.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
