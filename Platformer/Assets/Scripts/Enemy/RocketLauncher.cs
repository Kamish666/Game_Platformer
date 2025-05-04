using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour, IShot
{
    [SerializeField] private string projectileTag;
    [SerializeField] private Transform _firePoint;
    [GameEditorAnnotation][SerializeField] private float _fireRate = 2f;
    [GameEditorAnnotation][SerializeField] private float _detectionRange = 10f;

    [SerializeField] private LayerMask _obstacleMask;
    private BulletPooler _bulletPooler;

    [SerializeField] private float _offsetAngle = 0f; // угол на который надо повернуть, чтобы было ок

    [GameEditorAnnotation][SerializeField] private bool _useRotationLimits = false;
    [GameEditorAnnotation][SerializeField] private float _minRotation = -45f;
    [GameEditorAnnotation][SerializeField] private float _maxRotation = 45f;

    private Transform _player;

    /*    private Quaternion _angle;*/

    public float FireRate { get => _fireRate; }

    public string ProjectileTag { set => projectileTag = value; }

    private void Start()
    {
        _bulletPooler = BulletPooler.instance;
        _player = FindAnyObjectByType<ChangeColor>()?.transform;
        StartCoroutine(Shooting());
    }


    private void Update()
    {
        if (_player != null)
        {
            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector2 direction = _player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += _offsetAngle;


        if (_useRotationLimits)
        {
            angle = Mathf.Clamp(angle, _minRotation, _maxRotation);
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);

/*        _angle = Quaternion.Euler(0, 0, angle);*/
    }

    public IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireRate);
            if (CanSeePlayer() && _player != null)
            {
                //var rotation = Quaternion.Euler(_firePoint.rotation.x, _firePoint.rotation.y, _firePoint.rotation.z - 90);
                var rotation = Quaternion.Euler(_firePoint.eulerAngles.x, _firePoint.eulerAngles.y, _firePoint.eulerAngles.z - _offsetAngle);

                //rotation.z = -90;
                //rotation.w = 0;
                //rotation.x = 0;
                //rotation.y = 0;

                _bulletPooler.SpawnFromPool(projectileTag, _firePoint.position, rotation);
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (_player == null) return false;

        float distance = Vector2.Distance(_firePoint.position, _player.position);
        if (distance > _detectionRange) return false;

        Vector2 direction = (_player.position - _firePoint.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, direction, distance, _obstacleMask);
        return hit.collider == null;
    }
}
