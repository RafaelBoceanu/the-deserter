using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDestination : MonoBehaviour
{
    public int trigNum;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            if (trigNum == 4)
            {
                trigNum = 0;
            }
            if (trigNum == 3)
            {
                this.gameObject.transform.position = new Vector3(-11, 1, 15);
                trigNum = 4;
            }
            if (trigNum == 2)
            {
                this.gameObject.transform.position = new Vector3(9, 1, 29);
                trigNum = 3;
            }
            if (trigNum == 1)
            {
                this.gameObject.transform.position = new Vector3(23, 1, 29);
                trigNum = 2;
            }
            if (trigNum == 0)
            {
                this.gameObject.transform.position = new Vector3(46, 1, 44);
                trigNum = 1;
            }
        }
    }
}
