using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public delegate void Damage(float damage);
public delegate void Die();

public interface IHealth
{

    public event Damage GetDamage;
    public event Die OnDie;

    public float MaxHealth { get; }
    public float CurrentHealth { get; }
}

public class Health : MonoBehaviour, IHealth
{

    [SerializeField] private float _maxHealth;

    public event Damage GetDamage;
    public event Die OnDie;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }


    void Start()
    {
        CurrentHealth = MaxHealth;    }


    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        GetDamage?.Invoke(CurrentHealth);
        if (CurrentHealth > 0)
        {
            // anim.SetTrigger("hurt");
        }
        else
        {
            // anim.SetTrigger("die");
            OnDie?.Invoke();
        }
    }
}
