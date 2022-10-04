using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon;
    public WeaponHandler weaponHandler;
    public AudioSource pickUpSound;
    public GameObject fakeWeapon;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weapon.gameObject.SetActive(true);
            weaponHandler.currentWeapon = weapon;
            weaponHandler.currentWeapon.SetEquipped(true);
            pickUpSound.Play();
            fakeWeapon.SetActive(false);
        }
    }
}
