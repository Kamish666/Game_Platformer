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
        yield return new WaitForSeconds(3f); // Ждем n секунд
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезапускаем текущую сцену
    }


    public void LoadNextScene()
    {
        // Получение индекс текущей сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        OnLevelExit?.Invoke();

        // Проверка, существует ли следующая сцена
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // Загрузка следующую сцену
        }
        else
        {
            SceneManager.LoadScene(0); // Загрузка главное меню
        }
    }

/*    public void ExitLevel()
    {
        OnLevelExit?.Invoke();
    }*/

}
 