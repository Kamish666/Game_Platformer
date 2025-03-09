using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : Enemy
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _timeToDestroy = 5f;


    private Transform _player;
    private GameObject _bullet;

    private void Awake()
    {
        _bullet = this.gameObject;
        _player = FindAnyObjectByType<ChangeColor>()?.transform;
    }

    private void Start()
    {
        StartCoroutine(SetDestroy());
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            FollowTarget();
        }
    }

    IEnumerator SetDestroy()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        //Destroy(gameObject);
        _bullet.SetActive(false);
    }


    private void FollowTarget()
    {
        Vector2 direction = (_player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed);
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Я ребенок");
        base.OnTriggerEnter2D(collision);

        //Destroy(gameObject);
        _bullet.SetActive(false);
    }
}
