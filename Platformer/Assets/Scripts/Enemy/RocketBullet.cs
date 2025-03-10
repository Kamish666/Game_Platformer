using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : Bullet
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 200f;


    private Transform _player;

    new private void Awake()
    {
        base.Awake();
        _player = FindAnyObjectByType<ChangeColor>()?.transform;
    }


    private void Update()
    {
        if (_player != null)
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        Vector2 direction = (_player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed  * Time.deltaTime);
        transform.position += transform.right * _speed * Time.deltaTime;
    }

}
