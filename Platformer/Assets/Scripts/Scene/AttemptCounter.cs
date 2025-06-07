using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttemptCounter : MonoBehaviour
{
    public static event Action<int> OnAttemptChanged;

    private int _attemptCount;

    private void Start()
    {
        _attemptCount = PlayerPrefs.GetInt("AttemptCount", 0);
        OnAttemptChanged?.Invoke(_attemptCount);

        SceneController sceneController = SceneController.instance;

        if (sceneController != null)
        {
            sceneController.OnLevelRestart += IncrementAttemptCount;
            sceneController.OnLevelExit += ResetAttemptCount;
        }
    }

    private void IncrementAttemptCount()
    {
        _attemptCount++;
        PlayerPrefs.SetInt("AttemptCount", _attemptCount);
        PlayerPrefs.Save();
        OnAttemptChanged?.Invoke(_attemptCount);
    }

    private void ResetAttemptCount()
    {
        _attemptCount = 0;
        PlayerPrefs.SetInt("AttemptCount", _attemptCount);
        PlayerPrefs.Save();
        OnAttemptChanged?.Invoke(_attemptCount);
    }

    private void OnDestroy()
    {
        SceneController sceneController = SceneController.instance;

        if (sceneController != null)
        {
            sceneController.OnLevelRestart -= IncrementAttemptCount;
            sceneController.OnLevelExit -= ResetAttemptCount;
        }
    }
}