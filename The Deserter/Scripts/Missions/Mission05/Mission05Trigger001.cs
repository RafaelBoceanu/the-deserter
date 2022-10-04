using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission05Trigger001 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        mission.text = "Investigate the industrial area";
        yield return new WaitForSeconds(2);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
