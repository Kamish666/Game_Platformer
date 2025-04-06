using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : AirPatrol
{
    //public GameObject bullet;
    [SerializeField] private string _projectileTag;
    [SerializeField] private Transform _shoot;
    [GameEditorAnnotation] public float tumeShoot = 4f;

    private BulletPooler _bulletPooler;

    protected override void Start()
    {
        base.Start();

        _bulletPooler = BulletPooler.Instance;

        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(tumeShoot);
        //Instantiate(bullet, _shoot.transform.position, transform.rotation);
        _bulletPooler.SpawnFromPool(_projectileTag, _shoot.transform.position, transform.rotation);
        StartCoroutine(Shooting());
    }
}
