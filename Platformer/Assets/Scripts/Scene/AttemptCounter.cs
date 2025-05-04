using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttemptCounter : MonoBehaviour
{
    [SerializeField] private TextMeshPro _attemptText;
    private int _attemptCount = 0;

    private void Start()
    {
        // —читываем текущее значение попыток из PlayerPrefs
        _attemptCount = PlayerPrefs.GetInt("AttemptCount", 0);
        UpdateAttemptText();

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
        PlayerPrefs.SetInt("AttemptCount", _attemptCount); // —охран€ем в PlayerPrefs
        PlayerPrefs.Save();
    }

    private void ResetAttemptCount()
    {
        _attemptCount = 0;
        PlayerPrefs.SetInt("AttemptCount", _attemptCount); // —брасываем в PlayerPrefs
        PlayerPrefs.Save();
    }

    private void UpdateAttemptText()
    {
        _attemptText.text = $"{_attemptCount}";
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
