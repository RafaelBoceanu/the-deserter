using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    public GameObject visuals;
    public Animator playerAnimator;
    public CharacterStats characterStats;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
            visuals.SetActive(false);
            playerAnimator.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            visuals.SetActive(true);
            playerAnimator.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Retry()
    {
        Toggle();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        characterStats.SpawnAtNewSpawnpoint();
    }

    public void Menu()
    {
        Toggle();
        SceneManager.LoadScene("MainMenuScene");
    }

}

