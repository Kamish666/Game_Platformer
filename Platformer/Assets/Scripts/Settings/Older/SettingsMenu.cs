using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private ControlSettingsManager _settingsManager;


    private void Start()
    {
        _settingsManager = GetComponent<ControlSettingsManager>();
    }

    public void SaveSettings()
    {
        _settingsManager.SaveSettings();
        // Закрыть меню или обновить интерфейс
    }

    public void CancelSettings()
    {
        _settingsManager.LoadSettings();
        // Закрыть меню или обновить интерфейс
    }

    public void ResetSettings()
    {
        _settingsManager.ResetToDefaults();
        // Обновить интерфейс
    }
}

