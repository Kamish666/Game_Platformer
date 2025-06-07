using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttemptCounterUI : MonoBehaviour
{
    private TextMeshPro _attemptText;

    private void Awake()
    {
        _attemptText = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        AttemptCounter.OnAttemptChanged += UpdateText;
    }

    private void OnDisable()
    {
        AttemptCounter.OnAttemptChanged -= UpdateText;
    }

    private void UpdateText(int count)
    {
        _attemptText.text = $"{count}";
    }
}
