using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private float _maxHealth;

    public float MaxHealth
    {
        get => _maxHealth;

        //private set => _maxHealth = value;
    }
    public float CurrentHealth { get; private set; }
    void Start()
    {
        CurrentHealth = _maxHealth;
    }


    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);

        if (CurrentHealth > 0)
        {
            // anim.SetTrigger("hurt");
        }
        else
        {
            // anim.SetTrigger("die");
            GetComponent<PlayerMovement>().enabled = false;
        }
    }
}
