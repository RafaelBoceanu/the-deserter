using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private UserInput player { get { return FindObjectOfType<UserInput>(); } set { player = value; } }
    private WeaponHandler wh { get { return player.GetComponent<WeaponHandler>(); } set { wh = value; } }

    public enum AmmoType
    {
        Fist, PistolAmmo, RifleAmmo
    }
    public AmmoType ammoType;

    public int ammo;
    public AudioSource ammoPickup;

    void Start()
    {
        switch (ammoType)
        {
            case AmmoPickup.AmmoType.Fist:
                ammo = 0;
                break;
            case AmmoPickup.AmmoType.PistolAmmo:
                ammo = 1;
                break;
            case AmmoType.RifleAmmo:
                ammo = 2;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (wh.weaponType == 1 && wh.weaponsList[1].ammo.carryingAmmo >= wh.weaponsList[1].ammo.maxCarryingAmmo
                || wh.weaponType == 2 && wh.weaponsList[2].ammo.carryingAmmo >= wh.weaponsList[2].ammo.maxCarryingAmmo)
                return;

            else if (wh.weaponType == 1 && this.ammo == 1 && wh.weaponsList[1].ammo.carryingAmmo < wh.weaponsList[1].ammo.maxCarryingAmmo)
            {
                if (wh.weaponsList[1].ammo.maxCarryingAmmo - wh.weaponsList[1].ammo.carryingAmmo >= 10)
                    wh.weaponsList[1].ammo.carryingAmmo += 10;
                else
                    wh.weaponsList[1].ammo.carryingAmmo += wh.weaponsList[1].ammo.maxCarryingAmmo - wh.weaponsList[1].ammo.carryingAmmo;
                ammoPickup.Play();
                this.gameObject.SetActive(false);

            }

            else if (wh.weaponType == 2 && this.ammo == 2 && wh.weaponsList[2].ammo.carryingAmmo < wh.weaponsList[2].ammo.maxCarryingAmmo)
            {
                if (wh.weaponsList[2].ammo.maxCarryingAmmo - wh.weaponsList[2].ammo.carryingAmmo >= 20)
                    wh.weaponsList[2].ammo.carryingAmmo += 20;
                else
                    wh.weaponsList[2].ammo.carryingAmmo += wh.weaponsList[2].ammo.maxCarryingAmmo - wh.weaponsList[2].ammo.carryingAmmo;
                ammoPickup.Play();
                this.gameObject.SetActive(false);
            }
        }
    }
}
