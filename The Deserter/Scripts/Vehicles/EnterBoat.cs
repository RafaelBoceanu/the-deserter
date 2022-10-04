using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class EnterBoat : MonoBehaviour
{
    public GameObject boatCam;
    public GameObject playerCam;
    public GameObject thePlayer;
    public GameObject dummy;
    public GameObject exitTrigger;
    public GameObject theBoat;
    public bool canEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            canEnter = true;
    }

    void OnTriggerExit(Collider other)
    {
        canEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canEnter == true)
        {
            if (Input.GetButtonDown("Action"))
            {
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                theBoat.GetComponent<CarUserControl>().enabled = true;
                theBoat.GetComponent<CarAudio>().enabled = true;
                playerCam.SetActive(false);
                boatCam.SetActive(true);
                thePlayer.SetActive(false);
                exitTrigger.SetActive(true);
                dummy.SetActive(true);
                canEnter = false;
                thePlayer.transform.parent = this.gameObject.transform;
                StartCoroutine(ExitBoatTrigger());
            }
        }
    }

    IEnumerator ExitBoatTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        exitTrigger.SetActive(true);
    }
}
