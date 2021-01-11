using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenScrpt : MonoBehaviour
{
    public GameObject PausePanel, settingsPanel, gameplayPanel;

    public static bool gameIsPaused;


    void Start()
    {
        gameplayPanel.SetActive(true);
        PausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }

        }
    }

    public void Resume()
    {
        gameplayPanel.SetActive(true);
        PausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        gameplayPanel.SetActive(false);
        PausePanel.SetActive(true);
        settingsPanel.SetActive(false);

        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;

        SceneManager.LoadScene(0);
    }

    public void EnableSettingsUI()
    {
        PausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToPauseScreen()
    {
        PausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

}
