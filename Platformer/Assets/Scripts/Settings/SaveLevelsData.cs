using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLevelsData : MonoBehaviour
{

    private string _filePath;
    private void Awake()
    {
        _filePath = Application.persistentDataPath + "/saveLevelsWin.gamesave";
    }

    public void SaveGame(DataLevels data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(_filePath);

/*        DataLevels data = new DataLevels();

        data.levelsWin = levelsWin;*/

        bf.Serialize(file, data);

        file.Close();

        Debug.Log("Game data saved!");
    }


    public DataLevels LoadGame()
    {
        if (File.Exists(_filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(_filePath, FileMode.Open);
            FileStream file = new FileStream(_filePath, FileMode.Open);

            DataLevels data = (DataLevels)bf.Deserialize(file);

            file.Close();

            Debug.Log("Game data loaded!");
            return data;
        }
        else
        {
            Debug.Log("There is no save data!");
            return null;
        }

    }
}
