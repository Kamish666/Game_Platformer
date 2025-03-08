using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColors : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

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
            EnableEnemy(Color.white);
        }
        else if (blue)
        {
            EnableEnemy(new Color(0f, 0.231f, 1f));
        }
        else
        {
            EnableEnemy(Color.red);
        }
    }

    private void EnableEnemy(Color color)
    {
        _spriteRenderer.color = color;
    }
}
