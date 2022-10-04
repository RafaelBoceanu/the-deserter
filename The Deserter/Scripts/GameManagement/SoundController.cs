﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public void PlaySound(AudioSource audioS, AudioClip clip, bool randomisePitch = false, float randomPitchMin = 1, float randomPitchMax = 1)
    {
        audioS.clip = clip;

        if (randomisePitch == true)
            audioS.pitch = Random.Range(randomPitchMin, randomPitchMax);

        audioS.Play();
    }

    public void InstantiateClip(Vector3 pos, AudioClip clip, float time = 2f, bool randomisePitch = false, float randomPitchMin = 1, float randomPitchMax = 1)
    {
        GameObject clone = new GameObject("one shot audio");
        clone.transform.position = pos;
        AudioSource audio = clone.AddComponent<AudioSource>();
        audio.spatialBlend = 1;
        audio.clip = clip;
        audio.Play();

        Destroy(clone, time);
    }
}
