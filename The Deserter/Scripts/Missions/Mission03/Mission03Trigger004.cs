using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission03Trigger004 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject plant;
    public GameObject trigger;
    public GameObject mission01;
    public GameObject mission02;
    public GameObject mission03;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CompleteTask());
            plant.SetActive(false);
        }
    }

    IEnumerator CompleteTask()
    {
        mission.text = "Return the plants to Pete";
        trigger.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        mission01.SetActive(false);
        mission02.SetActive(false);
        mission03.SetActive(false);
    }
}
