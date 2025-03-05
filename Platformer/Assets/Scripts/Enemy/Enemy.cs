using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Enemy : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float _damage;

    [SerializeField] private bool _canTakeDamage = false;
    private GameObject _enemy;

    private void Awake()
    {
        _enemy = this.gameObject;
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D работает");
        if(_canTakeDamage && collision.GetComponent<DamageDealer>() != null)
        {
            collision.GetComponent<DamageDealer>().GetDamage(_enemy);
        }
        else if (collision.GetComponent<PlayerMovement>() != null)
        {
            collision.GetComponent<Health>().TakeDamage(_damage);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D работает");
            if (_canTakeDamage && collision.gameObject.GetComponent<DamageDealer>() != null)
            {
            Debug.Log("врак получает урон работает");
            collision.gameObject.GetComponent<DamageDealer>().GetDamage(_enemy);
            }
            else if (collision.gameObject.GetComponent<PlayerMovement>() != null)
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(_damage);
            }
    }
}*/


public class Enemy : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float _damage;

    [SerializeField] private bool _canTakeDamage = false;
    private GameObject _enemy;

    private void Awake()
    {
        _enemy = this.gameObject;
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D работает");
        if (_canTakeDamage)
        {
            VerificateCollision(collider: collision);
        }
        else if (collision.GetComponent<PlayerMovement>() != null)
        {
            collision.GetComponent<Health>().TakeDamage(_damage);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D работает");
        if (_canTakeDamage)
        {
            VerificateCollision(collision: collision);
        }
        else if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(_damage);
        }
    }

    private void VerificateCollision(Collision2D? collision = null, Collider2D? collider = null)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<DamageDealer>() != null)
            {
                Debug.Log("врак получает урон работает");
                collision.gameObject.GetComponent<DamageDealer>().GetDamage(_enemy);
            }
            else if (collision.gameObject.GetComponent<PlayerMovement>() != null)
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(_damage);
            }
        }
        else
        {
            if (_canTakeDamage && collider.GetComponent<DamageDealer>() != null)
            {
                collider.GetComponent<DamageDealer>().GetDamage(_enemy);
            }
            else if (collider.GetComponent<PlayerMovement>() != null)
            {
                collider.GetComponent<Health>().TakeDamage(_damage);
            }
        }
    }
}