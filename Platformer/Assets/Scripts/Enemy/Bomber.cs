using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShot
{
    public IEnumerator Shooting();

    public float FireRate { get; }

    public string ProjectileTag { set; }
}

public class Bomber : AirPatrol, IShot
{
    //public GameObject bullet;
    [SerializeField] private string projectileTag;
    [SerializeField] private Transform _shoot;
    [GameEditorAnnotation("Fire interval")]
    [SerializeField] private float _fireRate = 4f;

    private PoolerBulletsAndPS _bulletPooler;

    public float FireRate { get => _fireRate; }

    public string ProjectileTag { set => projectileTag = value; }

    protected override void Start()
    {
        base.Start();

        _bulletPooler = PoolerBulletsAndPS.instance;

        StartCoroutine(Shooting());
    }

    public IEnumerator Shooting()
    {
        yield return new WaitForSeconds(_fireRate);
        //Instantiate(bullet, _shoot.transform.position, transform.rotation);
        _bulletPooler.SpawnFromPool(projectileTag, _shoot.transform.position, transform.rotation);
        StartCoroutine(Shooting());
    }
}
