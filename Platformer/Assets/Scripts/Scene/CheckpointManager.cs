using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3? checkpointPosition;

    public static event Action<string> OnCheckpointActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("CheckpointID"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");

            string checkpointID = PlayerPrefs.GetString("CheckpointID");

            OnCheckpointActive?.Invoke(checkpointID);
            checkpointPosition = new Vector3(x, y, z);

            if (SceneController.instance != null)
            {
                SceneController.instance.OnLevelExit += ClearCheckpoint;
            }

            ChangeColor.instance.transform.position = checkpointPosition.Value;
        }
    }


    public void ActivateCheckpoint(Vector3 position, string checkpointID)
    {
        PlayerPrefs.SetFloat("CheckpointX", position.x);
        PlayerPrefs.SetFloat("CheckpointY", position.y);
        PlayerPrefs.SetFloat("CheckpointZ", position.z);
        PlayerPrefs.SetString("CheckpointID", checkpointID);
        PlayerPrefs.Save();

        OnCheckpointActive?.Invoke(checkpointID);
    }

    public void ClearCheckpoint()
    {
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("CheckpointID");
    }


    private void OnDisable()
    {
        if (SceneController.instance != null)
            SceneController.instance.OnLevelExit -= ClearCheckpoint;
    }
}