using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission07Trigger002 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject trigger;
    public GameObject obj;
    public GameObject mission06;
    public AudioSource voice;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        voice.Play();
        yield return new WaitForSeconds(4);
        trigger.SetActive(true);
        obj.SetActive(true);
        yield return new WaitForSeconds(5);
        mission.text = "Regroup in the morgue";
        mission06.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
