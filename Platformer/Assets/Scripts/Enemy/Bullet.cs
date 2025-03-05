using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : Enemy
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _timeToDestroy = 10f;
    private GameObject _bullet;

    private void Awake()
    {
        //Debug.Log("Метод успешно отработал");
        _bullet = this.gameObject;
    }

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


        //Destroy(gameObject);
        _bullet.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Я ребенок");
        base.OnTriggerEnter2D (collision);

        //Destroy(gameObject);
        _bullet.SetActive(false);
    }
}
