using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelListLoader : MonoBehaviour
{
    public string levelsFolder = "Levels";  // �����, ��� �������� ������
    public GameObject levelItemPrefab;      // ���� ������ ChoiceLevelElementPrefab
    public Transform contentParent;         // ��������� ������ ScrollView

    public string playSceneName = "GameScene";    // ����� ��� ����
    public string editSceneName = "EditorScene";  // ����� ��� ��������������

    private void Start()
    {
        LoadLevels();
    }

    void LoadLevels()
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, levelsFolder)))
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, levelsFolder));

        string[] directories = Directory.GetDirectories(Path.Combine(Application.persistentDataPath, levelsFolder));

        foreach (string dir in directories)
        {
            string levelName = Path.GetFileName(dir);

            GameObject item = Instantiate(levelItemPrefab, contentParent);
            item.transform.Find("NameLevel").GetComponent<TextMeshProUGUI>().text = levelName;

            // ������ "������"
            item.transform.Find("Play").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("SelectedLevel", levelName);
                SceneManager.LoadScene(playSceneName);
            });

            // ������ "�������������"
            item.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("SelectedLevel", levelName);
                SceneManager.LoadScene(editSceneName);
            });

            // ������ "�������"
            item.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(() =>
            {
                Directory.Delete(Path.Combine(Application.persistentDataPath, levelsFolder, levelName), true);
                Destroy(item);
            });
        }
    }
}
