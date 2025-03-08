using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private SaveLevelsData _saveLvlData;

    [SerializeField] private int _countLevels = 3;
    [SerializeField] private int _prologIndex;

    private void Start()
    {
        _saveLvlData = GetComponent<SaveLevelsData>();

        //PlayerPrefs.DeleteKey("FirstLaunch");

        if (!PlayerPrefs.HasKey("FirstLaunch"))
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);
            PlayerPrefs.Save();

            Debug.Log("«¿œ”—  œ–ŒÀŒ√¿");

            LoadScene(_prologIndex);
        }
    }

    public void PlayGame()
    {
        DataLevels data;
        data = _saveLvlData.LoadGame();
        Debug.Log("ÃÂÌ˛   " + data);


        if (data == null)
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(data.levelsWin < _countLevels)
        {
            Debug.Log($"data.levelsWin: {data.levelsWin}  _countLevels: {_countLevels}");
            LoadScene(data.levelsWin + 1);
        }
        else
            LoadScene(data.levelsWin);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
}
