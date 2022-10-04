using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastTriggerOn : MonoBehaviour
{
    public GameObject trigger1;
    public GameObject trigger2;
    public GameObject trigger3;
    public GameObject missionTrigger;

    // Update is called once per frame
    void Update()
    {
        if (!trigger1.activeSelf && !trigger2.activeSelf && !trigger3.activeSelf)
        {
            missionTrigger.gameObject.SetActive(true);
        }
    }
}
