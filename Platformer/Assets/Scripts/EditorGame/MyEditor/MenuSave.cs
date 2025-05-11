using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class MenuSave : MonoBehaviour
{
    public delegate void SaveAndLoad(string path);
    public event SaveAndLoad OnSave;
    public event SaveAndLoad OnLoad;

    [SerializeField] private TMP_InputField levelNameInput;
    [SerializeField] private GameObject overwriteWarningWindow;
    [SerializeField] private GameObject emptyNameWarningWindow;
    [SerializeField] private string levelsFolder = "Levels";

    private string _fullCurrentPath;
    private string _currentPath;

    private void Awake()
    {
        var handlers = GetComponents<ISaveHandler>();
        foreach (var handler in handlers)
        {
            OnSave += handler.Save;
            OnLoad += handler.Load;
        }


        string levelName = PlayerPrefs.GetString("SelectedLevel");
        //Debug.Log(levelName);
        if (levelName != "")
        {
            GetPath(levelName);
            OnLoad?.Invoke(_currentPath);
        }

        Debug.Log("MenuSave");
    }


    // ���������� ��� ������� ������ "���������"
    public void TrySaveLevel()
    {
        string levelName = levelNameInput.text.Trim();

        if (string.IsNullOrEmpty(levelName))
        {
            emptyNameWarningWindow.SetActive(true);
            return;
        }

        GetPath(levelName);

        if (Directory.Exists(_fullCurrentPath))
        {
            overwriteWarningWindow.SetActive(true);
        }
        else
        {
            SaveLevel();
        }
    }

    // ���������� �� UI, ���� ����� ����������� ������������
    public void ConfirmOverwrite()
    {
        SaveLevel();
        overwriteWarningWindow.SetActive(false);
    }

    private void SaveLevel()
    {
        // ������� ����� ���� � ���
        if (!Directory.Exists(_fullCurrentPath))
            Directory.CreateDirectory(_fullCurrentPath);

        // �������� ���� ������������
        OnSave?.Invoke(_currentPath);
        Debug.Log($"Level saved to: {_fullCurrentPath}/");
    }

    // ���������� �� UI ��� �������� �������������� � ������������ ������
    public void CancelOverwrite()
    {
        overwriteWarningWindow.SetActive(false);
    }

    // ���������� �� UI ��� �������� ���� �� ������ ������� �����
    public void CloseEmptyNameWarning()
    {
        emptyNameWarningWindow.SetActive(false);
    }

    private void GetPath(string levelName)
    {
        _currentPath = Path.Combine(levelsFolder, levelName);
        _fullCurrentPath = Path.Combine(Application.persistentDataPath, _currentPath);
    }
}