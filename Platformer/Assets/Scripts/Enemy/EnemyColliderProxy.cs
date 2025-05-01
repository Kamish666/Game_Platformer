using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderProxy : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemy?.HandleCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _enemy?.HandleCollision(collision.collider);
    }
}
