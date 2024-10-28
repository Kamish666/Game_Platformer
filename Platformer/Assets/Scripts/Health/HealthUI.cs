using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health _objectHealt;
    [SerializeField] private Image _totalHealthBar;
    [SerializeField] private Image _currentHealthBar;
    [SerializeField] private TextMeshProUGUI _textHealth;
    private float _maxHealth;

    private void Start()
    {
        _objectHealt.GetDamage += UpdateHealthUI;

        _maxHealth = _objectHealt.MaxHealth;

/*        float amountHealth = _maxHealth / 10;

        _totalHealthBar.fillAmount = amountHealth;
        _currentHealthBar.fillAmount = amountHealth;*/
        _totalHealthBar.fillAmount = 1;
        _currentHealthBar.fillAmount = 1;


        _textHealth.text = $"HP: {_maxHealth}/{_maxHealth}";
    }


    public void UpdateHealthUI(float health)
    {
/*        _currentHealthBar.fillAmount = health / 10;*/
        _currentHealthBar.fillAmount = health / _maxHealth;
        _textHealth.text = $"HP: {health}/{_maxHealth}";
    }
}
