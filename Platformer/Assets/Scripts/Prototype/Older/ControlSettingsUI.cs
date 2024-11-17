using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingsUI : MonoBehaviour
{
    private ControlSettingsManager _settingsManager;

    public Button moveLeftButton;
    public Button moveRightButton;
    public Button jumpButton;
    public Button changeColorLeftButton;
    public Button changeColorRightButton;

    private KeyCode keyToChange;

    private void Start()
    {
        _settingsManager = GetComponent<ControlSettingsManager>();
        // Привязка методов к кнопкам
        moveLeftButton.onClick.AddListener(() => StartChangingKey(ref _settingsManager.controlSettings.moveLeft));
        moveRightButton.onClick.AddListener(() => StartChangingKey(ref _settingsManager.controlSettings.moveRight));
        jumpButton.onClick.AddListener(() => StartChangingKey(ref _settingsManager.controlSettings.jump));
        changeColorLeftButton.onClick.AddListener(() => StartChangingKey(ref _settingsManager.controlSettings.changeColorLeft));
        changeColorRightButton.onClick.AddListener(() => StartChangingKey(ref _settingsManager.controlSettings.changeColorRight));
    }

    private void StartChangingKey(ref KeyCode key)
    {
        keyToChange = key;
        StartCoroutine(WaitForKeyPress());
    }

    private System.Collections.IEnumerator WaitForKeyPress()
    {
        // Ожидание нажатия новой клавиши
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(k))
                    {
                        ChangeKey(k);
                        yield break; // Завершить корутину после изменения клавиши
                    }
                }
            }
            yield return null; // Ждать следующего кадра
        }
    }

    private void ChangeKey(KeyCode newKey)
    {
        // Изменение клавиши управления
        if (keyToChange == _settingsManager.controlSettings.moveLeft)
        {
            _settingsManager.controlSettings.moveLeft = newKey;
        }
        else if (keyToChange == _settingsManager.controlSettings.moveRight)
        {
            _settingsManager.controlSettings.moveRight = newKey;
        }
        else if (keyToChange == _settingsManager.controlSettings.jump)
        {
            _settingsManager.controlSettings.jump = newKey;
        }
        else if (keyToChange == _settingsManager.controlSettings.changeColorLeft)
        {
            _settingsManager.controlSettings.changeColorLeft = newKey;
        }
        else if (keyToChange == _settingsManager.controlSettings.changeColorRight)
        {
            _settingsManager.controlSettings.changeColorRight = newKey;
        }

        // Обновление текста кнопки (если нужно)
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        // Обновите текст кнопок, если у вас есть текстовые поля для отображения текущих клавиш
        moveLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = _settingsManager.controlSettings.moveLeft.ToString();
        moveRightButton.GetComponentInChildren<TextMeshProUGUI>().text = _settingsManager.controlSettings.moveRight.ToString();
        jumpButton.GetComponentInChildren<TextMeshProUGUI>().text = _settingsManager.controlSettings.jump.ToString();
        changeColorLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = _settingsManager.controlSettings.changeColorLeft.ToString();
        changeColorRightButton.GetComponentInChildren<TextMeshProUGUI>().text = _settingsManager.controlSettings.changeColorRight.ToString();
    }
}
