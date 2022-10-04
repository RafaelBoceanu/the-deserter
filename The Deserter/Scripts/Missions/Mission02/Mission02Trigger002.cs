using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission02Trigger002 : MonoBehaviour
{
    public GameObject npc;
    public GameObject car;
    public GameObject mission01;
    public Text mission;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            car.SetActive(false);
            npc.SetActive(true);
            mission.text = "Kill the man in the house";
            mission01.SetActive(false);
            save.SaveGame();
            this.gameObject.SetActive(false);
        }
    }
}
