using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Bullet : MonoBehaviour
{

    [SerializeField] private float _timeToDestroy = 10f;
    //private GameObject _bullet;
    [SerializeField] private GameObject _bulletPS;


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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PoolerBulletsAndPS.instance.SpawnFromPool(_bulletPS.name, transform.position, transform.rotation);

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
