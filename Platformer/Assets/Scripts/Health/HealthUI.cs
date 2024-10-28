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
        _maxHealth = _objectHealt.MaxHealth;
        _totalHealthBar.fillAmount = _maxHealth / 10;
        _textHealth.text = $"HP: {_maxHealth}/{_maxHealth}";
    }

    private void Update()
    {
        _currentHealthBar.fillAmount = _objectHealt.CurrentHealth / 10;
        _textHealth.text = $"HP: {_objectHealt.CurrentHealth}/{_maxHealth}";

    }

}
