using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Transform _gunPivot;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private string _projectileTag;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private Transform _playerTransform;

    private PoolerBulletsAndPS _bulletPooler;

    private void Start()
    {
        _bulletPooler = PoolerBulletsAndPS.instance;
    }

    private void Update()
    {
        RotateGun();
        if (Input.GetKeyDown(KeyCode.W))
        {
            Shoot();
        }
    }

    private void RotateGun()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - _gunPivot.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (_playerTransform.localScale.x < 0)
        {
            angle += 180f;
        }

        _gunPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Shoot()
    {
        GameObject projectile = _bulletPooler.SpawnFromPool(_projectileTag, _firePoint.position, _gunPivot.rotation);
        if (projectile.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = _gunPivot.right * _projectileSpeed * Mathf.Sign(_playerTransform.localScale.x);
        }
    }
}

