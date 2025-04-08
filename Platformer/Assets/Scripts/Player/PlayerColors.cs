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

    private void OnColorChanged(bool red, bool green, bool blue)
    {
        // Проверка условий на основе цвета врага
        if (red)
        {
            EnableEnemy(Color.red);
        }
        else if (green)
        {
            EnableEnemy(Color.white);
        }
        else
        {
            EnableEnemy(new Color(0f, 0.231f, 1f));
        }

    }

    private void EnableEnemy(Color color)
    {
        _spriteRenderer.color = color;
    }
}
