using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour, IShot
{
    [SerializeField] private string projectileTag;
    [SerializeField] private Transform _firePoint;
    [GameEditorAnnotation("Fire interval")]
    [SerializeField] private float _fireRate = 2f;
    [GameEditorAnnotation("Projectile speed")]
    [SerializeField] private float _projectileSpeed = 1f;
    private PoolerBulletsAndParticalSystems _bulletPooler;

    public float FireRate { get => _fireRate; }

    public string ProjectileTag { set => projectileTag = value; }

    private void Start()
    {
        _bulletPooler = PoolerBulletsAndParticalSystems.instance;
        StartCoroutine(Shooting());
    }

    public IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireRate);
            GameObject projectile = _bulletPooler.SpawnFromPool(projectileTag, _firePoint.position, transform.rotation);

            if (projectile.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = transform.right * _projectileSpeed;
            }
        }
    }
}
