using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission04On : MonoBehaviour
{
    public GameObject mission01;
    public GameObject mission02;
    public GameObject mission03;
    public GameObject mission04;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mission01.SetActive(false);
            mission02.SetActive(false);
            mission03.SetActive(false);
            mission04.SetActive(true);
        }
    }
}
