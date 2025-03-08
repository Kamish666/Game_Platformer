using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] private string _projectileTag;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private BulletPooler _bulletPooler;

    private Transform _player;

    private void Start()
    {
        _bulletPooler = BulletPooler.Instance;
        _player = FindAnyObjectByType<ChangeColor>()?.transform;
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireRate);
            if (CanSeePlayer() && _player != null)
            {
                _bulletPooler.SpawnFromPool(_projectileTag, _firePoint.position, Quaternion.identity);
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (_player == null) return false;
        Vector2 direction = (_player.position - _firePoint.position).normalized;
        float distance = Vector2.Distance(_firePoint.position, _player.position);
        RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, direction, distance, _obstacleMask);
        return hit.collider == null;
    }
}
