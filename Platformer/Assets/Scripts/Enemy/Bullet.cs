using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Enemy
{

    [SerializeField] private float _timeToDestroy = 10f;
    //private GameObject _bullet;
    [SerializeField] private GameObject _bulletPS;

    protected void Awake()
    {
        //Debug.Log("Метод успешно отработал");
        //_bullet = this.gameObject;
    }

    private void Start()
    {
        StartCoroutine(SetDestroy());
    }



    IEnumerator SetDestroy()
    {
        yield return new WaitForSeconds(_timeToDestroy);


        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Я ребенок");
        base.OnTriggerEnter2D(collision);

        PoolerBulletsAndPS.instance.SpawnFromPool(_bulletPS.name, transform.position, transform.rotation);

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
