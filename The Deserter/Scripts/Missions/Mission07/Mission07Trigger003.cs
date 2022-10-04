using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission07Trigger003 : MonoBehaviour
{
    public GameObject player;
    public GameObject container;
    public GameObject blackImage;
    public Text mission;
    public Save save;
    public SceneFader sceneFader;
    public GameObject mission06;
    public GameObject mission07;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        blackImage.SetActive(true);
        player.transform.position = new Vector3(35.6f, -4.8f, -3.9f);
        yield return new WaitForSeconds(2);
        blackImage.SetActive(false);
        mission.text = "Reach the bedroom";
        mission06.SetActive(false);
        mission07.SetActive(false);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
