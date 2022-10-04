using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    public GameObject destinationPoint;
    NavMeshAgent theAgent;
    public static bool fleeMode = false;
    public GameObject fleeDest;
    public AudioSource helpMeFX;
    public bool isFleeing = false;

    // Start is called before the first frame update
    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fleeMode == false)
            theAgent.SetDestination(destinationPoint.transform.position);
        else
        {
            theAgent.SetDestination(fleeDest.transform.position);
            if(isFleeing == false)
            {
                isFleeing = true;
                StartCoroutine(FleeingNPC());
            }
        }
    }

    IEnumerator FleeingNPC()
    {
        helpMeFX.Play();
        yield return new WaitForSeconds(13);
        fleeMode = false;
        isFleeing = false;
        this.GetComponent<Animator>().Play("Walking");
        this.GetComponent<NavMeshAgent>().speed = 2.5f;
    }
}