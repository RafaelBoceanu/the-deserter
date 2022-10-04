using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPickup : MonoBehaviour
{
    public GameObject obj;
    public AudioSource pickUpSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //pickUpSound.Play();
            obj.SetActive(false);
        }
    }
}
