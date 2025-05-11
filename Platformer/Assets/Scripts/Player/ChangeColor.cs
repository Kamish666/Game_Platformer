using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ChangeColor : MonoBehaviour
{

    public InputActionReference changeColor;

    private int _currentColorIndex = 1; // 0: Красный, 1: Зеленый, 2: Синий

    private bool[] _activeColorsM = { true, false, false}; // 0: Красный, 1: Зеленый, 2: Синий

    private GameObject[] _colorBlocs;

    public delegate void Colors(bool red, bool green, bool blue);
    public event Colors enemyColors;

    public static ChangeColor instance;

    public bool IsRed => _activeColorsM[0];
    public bool IsGreen => _activeColorsM[1];
    public bool IsBlue => _activeColorsM[2];


    private void Awake()
    {
        instance = this;
        Debug.Log("ChangeColor Awake");
    }

    private void Start()
    {
        _colorBlocs = new GameObject[] {GameObject.Find("RedBlocks"), GameObject.Find("GreenBlocks"),  GameObject.Find("BlueBlocks")};

        UpdateColorVisibility();

        GetComponent<Health>().OnDie += DeactiveScript;
        Debug.Log("ChangeColor Start");

    }



    /*    private void OnEnable()
        {
            //Debug.Log("OnEnable");
            //changeColor.action.Enable();
            OnChangeColor();
        }*/


    private void OnChangeColor()
    {
        //Debug.Log("OnChangeColor");

        float changeInput = changeColor.action.ReadValue<float>();

        if (changeInput > 0)
        {
            ChangeColorIndex(-1); // Вызов изменения цвета вперёд
        }
        else if (changeInput < 0)
        {
            ChangeColorIndex(1); // Вызов изменения цвета назад
        }
    }






    /*    private void OnEnable()
        {
            // Подписываемся на событие started
            changeColor.action.Enable();
            changeColor.action.started += OnChangeColor;
        }

        private void OnDisable()
        {
            // Отписываемся от события
            changeColor.action.started -= OnChangeColor;
            changeColor.action.Disable();
        }


        private void OnChangeColor(InputAction.CallbackContext context)
        {
            // Получаем значение оси
            float changeInput = context.ReadValue<float>();

            if (changeInput > 0)
            {
                ChangeColorIndex(1); // Вызов изменения цвета вперёд
            }
            else if (changeInput < 0)
            {
                ChangeColorIndex(-1); // Вызов изменения цвета назад
            }
        }*/


    /*    private void Update()
        {
            _changeInput = changeColor.action.ReadValue<float>();

            if (_changeInput == -1)
            {
                ChangeColorIndex(1);
            }

            if (_changeInput == 1)
            {
                ChangeColorIndex(-1);
            }
        }*/



    /*    private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                ChangeColorIndex(1);
            }

            if (Input.GetMouseButtonDown(1))
            {
                ChangeColorIndex(-1);
            }

        }
    */

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
        ChangeColor.instance.enabled = false;

        changeColor.action.Disable();
    }
}
