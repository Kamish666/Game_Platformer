using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using static UnityEngine.Rendering.DebugUI;

public class TransparencyMenu : MonoBehaviour
{
    [SerializeField] private Slider _transparencyObjectSlider, _transparencyPlatformSlider;
    [SerializeField] private Toggle _transparencyToggle;

    private string _objectKey = "TransparencyObjectValue", _platformKey = "TransparencyPlatformValue",
        _toggleKey = "TransparencyPlatformType";


    private void Start()
    {
        if (!PlayerPrefs.HasKey(_objectKey))
            PlayerPrefs.SetInt(_objectKey, 16);
        if (!PlayerPrefs.HasKey(_platformKey))
            PlayerPrefs.SetInt(_platformKey, 16);
        if (!PlayerPrefs.HasKey(_toggleKey))
            PlayerPrefs.SetInt(_toggleKey, 1);


        _transparencyObjectSlider.value = PlayerPrefs.GetInt(_objectKey);
        _transparencyPlatformSlider.value = PlayerPrefs.GetInt(_platformKey);
        _transparencyToggle.isOn = PlayerPrefs.GetInt(_toggleKey) == 1;

        // Подписываемся на события изменения значений слайдеров
        _transparencyObjectSlider.onValueChanged.AddListener(OnTransparencyObjectValueChange);
        _transparencyPlatformSlider.onValueChanged.AddListener(OnTransparencyPlatformValueChange);
        _transparencyToggle.onValueChanged.AddListener(OnTransparencyPlatformType);
    }

    private void OnTransparencyObjectValueChange(float value)
    {
        PlayerPrefs.SetInt(_objectKey, (int)value);
        PlayerPrefs.Save();
        //Debug.Log("TransparencyObjectValue changed to: " + value);
    }

    // Метод для обработки изменения громкости звуковых эффектов
    private void OnTransparencyPlatformValueChange(float value)
    {
        PlayerPrefs.SetInt(_platformKey, (int)value);
        PlayerPrefs.Save();
        //Debug.Log("TransparencyPlatformValue changed to: " + value);
    }

    private void OnTransparencyPlatformType(bool type)
    {
        int value = type ? 1 : 0;
        PlayerPrefs.SetInt(_toggleKey, value);
    }

    private void OnDestroy()
    {
        _transparencyObjectSlider.onValueChanged.RemoveListener(OnTransparencyObjectValueChange);
        _transparencyPlatformSlider.onValueChanged.RemoveListener(OnTransparencyPlatformValueChange);
    }
}

