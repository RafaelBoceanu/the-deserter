using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public int sceneToLoad = 1; 
    public GameObject loadButton;
    public int loadInt;
    public float loadHealth;
    public float x;
    public float y;
    public float z;
    public int loadPistolClipAmmo;
    public int loadPistolCarryingAmmo;
    public int loadRifleClipAmmo;
    public int loadRifleCarryingAmmo;

    public SceneFader sceneFader;

    void Start()
    {
        loadInt = PlayerPrefs.GetInt("Autosave");
        loadHealth = PlayerPrefs.GetFloat("Health");
        loadPistolClipAmmo = PlayerPrefs.GetInt("PistolClipAmmo");
        loadPistolCarryingAmmo = PlayerPrefs.GetInt("PistolCarryingAmmo");
        loadRifleClipAmmo = PlayerPrefs.GetInt("RifleClipAmmo");
        loadRifleCarryingAmmo = PlayerPrefs.GetInt("RifleCarryingAmmo");
        x = PlayerPrefs.GetFloat("PlayerX");
        y = PlayerPrefs.GetFloat("PlayerY");
        z = PlayerPrefs.GetFloat("PlayerZ");
        if (loadInt != 0)
        {
            loadButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        sceneFader.FadeTo(sceneToLoad);
        PlayerPrefs.SetInt("Autosave", 0);
        PlayerPrefs.SetFloat("Health", 100);
        PlayerPrefs.SetInt("PistolCarryingAmmo", 30);
        PlayerPrefs.SetInt("PistolClipAmmo", 10);
        PlayerPrefs.SetInt("RifleCarryingAmmo", 100);
        PlayerPrefs.SetInt("RifleClipAmmo", 25);
        PlayerPrefs.SetString("MissionText", "Find a way out of the woods");
        PlayerPrefs.SetFloat("PlayerX", 434);
        PlayerPrefs.SetFloat("PlayerY", 99.142f);
        PlayerPrefs.SetFloat("PlayerZ", 1511.91f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadGame()
    {
        sceneFader.FadeTo(loadInt);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Credits()
    {
        sceneFader.FadeTo(5);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
