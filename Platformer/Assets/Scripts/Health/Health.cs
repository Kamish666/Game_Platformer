using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    [SerializeField] private float _maxHealth;
    public delegate void Damage(float damage);
    public Damage GetDamage;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }

    void Start()
    {
        CurrentHealth = _maxHealth;
    }


    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
        GetDamage?.Invoke(CurrentHealth);
        if (CurrentHealth > 0)
        {
            // anim.SetTrigger("hurt");
        }
        else
        {
            // anim.SetTrigger("die");
            GetComponent<PlayerMovement>().enabled = false;
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(5f); // ∆дем 5 секунд
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ѕерезапускаем текущую сцену
    }
}
