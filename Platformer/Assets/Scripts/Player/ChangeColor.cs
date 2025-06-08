using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Tilemaps;

public class ChangeColor : MonoBehaviour
{

    public InputActionReference changeColor;

    private int _currentColorIndex = 1; // 0: Красный, 1: Зеленый, 2: Синий

    private bool[] _activeColorsM = { true, false, false }; // 0: Красный, 1: Зеленый, 2: Синий

    private TilemapSetting[] _tilemaps;

    public delegate void Colors(bool red, bool green, bool blue);
    public event Colors enemyColors;

    public static ChangeColor instance;

    private string _platformKey = "TransparencyPlatformValue", _toggleKey = "TransparencyPlatformType";
    private int _maxValue = 20;
    private float _currentValue;

    int _sortedOrderTilemap;

    public bool IsRed => _activeColorsM[0];
    public bool IsGreen => _activeColorsM[1];
    public bool IsBlue => _activeColorsM[2];


    private void Awake()
    {
        instance = this;
        //Debug.Log("ChangeColor Awake");

        _currentValue = CalculateValue(PlayerPrefs.GetInt(_platformKey));

        int value = PlayerPrefs.GetInt(_toggleKey);
        if (value == 0)
            _sortedOrderTilemap = 9;
        else
            _sortedOrderTilemap = 11;
    }

    private void Start()
    {
        List<Tilemap> colorBlocs = FindObjectsOfType<Tilemap>().ToList();

        for (int i = colorBlocs.Count - 1; i >= 0; i--)
        {
            if (colorBlocs[i].name == "Black")
                colorBlocs.RemoveAt(i);
        }

        colorBlocs = colorBlocs.OrderByDescending(t => t.name).ToList();

        _tilemaps = new TilemapSetting[colorBlocs.Count];

        for (int i = 0; i < colorBlocs.Count; i++)
        {
            _tilemaps[i] = new TilemapSetting();
            _tilemaps[i].tilemap = colorBlocs[i];
            _tilemaps[i].renderer = colorBlocs[i].GetComponent<TilemapRenderer>();
            _tilemaps[i].collider = colorBlocs[i].GetComponent<Collider2D>();
        }

        UpdateColorVisibility();

        GetComponent<Health>().OnDie += DeactiveScript;
        //Debug.Log("ChangeColor Start");

    }

    private float CalculateValue(float value) => 1 - value/_maxValue;

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

    private void ChangeColorIndex(int number)
    {
        _currentColorIndex = (_currentColorIndex + number + _tilemaps.Length) % _tilemaps.Length;
        UpdateColorVisibility();
    }

    private void UpdateColorVisibility()
    {
        for (int i = 0; i < _tilemaps.Length; i++)
        {
            bool acitiv = i == _currentColorIndex;

            Color color = _tilemaps[i].tilemap.color;

            if (acitiv)
            {
                color.a = 1;

                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //ПОМЕНЯЙ ЗДЕСЬ ЗНАЧЕНИЕ НА 9 ДЛЯ ПОКАЗА
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                _tilemaps[i].renderer.sortingOrder = _sortedOrderTilemap;
            }
            else
            {
                color.a = _currentValue;

                _tilemaps[i].renderer.sortingOrder = 10;
            }

            Debug.Log(color.a);

            _tilemaps[i].tilemap.color = color;
            _tilemaps[i].collider.enabled = acitiv;

            _activeColorsM[i] = acitiv;
        }
        enemyColors?.Invoke(_activeColorsM[0], _activeColorsM[1], _activeColorsM[2]);
    }

    public void ChooceColor(string color)
    {
        if (color == "Red")
            _currentColorIndex = 0;
        else if (color == "Green")
            _currentColorIndex = 1;
        else 
            _currentColorIndex = 2;

        UpdateColorVisibility();
    }

    private void DeactiveScript()
    {
        ChangeColor.instance.enabled = false;

        changeColor.action.Disable();
    }

    private class TilemapSetting
    {
        public Tilemap tilemap;
        public TilemapRenderer renderer;
        public Collider2D collider;
    }
}