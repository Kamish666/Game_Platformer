using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ChangeColor : MonoBehaviour
{

    public InputActionReference changeColor;

    private int _currentColorIndex = 0; // 0: �������, 1: �������, 2: �����

    private bool[] _activeColorsM = { true, false, false}; // 0: �������, 1: �������, 2: �����

    private GameObject[] _colorBlocs;

    public delegate void Colors(bool green, bool red, bool blue);
    public event Colors enemyColors;

    private void Start()
    {
        _colorBlocs = new GameObject[] { GameObject.Find("GreenBlocks"), GameObject.Find("RedBlocks"), GameObject.Find("BlueBlocks")};

        UpdateColorVisibility();

        FindObjectOfType<PlayerMovement>().GetComponent<Health>().OnDie += DeactiveScript;
    }



    private void OnEnable()
    {
        //Debug.Log("OnEnable");
        //changeColor.action.Enable();
        OnChangeColor();
    }


    private void OnChangeColor()
    {
        //Debug.Log("OnChangeColor");

        float changeInput = changeColor.action.ReadValue<float>();

        if (changeInput > 0)
        {
            ChangeColorIndex(1); // ����� ��������� ����� �����
        }
        else if (changeInput < 0)
        {
            ChangeColorIndex(-1); // ����� ��������� ����� �����
        }
    }






    /*    private void OnEnable()
        {
            // ������������� �� ������� started
            changeColor.action.Enable();
            changeColor.action.started += OnChangeColor;
        }

        private void OnDisable()
        {
            // ������������ �� �������
            changeColor.action.started -= OnChangeColor;
            changeColor.action.Disable();
        }


        private void OnChangeColor(InputAction.CallbackContext context)
        {
            // �������� �������� ���
            float changeInput = context.ReadValue<float>();

            if (changeInput > 0)
            {
                ChangeColorIndex(1); // ����� ��������� ����� �����
            }
            else if (changeInput < 0)
            {
                ChangeColorIndex(-1); // ����� ��������� ����� �����
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
        GetComponent<ChangeColor>().enabled = false;

        changeColor.action.Disable();
    }
}
