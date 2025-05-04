using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour, IShot
{
    [SerializeField] private string projectileTag;
    [SerializeField] private Transform _firePoint;
    [GameEditorAnnotation][SerializeField] private float _fireRate = 2f;
    [GameEditorAnnotation][SerializeField] private float _projectileSpeed = 1f;
    private BulletPooler _bulletPooler;

    public float FireRate { get => _fireRate; }

    public string ProjectileTag { set => projectileTag = value; }

    private void Start()
    {
        _bulletPooler = BulletPooler.instance;
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
