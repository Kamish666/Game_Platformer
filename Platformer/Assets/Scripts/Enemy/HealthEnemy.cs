using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour, IHealth
{


    [SerializeField] private float _maxHealth;

    public event Damage GetDamage;
    public event Die OnDie;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }

    private bool _isDied = false;

    [SerializeField] Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        Debug.Log("Получен урон. Здоровье: " +  CurrentHealth);
        GetDamage?.Invoke(CurrentHealth);
        if (CurrentHealth > 0)
        {
            // anim.SetTrigger("hurt");
        }
        else if (!_isDied)
        {
            _isDied = true;
            // anim.SetTrigger("die");
            OnDie?.Invoke();
            _anim.SetTrigger("die");
            Destroy(gameObject);
        }
    }
}
