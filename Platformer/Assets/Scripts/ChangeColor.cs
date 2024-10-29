using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    private int _currentColorIndex = 0; // 0: Зеленый, 1: Синий, 2: Красный

    private GameObject[] _colorBlocs;

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
            _colorBlocs[i].SetActive(i == _currentColorIndex);
        }
    }

    private void DeactiveScript()
    {
        GetComponent<ChangeColor>().enabled = false;
    }
}
