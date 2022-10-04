using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission10Trigger002 : MonoBehaviour
{
    public SceneFader sceneFader;
    public Light directionalLight;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CompleteTask());
        }
    }

    IEnumerator CompleteTask()
    {
        directionalLight.color = (Color.green / 3.0f) * Time.deltaTime;
        directionalLight.intensity = 8;
        yield return new WaitForSeconds(3);
        sceneFader.FadeTo(5);
        this.gameObject.SetActive(false);
    }
}
