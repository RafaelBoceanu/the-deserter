using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission04Trigger001 : MonoBehaviour
{
    public GameObject container;
    public GameObject mission01;
    public GameObject mission02;
    public GameObject mission03;
    public GameObject boat;
    public Text mission;
    public Save save;
    //public AudioSource voice;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        mission.text = "Investigate the appearances on the farm outside town";
        boat.SetActive(false);
        yield return new WaitForSeconds(2);
        mission01.SetActive(false);
        mission02.SetActive(false);
        mission03.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
