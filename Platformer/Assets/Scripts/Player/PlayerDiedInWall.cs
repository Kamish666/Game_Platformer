using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedInWall : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionMask; // Маска объектов, с которыми проверяется пересечение
    [Range(0f, 1f)]
    [SerializeField] private float _deathDive = 0.5f; // Порог, после которого игрок умирает

    private Collider2D _playerCollider;
    private Health _health;

    void Start()
    {
        // Найти ChangeColor и подписаться на событие enemyColors
        ChangeColor changeColorScript = FindObjectOfType<ChangeColor>();
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }

        _health = GetComponent<Health>();

        _playerCollider = GetComponent<Collider2D>();
    }

    private void OnColorChanged(bool green, bool red, bool blue)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_playerCollider.bounds.center, _playerCollider.bounds.size, 0, _collisionMask);

        float overlapArea = 0f;
        foreach (var col in colliders)
        {
            if (col == _playerCollider) continue;
            overlapArea += CalculateOverlapArea(_playerCollider, col);
        }

        float playerArea = _playerCollider.bounds.size.x * _playerCollider.bounds.size.y;
        float overlapPercentage = overlapArea / playerArea;

        if (overlapPercentage >= _deathDive)
        {
            Die();
        }
        else if (overlapPercentage > 0)
        {
            PushOut();
        }
    }

    private float CalculateOverlapArea(Collider2D colA, Collider2D colB)
    {
        Bounds a = colA.bounds;
        Bounds b = colB.bounds;

        float xOverlap = Mathf.Max(0, Mathf.Min(a.max.x, b.max.x) - Mathf.Max(a.min.x, b.min.x));
        float yOverlap = Mathf.Max(0, Mathf.Min(a.max.y, b.max.y) - Mathf.Max(a.min.y, b.min.y));

        return xOverlap * yOverlap;
    }

    private void Die()
    {
        //Debug.Log("Player Died");
        //_health.HandleDeath();
        _health.HandleDeath();
    }

    private void PushOut()
    {
/*        Debug.Log("Player Pushed Out");
        // Выталкивание игрока за пределы пересекаемого объекта
        Collider2D closestCollider = null;
        float maxOverlap = 0;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(_playerCollider.bounds.center, _playerCollider.bounds.size, 0, _collisionMask);
        foreach (var col in colliders)
        {
            if (col == _playerCollider) continue;
            float overlap = CalculateOverlapArea(_playerCollider, col);
            if (overlap > maxOverlap)
            {
                maxOverlap = overlap;
                closestCollider = col;
            }
        }

        if (closestCollider != null)
        {
            Vector2 pushDirection = (transform.position - closestCollider.bounds.center).normalized;
            transform.position += (Vector3)pushDirection * 0.1f; // Выталкивание
        }*/
    }
}