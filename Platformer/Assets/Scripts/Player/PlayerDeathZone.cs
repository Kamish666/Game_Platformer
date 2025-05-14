using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionMask;

    private GameObject _player;
    private Health _health;

    private void Start()
    {
        _player = GetComponentInParent<PlayerColors>().gameObject;
        _health = GetComponentInParent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collider2D collider)
    {
        //Debug.Log("PlayerDeathZone " + collider.name);
        if (((1 << collider.gameObject.layer) & _collisionMask) != 0)
        {
            _health.HandleDeath();
        }
    }
}
