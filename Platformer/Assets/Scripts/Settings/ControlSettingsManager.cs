using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ControlSettingsManager : MonoBehaviour
{
    public ControlSettings controlSettings;

    private void Start()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(controlSettings);
        PlayerPrefs.SetString("ControlSettings", json);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("ControlSettings"))
        {
            string json = PlayerPrefs.GetString("ControlSettings");
            controlSettings = JsonUtility.FromJson<ControlSettings>(json);
        }
        else
        {
            controlSettings = new ControlSettings(); // »спользовать значени€ по умолчанию
        }
    }

    public void ResetToDefaults()
    {
        controlSettings = new ControlSettings(); // —брос к значени€м по умолчанию
    }
}

