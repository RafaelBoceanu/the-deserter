using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission09Trigger001 : MonoBehaviour
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
        mission.text = "Investigate the swamp";
        yield return new WaitForSeconds(0.1f);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
