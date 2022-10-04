﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission03Trigger001 : MonoBehaviour
{
    public GameObject container;
    public GameObject mission01;
    public GameObject mission02;
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
        container.SetActive(false);
        mission.text = "";
        hintBox.SetActive(true);
        hint.text = "Press 'E' when near a boat to enter it";
        yield return new WaitForSeconds(4);
        hintBox.SetActive(false);
        hint.text = "";
        container.SetActive(true);
        mission.text = "Drive the boat to the pier house";
        mission01.SetActive(false);
        mission02.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
