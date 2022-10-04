using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mission01Trigger001 : MonoBehaviour
{
    public Text mission;
    public Save save;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CompleteTask());
        }
    }

    IEnumerator CompleteTask()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(7);
        mission.text = "Reach the town";
        yield return new WaitForSeconds(27);
        save.SaveGame();
        this.gameObject.SetActive(false);
    }
}
