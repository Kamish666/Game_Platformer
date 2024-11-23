using UnityEngine;
using System.IO;

public class ClearSaveData : MonoBehaviour
{

#if UNITY_EDITOR
    private string _filePath;

    [ContextMenu("CleaningSaveAwake")]
    private void Awake()
    {
        // ������������� ���� ���������� � Awake
        _filePath = Application.persistentDataPath + "/saveLevelsWin.gamesave";
        ClearAllSaves();

    }

    [ContextMenu("CleaningSave")]
    private void ClearAllSaves()
    {

        // �������� ����� ����������
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
            Debug.Log($"���� ���������� �����: {_filePath}");
        }
        else
        {
            Debug.LogWarning($"���� ���������� �� ������: {_filePath}");
        }

        // ������� PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs �������!");

    }
#endif
}
