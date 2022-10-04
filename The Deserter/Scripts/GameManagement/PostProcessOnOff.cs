using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessOnOff : MonoBehaviour
{
    public GameObject playerCamera;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GetComponent<PostProcessVolume>().enabled = true;
            playerCamera.GetComponent<Camera>().farClipPlane = 200;
        }
    }

    void OnTriggerExit(Collider other)
    {
        this.GetComponent<PostProcessVolume>().enabled = false;
        playerCamera.GetComponent<Camera>().farClipPlane = 300;
    }
}
