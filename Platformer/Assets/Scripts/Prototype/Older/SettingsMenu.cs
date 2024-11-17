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
        // ������� ���� ��� �������� ���������
    }

    public void CancelSettings()
    {
        _settingsManager.LoadSettings();
        // ������� ���� ��� �������� ���������
    }

    public void ResetSettings()
    {
        _settingsManager.ResetToDefaults();
        // �������� ���������
    }
}

