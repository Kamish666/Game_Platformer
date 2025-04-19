using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelListLoader : MonoBehaviour
{
    public string levelsFolder = "Levels";  // Папка, где хранятся уровни
    public GameObject levelItemPrefab;      // Твой префаб ChoiceLevelElementPrefab
    public Transform contentParent;         // Контейнер внутри ScrollView

    public string playSceneName = "GameScene";    // Сцена для игры
    public string editSceneName = "EditorScene";  // Сцена для редактирования

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

            // Кнопка "Играть"
            item.transform.Find("Play").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("SelectedLevel", levelName);
                SceneManager.LoadScene(playSceneName);
            });

            // Кнопка "Редактировать"
            item.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("SelectedLevel", levelName);
                SceneManager.LoadScene(editSceneName);
            });

            // Кнопка "Удалить"
            item.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(() =>
            {
                Directory.Delete(Path.Combine(Application.persistentDataPath, levelsFolder, levelName), true);
                Destroy(item);
            });
        }
    }
}
