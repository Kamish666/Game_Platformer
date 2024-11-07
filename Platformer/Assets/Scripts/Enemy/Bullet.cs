using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : Enemy
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _timeToDestroy = 10f;

    private void Start()
    {
        StartCoroutine(SetDestroy());
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.down * _speed);
    }

    IEnumerator SetDestroy()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Я ребенок");
        base.OnTriggerEnter2D (collision);
        Destroy(gameObject);
    }
}
