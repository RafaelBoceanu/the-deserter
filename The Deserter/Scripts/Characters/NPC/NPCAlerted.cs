﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAlerted : MonoBehaviour
{
    void OnTriggerEnter(Collider npc)
    {
        if(npc.tag == "NPC")
        {
            npc.GetComponent<Animator>().Play("Running");
            npc.GetComponent<NavMeshAgent>().speed = 7.5f;
            NPCAI.fleeMode = true;
        }
    }
}
