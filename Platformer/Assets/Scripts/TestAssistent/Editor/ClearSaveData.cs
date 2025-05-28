using UnityEngine;
using System.IO;
using UnityEditor;

public static class ClearSaveDataEditor
{
    [MenuItem("Tools/Clear Save Data")]
    public static void ClearAllSaves()
    {
        string filePath = Application.persistentDataPath + "/saveLevelsWin.gamesave";

        // Удаление файла сохранения
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Файл сохранений удалён: {filePath}");
        }
        else
        {
            Debug.LogWarning($"Файл сохранений не найден: {filePath}");
        }

        // Очистка PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs очищены!");
    }
}
