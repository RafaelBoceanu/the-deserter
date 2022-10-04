using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiamKill : MonoBehaviour
{
    public GameObject npc;
    public AudioSource voice;
    public GameObject trigger;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LiamSpeech());
        }
    }

    IEnumerator LiamSpeech()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
        trigger.SetActive(true);
        npc.GetComponent<Animator>().Play("Stand");
        yield return new WaitForSeconds(0.5f);
        npc.GetComponent<Animator>().Play("Terrified");
        yield return new WaitForSeconds(0.5f);
        voice.Play();
    }
}
