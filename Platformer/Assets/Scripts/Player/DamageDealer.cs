using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float _damage;


    public void GetDamage(GameObject enemy)
    {
        enemy.GetComponent<HealthEnemy>().TakeDamage(_damage);
    }
}
