using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class ExitCar : MonoBehaviour
{
    public GameObject carCam;
    public GameObject playerCam;
    public GameObject thePlayer;
    public GameObject exitTrigger;
    public GameObject theCar;
    public GameObject exitPlace;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            thePlayer.SetActive(true);
            carCam.SetActive(false);
            playerCam.SetActive(true);
            theCar.GetComponent<CarUserControl>().enabled = false;
            theCar.GetComponent<CarAudio>().enabled = false;
            theCar.tag = "Untagged";
            thePlayer.transform.parent = null;
            StartCoroutine(EnterAgain());
        }
    }

    IEnumerator EnterAgain()
    {
        yield return new WaitForSeconds(0.5f);
        exitPlace.GetComponent<BoxCollider>().enabled = true;
        this.gameObject.SetActive(false);
    }
}
