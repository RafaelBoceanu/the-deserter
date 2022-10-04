using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class ExitBoat : MonoBehaviour
{
    public GameObject boatCam;
    public GameObject playerCam;
    public GameObject thePlayer;
    public GameObject dummy;
    public GameObject exitTrigger;
    public GameObject theBoat;
    public GameObject exitPlace;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            dummy.SetActive(false);
            thePlayer.SetActive(true);
            boatCam.SetActive(false);
            playerCam.SetActive(true);
            theBoat.GetComponent<CarUserControl>().enabled = false;
            theBoat.GetComponent<CarAudio>().enabled = false;
            thePlayer.transform.parent = null;
            StartCoroutine(EnterBoatAgain());
        }
    }

    IEnumerator EnterBoatAgain()
    {
        yield return new WaitForSeconds(0.5f);
        exitPlace.GetComponent<BoxCollider>().enabled = true;
        this.gameObject.SetActive(false);
    }
}
