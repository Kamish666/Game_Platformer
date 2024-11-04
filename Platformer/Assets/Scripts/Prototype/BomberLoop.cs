using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberLoop : AirPatrolLoop
{
    public GameObject bullet;
    [SerializeField] private Transform _shoot;
    public float tumeShoot = 4f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(tumeShoot); 
        Instantiate(bullet, _shoot.transform.position, transform.rotation);
        StartCoroutine(Shooting());
    }
}
