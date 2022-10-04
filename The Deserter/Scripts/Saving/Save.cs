using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    private UserInput player { get { return FindObjectOfType<UserInput>(); } set { player = value; } }
    private CharacterStats playerStats { get { return FindObjectOfType<CharacterStats>(); } set { playerStats = value; } }
    private WeaponHandler wh { get { return player.GetComponent<WeaponHandler>(); } set { wh = value; } }

    public int activeScene;
    public int pistol;
    public int rifle;
    public Text missionText;


    // Start is called before the first frame update
    void Start()
    {
       activeScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Autosave", activeScene);
        PlayerPrefs.SetFloat("Health", playerStats.health);
        PlayerPrefs.SetInt("PistolCarryingAmmo", wh.weaponsList[1].ammo.carryingAmmo);
        PlayerPrefs.SetInt("PistolClipAmmo", wh.weaponsList[1].ammo.clipAmmo);
        PlayerPrefs.SetInt("RifleCarryingAmmo", wh.weaponsList[2].ammo.carryingAmmo);
        PlayerPrefs.SetInt("RifleClipAmmo", wh.weaponsList[2].ammo.clipAmmo);
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
        PlayerPrefs.SetString("MissionText", missionText.text);
    }
}
