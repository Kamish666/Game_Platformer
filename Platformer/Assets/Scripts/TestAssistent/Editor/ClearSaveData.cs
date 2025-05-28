using UnityEngine;
using System.IO;
using UnityEditor;

public static class ClearSaveDataEditor
{
    [MenuItem("Tools/Clear Save Data")]
    public static void ClearAllSaves()
    {
        string filePath = Application.persistentDataPath + "/saveLevelsWin.gamesave";

        // �������� ����� ����������
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"���� ���������� �����: {filePath}");
        }
        else
        {
            Debug.LogWarning($"���� ���������� �� ������: {filePath}");
        }

        // ������� PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs �������!");
    }
}
