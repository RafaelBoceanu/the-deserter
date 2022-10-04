using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mission02Trigger003 : MonoBehaviour
{
    public GameObject container;
    public Text mission;
    public GameObject mission01;
    public GameObject mission02;
    public AudioSource phone;
    public AudioSource voice;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CompleteTask());
    }

    IEnumerator CompleteTask()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        phone.Play();
        yield return new WaitForSeconds(3);
        phone.Stop();
        voice.Play();
        yield return new WaitForSeconds(3);
        container.SetActive(true);
        mission.text = "Reach the lake";
        mission01.SetActive(false);
        mission02.SetActive(false);
    }
}
