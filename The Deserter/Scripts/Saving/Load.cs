using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    private UserInput player { get { return FindObjectOfType<UserInput>(); } set { player = value; } }
    private CharacterStats playerStats { get { return FindObjectOfType<CharacterStats>(); } set { playerStats = value; } }
    private WeaponHandler wh { get { return player.GetComponent<WeaponHandler>(); } set { wh = value; } }
    //private Text missionText { get { return FindObjectOfType<Text>(); } set { missionText = value; } }

    public Text missionText;

    // Start is called before the first frame update
    private void Start()
    {
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");

        playerStats.health = PlayerPrefs.GetFloat("Health");
        player.transform.position = new Vector3(x, y, z);
        wh.weaponsList[1].ammo.carryingAmmo = PlayerPrefs.GetInt("PistolCarryingAmmo");
        wh.weaponsList[1].ammo.clipAmmo = PlayerPrefs.GetInt("PistolClipAmmo");
        wh.weaponsList[2].ammo.carryingAmmo = PlayerPrefs.GetInt("RifleCarryingAmmo");
        wh.weaponsList[2].ammo.clipAmmo = PlayerPrefs.GetInt("RifleClipAmmo");
        missionText.text = PlayerPrefs.GetString("MissionText");
    }
}
