﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour 
{
	public int speed;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, speed, 0);
	}
}
