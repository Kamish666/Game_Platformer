using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float _elapsedTime; // �����, ��������� � ������ ������

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
        // �������������� ������� � ������� � ���� �������
        string minutes = Mathf.Floor(_elapsedTime / 60).ToString("00");
        string seconds = (_elapsedTime % 60).ToString("00");
        string milliseconds = ((_elapsedTime * 100) % 100).ToString("00");

        // ���������� ������ �������
        timerText.text = $"{minutes}:{seconds}:{milliseconds}";
    }
}
