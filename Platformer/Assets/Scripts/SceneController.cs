using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public delegate void LevelAction();
    public event LevelAction OnLevelRestart;
    public event LevelAction OnLevelExit;

    private void Start()
    {
        FindObjectOfType<PlayerMovement>().GetComponent<Health>().OnDie += RestartScene;


    }

    public void RestartScene()
    {
        OnLevelRestart?.Invoke();
        StartCoroutine(RestartSceneCoroutine());
    }

    public static IEnumerator RestartSceneCoroutine()
    {
        yield return new WaitForSeconds(3f); // ���� n ������
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ������������� ������� �����
    }


    public void LoadNextScene()
    {
        // ��������� ������ ������� �����
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

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

/*    public void ExitLevel()
    {
        OnLevelExit?.Invoke();
    }*/

}
 