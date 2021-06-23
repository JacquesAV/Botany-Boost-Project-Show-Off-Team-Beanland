using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer; //The audio mixer for all sounds in the game
    public TMP_Dropdown resolutionDropdowns = null;
    public TMP_Dropdown displayDropdown = null;
    public Resolution[] resolutions;
    public Toggle englishToggle = null;
    public Toggle dutchToggle = null;
    public Slider masterSlider, ambienceSlider, sfxSlider, interfaceSlider, voiceSlider, musicSlider, qualitySlider, shadowSlider;

    public void Awake()
    {
        GetValidResolutions();
        LoadPlayerPrefs();
    }

    private void LoadPlayerPrefs()
    {
        //Set the volumes
        #region Volume
        float volume = GetGivenVolume("MasterVolume");
        SetMasterVolume(volume);
        if (masterSlider != null) { masterSlider.value = volume; }

        volume = GetGivenVolume("AmbienceVolume");
        SetAmbienceVolume(volume);
        if (ambienceSlider != null) { ambienceSlider.value = volume; }

        volume = GetGivenVolume("SFXVolume");
        SetSFXVolume(volume);
        if (sfxSlider != null) { sfxSlider.value = volume; }

        volume = GetGivenVolume("InterfaceVolume");
        SetInterfaceVolume(volume);
        if (interfaceSlider != null) { interfaceSlider.value = volume; }

        volume = GetGivenVolume("VoiceVolume");
        SetVoiceVolume(volume);
        if (voiceSlider != null) { voiceSlider.value = volume; }

        volume = GetGivenVolume("MusicVolume");
        SetMusicVolume(volume);
        if (musicSlider != null) { musicSlider.value = volume; }

        //Set the display
        int intValue = GetDisplay();
        SetDisplay(intValue);
        if (displayDropdown != null) { displayDropdown.value = intValue; }
        #endregion

        //Set the resolution if saved, else should start up as default
        SetResolution(GetResolution());

        //Set the quality
        intValue = GetQuality();
        SetQuality(intValue);
        if (qualitySlider != null) { qualitySlider.value = intValue; }

        //Set the shadows
        intValue = GetShadows();
        SetShadows(intValue);
        if (shadowSlider != null) { shadowSlider.value = intValue; }

        //Update language related
        UpdateLanguageToggles();
        SetLanguage(GetLanguage());
    }

    private void GetValidResolutions()
    {
        //Error Handling
        if (resolutionDropdowns == null)
        {
            Debug.LogWarning("Missing a dropdown menu, resolutions settings will not work!");
            return;
        }

        //Get the supported screen
        resolutions = Screen.resolutions;

        //Clear old options
        resolutionDropdowns.ClearOptions();

        //Temporary list and text of the options
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            //Set the current resolution
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                //Set to the current index being explored
                currentResolutionIndex = i;
            }
        }

        //Add the options to the dropdown
        resolutionDropdowns.AddOptions(options);
        resolutionDropdowns.value = currentResolutionIndex;
        resolutionDropdowns.RefreshShownValue();

        //Print each resolution with the index
        foreach (Resolution res in resolutions)
        {
            Debug.Log("Loaded Index "+resolutions.ToList().IndexOf(res) +": "+res);
        }
    }

    #region Volume Settings
    //Changes the master volume setting
    public void SetMasterVolume(float givenValue)
    {
        SetGivenVolume("MasterVolume", givenValue);
    }
    //Changes the master volume setting
    public void SetAmbienceVolume(float givenValue)
    {
        SetGivenVolume("AmbienceVolume", givenValue);
    }
    //Changes the master volume setting
    public void SetSFXVolume(float givenValue)
    {
        SetGivenVolume("SFXVolume", givenValue);
    }
    //Changes the master volume setting
    public void SetInterfaceVolume(float givenValue)
    {
        SetGivenVolume("InterfaceVolume", givenValue);
    }
    //Changes the master volume setting
    public void SetVoiceVolume(float givenValue)
    {
        SetGivenVolume("VoiceVolume", givenValue);
    }
    //Changes the master volume setting
    public void SetMusicVolume(float givenValue)
    {
        SetGivenVolume("MusicVolume", givenValue);
    }
    //Sets volume based
    private void SetGivenVolume(string givenGroup, float givenValue)
    {
        //Set/Update the volume
        audioMixer.SetFloat(givenGroup, givenValue);

        //Set/Update the player prefs
        PlayerPrefs.SetFloat(givenGroup, givenValue);
    }
    //Returns the flaot value from the player prefs
    private float GetGivenVolume(string givenGroup)
    {
        return PlayerPrefs.GetFloat(givenGroup, 0); //Default volume 100%
    }
    #endregion

    #region Video Settings
    public void SetDisplay(int givenState)
    {
        //Set the state
        Screen.fullScreenMode = (FullScreenMode)givenState;

        Debug.Log("Display settings changed to: " + Screen.fullScreenMode);

        //Set/Update the player prefs
        PlayerPrefs.SetInt("DisplayMode", givenState);
    }
    //Returns the display value from the player prefs
    private int GetDisplay()
    {
        return PlayerPrefs.GetInt("DisplayMode", 1); //Default fullscreen window
    }
    public void SetResolution(int givenResolutionIndex)
    {
        //Temporary reference
        Resolution resolution;

        //Error handling
        try
        {
            //Get the set resolution
            resolution = resolutions[givenResolutionIndex];
        }
        catch
        {
            //Default to system in case of issues
            resolution = Screen.currentResolution;
            Debug.LogWarning("Issue found with given resolution index ("+givenResolutionIndex+"), defaulting to current resolution!");
        }

        //Set/Update the player prefs
        PlayerPrefs.SetInt("ResolutionIndex", givenResolutionIndex);

        //Set the given resolution
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Debug.Log("Resolution changed to: " + Screen.currentResolution.width +"/"+ Screen.currentResolution.height);
        
        //Ensure the dropdown menu is displaying the right item
        resolutionDropdowns.value = givenResolutionIndex;
    }
    //Returns the resolution value from the player prefs
    private int GetResolution()
    {
        Debug.Log("I'm happening with index: "+ PlayerPrefs.GetInt("ResolutionIndex", -1));
        return PlayerPrefs.GetInt("ResolutionIndex", -1);
    }
    public void SetQuality(float givenQualityIndex)
    {
        //Directly access the quality settings and set it based on the given index
        QualitySettings.SetQualityLevel((int)givenQualityIndex);

        Debug.Log("Quality settings changed to: " + QualitySettings.names[(int)givenQualityIndex]);

        //Set/Update the player prefs
        PlayerPrefs.SetInt("QualityLevel", (int)givenQualityIndex);
    }
    //Returns the quality value from the player prefs
    private int GetQuality()
    {
        return PlayerPrefs.GetInt("QualityLevel", 1);
    }
    public void SetShadows(float givenQualityIndex)
    {
        //Directly access the quality settings and set it based on the given index
        QualitySettings.shadows = (ShadowQuality)givenQualityIndex;

        Debug.Log("Shadow settings changed to: " + QualitySettings.shadows);

        //Set/Update the player prefs
        PlayerPrefs.SetInt("ShadowQuality", (int)givenQualityIndex);
    }
    //Returns the shadows value from the player prefs
    private int GetShadows()
    {
        return PlayerPrefs.GetInt("ShadowQuality", 1);
    }
    #endregion

    #region Language
    public void SetLanguage(string givenLocaleName)
    {
        //Error handling
        if (englishToggle == null || dutchToggle == null)
        {
            Debug.LogWarning("No toggles given to toggle, be sure they are present!");
            return;
        }

        switch (givenLocaleName)
        {
            //Change the locale to english
            case "English (en)":
                //Ignore if toggle was not enabled
                if (!englishToggle.isOn) { return; }

                //Set
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(SystemLanguage.English);

                //Update the player prefs
                PlayerPrefs.SetString("Language", "English (en)");
                break;

            //Change the locale to dutch
            case "Dutch (nl)":
                //Ignore if toggle was not enabled
                if (!dutchToggle.isOn) { return; }

                //Set
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(SystemLanguage.Dutch);

                //Update the player prefs
                PlayerPrefs.SetString("Language", "Dutch (nl)");
                break;

            //Default the locale to english
            default:
                Debug.LogWarning("Invalid locale name given, defaulted to english!");
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(SystemLanguage.English);
                return;
        }
        //Debug the change
        Debug.Log("Language changed to (end of setter)" + LocalizationSettings.SelectedLocale);
    }

    private string GetLanguage()
    {
        Debug.Log("Getting Language from GetLanguage(): " + PlayerPrefs.GetString("Language", "English (en)"));
        return PlayerPrefs.GetString("Language", "English (en)"); //Default english
    }
    private void UpdateLanguageToggles()
    {
        //Update the toggles active state
        switch (GetLanguage())
        {
            //If english, toggle english toggle
            case "English (en)":
                englishToggle.isOn = true;
                break;

            //If dutch, toggle dutch toggle
            case "Dutch (nl)":
                dutchToggle.isOn = true;
                break;

            //If neither, disable both
            default:
                englishToggle.isOn = false;
                dutchToggle.isOn = false;
                break;
        }
    }
    #endregion
}
