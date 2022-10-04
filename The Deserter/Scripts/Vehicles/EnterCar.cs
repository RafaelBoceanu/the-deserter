using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class EnterCar : MonoBehaviour
{
    public GameObject carCam;
    public GameObject playerCam;
    public GameObject thePlayer;
    public GameObject exitTrigger;
    public GameObject theCar;
    public bool canEnter;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
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
                theCar.GetComponent<CarUserControl>().enabled = true;
                theCar.GetComponent<CarAudio>().enabled = true;
                theCar.tag = "Player";
                playerCam.SetActive(false);
                carCam.SetActive(true);
                thePlayer.SetActive(false);
                exitTrigger.SetActive(true);
                canEnter = false;
                thePlayer.transform.parent = this.gameObject.transform;
                StartCoroutine(ExitTrigger());
            }
        }
    }

    IEnumerator ExitTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        exitTrigger.SetActive(true);
    }
}
