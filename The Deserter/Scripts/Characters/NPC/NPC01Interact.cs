using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC01Interact : MonoBehaviour
{
    public AudioSource[] voiceLine;
    public int lineNumber;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(NPCVoiceover());
        }
    }

    IEnumerator NPCVoiceover()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        lineNumber = Random.Range(0, 3);
        voiceLine[lineNumber].Play();
        yield return new WaitForSeconds(2);
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
