using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resDrpDwn;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        resDrpDwn.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndx = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndx = i;
            }
        }

        resDrpDwn.AddOptions(options);
        resDrpDwn.value = currentResIndx;
        resDrpDwn.RefreshShownValue();

    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQualiy (int qualIndx)
    {
        QualitySettings.SetQualityLevel(qualIndx);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution (int resIndx)
    {
        Resolution resolution = resolutions[resIndx];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    }
}
