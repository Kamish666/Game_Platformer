using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    [SerializeField] private string _projectileTag;
    [SerializeField] private Transform _firePoint;
    [GameEditorAnnotation][SerializeField] private float _fireRate = 2f;
    [GameEditorAnnotation][SerializeField] private float _projectileSpeed = 1f;
    private BulletPooler _bulletPooler;

    private void Start()
    {
        _bulletPooler = BulletPooler.Instance;
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireRate);
            GameObject projectile = _bulletPooler.SpawnFromPool(_projectileTag, _firePoint.position, transform.rotation);

            if (projectile.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = transform.right * _projectileSpeed;
            }
        }
    }
}
