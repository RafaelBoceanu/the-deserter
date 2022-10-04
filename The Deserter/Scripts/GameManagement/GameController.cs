using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController GC;

    public UserInput player { get { return FindObjectOfType<UserInput>(); } set { player = value; } }

    private CharacterStats playerStats { get { return player.GetComponent<CharacterStats>(); } set { playerStats = value; } }

    private PlayerUI playerUI { get { return FindObjectOfType<PlayerUI>(); } set { playerUI = value; } }

    private WeaponHandler wh { get { return player.GetComponent<WeaponHandler>(); } set { wh = value; } }

    public Weapon pistol;
    public Weapon rifle;

    void Awake()
    {
        if (GC == null)
        { 
            GC = this;
        }
        else
        {
            if (GC != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (player)
        {
            if (playerUI)
            {
                if (wh)
                {
                    if (playerUI.ammoText)
                    {
                        if (wh.currentWeapon.weaponType == 0)
                        {
                            playerUI.ammoText.text = "Unarmed";
                            playerUI.hand.SetActive(true);
                            playerUI.pistol.SetActive(false);
                            playerUI.rifle.SetActive(false);
                        }
                        else if (wh.currentWeapon.weaponType == (Weapon.WeaponType)1 && pistol.gameObject.activeInHierarchy == true)
                        {
                            playerUI.ammoText.text = wh.currentWeapon.ammo.clipAmmo + "/" + wh.currentWeapon.ammo.carryingAmmo;
                            playerUI.pistol.SetActive(true);
                            playerUI.hand.SetActive(false);
                            playerUI.rifle.SetActive(false);
                        }
                        else if (wh.currentWeapon.weaponType == (Weapon.WeaponType)2 && rifle.gameObject.activeInHierarchy == true)
                        {
                            playerUI.ammoText.text = wh.currentWeapon.ammo.clipAmmo + "/" + wh.currentWeapon.ammo.carryingAmmo;
                            playerUI.rifle.SetActive(true);
                            playerUI.hand.SetActive(false);
                            playerUI.pistol.SetActive(false);
                        }
                    }
                }

                if (playerUI.healthBar)
                {
                    playerUI.healthBar.value = playerStats.health;
                }
            }
        }
    }
}
