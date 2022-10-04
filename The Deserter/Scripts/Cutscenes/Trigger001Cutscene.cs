using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Trigger001Cutscene : MonoBehaviour
{
    public GameObject fakeCamera;
    public GameObject playerCamera;
    public GameObject statsContainer;
    public GameObject missionContainer;
    public int animationDuration;
    public AudioSource characterVoice;

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {   
        if(other.tag == "Player")
            StartCoroutine(OpenScene());
    }

    IEnumerator OpenScene()
    {
        playerCamera.SetActive(false);
        statsContainer.SetActive(false);
        missionContainer.SetActive(false);
        fakeCamera.SetActive(true);
        yield return new WaitForSeconds(animationDuration);
        fakeCamera.SetActive(false);
        playerCamera.SetActive(true);
        statsContainer.SetActive(true);
        missionContainer.SetActive(true);
        yield return new WaitForSeconds(2);
        characterVoice.Play();
    }
}