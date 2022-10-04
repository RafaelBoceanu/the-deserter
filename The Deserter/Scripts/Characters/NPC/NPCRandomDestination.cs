using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRandomDestination : MonoBehaviour
{
    public int genPos;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Alien")
        {
            genPos = Random.Range(1, 5);

            if (genPos == 4)
            {
                this.gameObject.transform.position = new Vector3(-11, 1, 15);
            }
            if (genPos == 3)
            {
                this.gameObject.transform.position = new Vector3(9, 1, 29);
            }
            if (genPos == 2)
            {
                this.gameObject.transform.position = new Vector3(23, 1, 29);
            }
            if (genPos == 1)
            {
                this.gameObject.transform.position = new Vector3(46, 1, 44);
            }
        }
    }
}
