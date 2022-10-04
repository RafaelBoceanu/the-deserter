using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public CharacterStats characterStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Collided with player");
            if (characterStats.health >= 100)
            {
                Debug.Log("Full health:");
                return;
            }

            else if (characterStats.health < 100)
            {
                if (100 - characterStats.health >= 20)
                    characterStats.health += 20;
                else
                    characterStats.health += 100 - characterStats.health;
                this.gameObject.SetActive(false);
            }
        }
    }
}
