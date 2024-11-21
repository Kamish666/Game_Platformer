using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChoiceMenu : MonoBehaviour
{
    private SaveLevelsData _saveLvlData;

    [SerializeField] private Button[] _lvls;

    private void Start()
    {
        _saveLvlData = GetComponent<SaveLevelsData>();

        DataLevels data;

        data = _saveLvlData.LoadGame();

        if (data != null)
        {
            for (int i = 0; i < _lvls.Length; i++)
            {
                if (i <= data.levelsWin)
                    _lvls[i].interactable = true;
                else
                    _lvls[i].interactable = false;

            }
        }
        else
        {
            for (int i = 0; i < _lvls.Length; i++)
            {
                _lvls[i].interactable = false;
            }
            _lvls[0].interactable = true;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        Debug.Log($"Loading level: {levelIndex}");
        SceneManager.LoadScene(levelIndex);
    }

    public void Delete()
    {
        DataLevels data = new DataLevels();
        data.levelsWin = 1;
        _saveLvlData.SaveGame(data);
    }

}
