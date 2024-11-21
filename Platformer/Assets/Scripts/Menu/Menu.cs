using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private SaveLevelsData _saveLvlData;

    [SerializeField] private int _countLevels = 3;

    private void Start()
    {
        _saveLvlData = GetComponent<SaveLevelsData>();
    }

    public void PlayGame()
    {
        DataLevels data;
        data = _saveLvlData.LoadGame();
        Debug.Log("Μενώ   " + data);


        if (data == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(data.levelsWin < _countLevels)
        {
            Debug.Log($"data.levelsWin: {data.levelsWin}  _countLevels: {_countLevels}");
            SceneManager.LoadScene(data.levelsWin + 1);
        }
        else
            SceneManager.LoadScene(data.levelsWin);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
