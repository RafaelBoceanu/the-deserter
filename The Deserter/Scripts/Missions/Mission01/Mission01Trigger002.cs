using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mission01Trigger002 : MonoBehaviour
{
    public GameObject container;
    public GameObject mission01;
    public Text mission;
    public AudioSource phone;
    public AudioSource voice;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CompleteTask());
            this.enabled = false;
        }
    }

    IEnumerator CompleteTask()
    {
        yield return new WaitForSeconds(7);
        phone.Play();
        yield return new WaitForSeconds(6);
        phone.Stop();
        voice.Play();
        yield return new WaitForSeconds(3);
        container.SetActive(true);
        mission.text = "Meet the unknown caller";
        mission01.SetActive(false);
    }
}
