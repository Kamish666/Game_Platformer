using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColors : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    // ���������� ��� ������� �����
    private bool _isGreenEnemy = true, _isBlueEnemy = true, _isRedEnemy = true;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // ����� ChangeColor � ����������� �� ������� enemyColors
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

    private void OnColorChanged(bool green, bool blue, bool red)
    {
        // �������� ������� �� ������ ����� �����
        if (_isGreenEnemy && green)
        {
            EnableEnemy(Color.white);
        }
        else if (_isBlueEnemy && blue)
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
