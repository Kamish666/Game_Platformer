using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColors : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    // Переменные для каждого цвета
    [SerializeField] private bool _isGreenEnemy;
    [SerializeField] private bool _isBlueEnemy;
    [SerializeField] private bool _isRedEnemy;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

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
        if (_isGreenEnemy && green)
        {
            EnableEnemy(Color.green);
        }
        else if (_isBlueEnemy && blue)
        {
            EnableEnemy(Color.blue);
        }
        else if (_isRedEnemy && red)
        {
            EnableEnemy(Color.red);
        }
        else
        {
            DisableEnemy();
        }
    }

    private void EnableEnemy(Color color)
    {
        _collider.enabled = true;
        _spriteRenderer.color = color;
        _spriteRenderer.enabled = true;
    }

    private void DisableEnemy()
    {
        _collider.enabled = false;
        _spriteRenderer.enabled = false;
    }
}
