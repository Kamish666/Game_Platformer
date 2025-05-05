using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorColors : MonoBehaviour
{

    [SerializeField] private Image _indicatorCorors;
    [SerializeField] private Sprite _red, _green, _blue;

    private void Start()
    {

        // ����� ChangeColor � ����������� �� ������� enemyColors
        ChangeColor changeColorScript = ChangeColor.instance;
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.LogWarning("ChangeColor script not found in the scene!");
        }
    }

    private void OnColorChanged(bool red, bool green, bool blue)
    {
        // �������� ������� �� ������ ����� �����
        if (red)
        {
            _indicatorCorors.sprite = _red;
        }
        else if (green)
        {
            _indicatorCorors.sprite = _green;
        }
        else
        {
            _indicatorCorors.sprite = _blue;
        }

    }
}
