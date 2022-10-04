using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission04Trigger003 : MonoBehaviour
{
    public SceneFader sceneFader;
    public GameObject container;
    public GameObject hintBox;
    public GameObject mission01;
    public GameObject mission02;
    public GameObject mission03;
    public GameObject trigger;
    public Text hint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("PlayerX", -22.5f);
            PlayerPrefs.SetFloat("PlayerY", -13.07f);
            PlayerPrefs.SetFloat("PlayerZ", -49.13f);
            StartCoroutine(CompleteTask());
        }
    }

    IEnumerator CompleteTask()
    {
        container.SetActive(false);
        hintBox.SetActive(true);
        hint.text = "Go to the road in order\n to travel to the industrial area";
        yield return new WaitForSeconds(2);
        hintBox.SetActive(false);
        container.SetActive(true);
        sceneFader.FadeTo(2);
        mission01.SetActive(false);
        mission02.SetActive(false);
        mission03.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
