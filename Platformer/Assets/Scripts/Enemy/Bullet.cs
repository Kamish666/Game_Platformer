using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Enemy
{

    [SerializeField] private float _timeToDestroy = 10f;
    private GameObject _bullet;

    protected void Awake()
    {
        //Debug.Log("Метод успешно отработал");
        _bullet = this.gameObject;
    }

    private void Start()
    {
        StartCoroutine(SetDestroy());
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
        base.OnTriggerEnter2D(collision);

        //Destroy(gameObject);
        _bullet.SetActive(false);
    }
}
