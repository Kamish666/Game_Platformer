using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PlayerDiedInWall : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionMask; // ����� ��������, � �������� ����������� �����������
    [Range(0f, 1f)]
    [SerializeField] private float _deathThreshold = 0.5f; // �����, ����� �������� ����� �������

    private Collider2D _playerCollider;
    private Health _health;

    private void Start()
    {
        ChangeColor changeColorScript = ChangeColor.instance;
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }

        _playerCollider = GetComponent<Collider2D>();
        _health = GetComponent<Health>();
    }

    private void OnColorChanged(bool green, bool red, bool blue)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            _playerCollider.bounds.center,
            _playerCollider.bounds.size,
            0,
            _collisionMask
        );

        foreach (var col in colliders)
        {
            if (col == _playerCollider) continue;

            var distanceInfo = Physics2D.Distance(_playerCollider, col);
            if (distanceInfo.isOverlapped)
            {
                float penetration = -distanceInfo.distance;

                // �����������: ����� �� ����������� ������� ������ (������ ��� ������)
                float minSize = Mathf.Min(_playerCollider.bounds.size.x, _playerCollider.bounds.size.y);
                float penetrationPercent = penetration / minSize;

                Debug.Log(penetrationPercent);
                if (penetrationPercent >= _deathThreshold)
                {
                    Die();
                    return;
                }
            }
        }
    }

    private void Die()
    {
        _health.HandleDeath();
    }

    private void PushOut()
    {
/*        // ������������ ������ �� ������� ������������� �������
        Collider2D closestCollider = null;
        float maxOverlap = 0f;
        Bounds playerBounds = _playerCollider.bounds;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(playerBounds.center, playerBounds.size, 0f, _collisionMask);
        foreach (var col in colliders)
        {
            if (col == _playerCollider || !col.enabled) continue;
            if (!playerBounds.Intersects(col.bounds)) continue;

            float overlap = CalculateOverlapArea(playerBounds, col.bounds);
            if (overlap > maxOverlap)
            {
                maxOverlap = overlap;
                closestCollider = col;
            }
        }

        if (closestCollider != null)
        {
            Vector2 pushDirection = (transform.position - closestCollider.bounds.center).normalized;
            transform.position += (Vector3)pushDirection * 0.1f;
        }*/
    }
}
