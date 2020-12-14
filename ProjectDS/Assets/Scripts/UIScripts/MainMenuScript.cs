using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject SettingsPanel;

    private void Start()
    {
        MainPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EnableSettingsUI()
    {
        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    //Temporarily placed here until settings UI is implemented properly
    public void BackButton()
    {
        MainPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
}
