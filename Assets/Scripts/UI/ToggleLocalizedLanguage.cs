using UnityEngine;
using UnityEngine.Localization.Settings;

public class ToggleLocalizedLanguage : MonoBehaviour
{
    public void ToggleLanguage()
    {
        //Distinguish between the current localization settings inverted
        switch (LocalizationSettings.SelectedLocale.ToString())
        {
            //If already english, change to dutch
            case "English (en)":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(SystemLanguage.Dutch);
                break;

            //If already dutch, change to english
            case "Dutch (nl)":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(SystemLanguage.English);
                break;
        }

        //Debug the change
        Debug.Log("Language changed to " + LocalizationSettings.SelectedLocale);
    }
}
