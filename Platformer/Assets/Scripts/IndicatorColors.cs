using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorColors : MonoBehaviour
{

    [SerializeField] private Image _indicatorCorors;
    [SerializeField] private Sprite _green, _red, _blue;

    private void Start()
    {

        // Найти ChangeColor и подписаться на событие enemyColors
        ChangeColor changeColorScript = FindObjectOfType<ChangeColor>();
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.LogWarning("ChangeColor script not found in the scene!");
        }
    }

    private void OnColorChanged(bool green, bool red, bool blue)
    {
        // Проверка условий на основе цвета врага
        if (green)
        {
            _indicatorCorors.sprite = _green;
        }
        else if (blue)
        {
            _indicatorCorors.sprite = _blue;
        }
        else
        {
            _indicatorCorors.sprite = _red;
        }
    }
}
