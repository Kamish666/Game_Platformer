using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public delegate void LevelAction();
    public event LevelAction OnLevelRestart;
    public event LevelAction OnLevelExit;

    private SaveLevelsData _saveLvlData;

    private void Start()
    {
        FindObjectOfType<PlayerMovement>().GetComponent<Health>().OnDie += RestartSceneAfterDied;

        _saveLvlData = GetComponent<SaveLevelsData>();
    }

    private void RestartSceneAfterDied()
    {
        OnLevelRestart?.Invoke();
        StartCoroutine(RestartSceneCoroutine());
    }

    private static IEnumerator RestartSceneCoroutine()
    {
        yield return new WaitForSeconds(3f); // ���� n ������
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ������������� ������� �����
    }


    public void LoadNextScene()
    {
        // ��������� ������ ������� �����
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        DataLevels data;

        data = _saveLvlData.LoadGame();
        if (data == null || data.levelsWin < currentSceneIndex)
        {
            data = new DataLevels();
            data.levelsWin = currentSceneIndex;
            _saveLvlData.SaveGame(data);
        }

        OnLevelExit?.Invoke();

        // ��������, ���������� �� ��������� �����
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // �������� ��������� �����
        }
        else
        {
            SceneManager.LoadScene(0); // �������� ������� ����
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        OnLevelExit?.Invoke();
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        OnLevelRestart?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
 