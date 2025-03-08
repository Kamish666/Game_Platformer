using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float _elapsedTime; // Время, прошедшее с начала уровня

    void Start()
    {
        _elapsedTime = 0f;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        // Форматирование времени в секунды и доли секунды
        string minutes = Mathf.Floor(_elapsedTime / 60).ToString("00");
        string seconds = (_elapsedTime % 60).ToString("00");
        string milliseconds = ((_elapsedTime * 100) % 100).ToString("00");

        // Обновление текста таймера
        timerText.text = $"{minutes}:{seconds}:{milliseconds}";
    }
}
