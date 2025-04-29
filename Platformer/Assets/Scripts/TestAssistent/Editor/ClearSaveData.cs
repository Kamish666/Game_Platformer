using UnityEngine;
using System.IO;

public class ClearSaveData : MonoBehaviour
{

#if UNITY_EDITOR
    private string _filePath;

    [ContextMenu("CleaningSaveAwake")]
    private void Awake()
    {
        // Инициализация пути сохранений в Awake
        _filePath = Application.persistentDataPath + "/saveLevelsWin.gamesave";
        ClearAllSaves();

    }

    [ContextMenu("CleaningSave")]
    private void ClearAllSaves()
    {

        // Удаление файла сохранения
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
            Debug.Log($"Файл сохранений удалён: {_filePath}");
        }
        else
        {
            Debug.LogWarning($"Файл сохранений не найден: {_filePath}");
        }

        // Очистка PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs очищены!");

    }
#endif
}
