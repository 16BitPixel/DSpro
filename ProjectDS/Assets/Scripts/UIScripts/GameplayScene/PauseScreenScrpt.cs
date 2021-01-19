using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DS;

public class PauseScreenScrpt : MonoBehaviour
{
    public GameObject PauseUI, settingsUI, gameplayUI;

    public GameObject gameSceneUiPanel;

    public static bool gameIsPaused;

    Keyboard kb;

    bool _backgroundHasImage;
    
    void Start()
    {
        gameIsPaused = false;

        gameplayUI.SetActive(!gameIsPaused);
        PauseUI.SetActive(gameIsPaused);
        settingsUI.SetActive(gameIsPaused);

        _backgroundHasImage = false;        

        kb = InputSystem.GetDevice<Keyboard>();

        //inputActions = new PlayerControls();


    }



    // Update is called once per frame
    void Update()
    {          
        

        if (kb.escapeKey.wasPressedThisFrame)
        {
            if (gameIsPaused) Resume();
            else Pause();
        } 


    }

    public void DisableBGImage()
    {
       if (_backgroundHasImage)
       {
            Color color = new Color(255, 255, 255, 0);
            gameSceneUiPanel.GetComponent<Image>().color = color;
            _backgroundHasImage = false;
       }

    }

    public void EnableBGImage()
    {
        if (!_backgroundHasImage)
        {
            Color color = new Color(255, 255, 255, 255);
            gameSceneUiPanel.GetComponent<Image>().color = color;
            _backgroundHasImage = true;
        }
    }

    public void Resume()
    {
        DisableBGImage();
        
        gameplayUI.SetActive(true);
        PauseUI.SetActive(false);
        settingsUI.SetActive(false);

        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        EnableBGImage();
        
        gameplayUI.SetActive(false);
        PauseUI.SetActive(true);
        settingsUI.SetActive(false);

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
        PauseUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void BackToPauseScreen()
    {
        PauseUI.SetActive(true);
        settingsUI.SetActive(false);
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
