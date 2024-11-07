using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float _damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            collision.GetComponent<Health>().TakeDamage(_damage);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(_damage);
        }
    }
}
