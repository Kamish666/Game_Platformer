using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    private int _currentColorIndex = 0; // 0: Зеленый, 1: Синий, 2: Красный

    private bool[] _activeColorsM = { true, false, false}; // 0: Зеленый, 1: Синий, 2: Красный

    private GameObject[] _colorBlocs;

    public delegate void Colors(bool green, bool blue, bool red);
    public event Colors enemyColors;

    private void Start()
    {
        _colorBlocs = new GameObject[] { GameObject.Find("GreenBlocks"), GameObject.Find("BlueBlocks"), GameObject.Find("RedBlocks") };

        UpdateColorVisibility();

        FindObjectOfType<PlayerMovement>().GetComponent<Health>().OnDie += DeactiveScript;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            ChangeColorIndex(1);
        }

        if (Input.GetMouseButtonDown(1)) // Правая кнопка мыши
        {
            ChangeColorIndex(-1);
        }
    }

    private void ChangeColorIndex(int number)
    {
        _currentColorIndex = (_currentColorIndex + number + _colorBlocs.Length) % _colorBlocs.Length;
        UpdateColorVisibility();
    }

    private void UpdateColorVisibility()
    {
        for (int i = 0; i < _colorBlocs.Length; i++)
        {
            bool acitiv = i == _currentColorIndex;
            _colorBlocs[i].SetActive(acitiv);
            _activeColorsM[i] = acitiv;
        }
        enemyColors?.Invoke(_activeColorsM[0], _activeColorsM[1], _activeColorsM[2]);
    }

    private void DeactiveScript()
    {
        GetComponent<ChangeColor>().enabled = false;
    }
}
