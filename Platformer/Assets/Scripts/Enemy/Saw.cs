using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : Enemy
{
    [Header("Настройки масштабирования")]
    [Range(0f, 10f)]
    [GameEditorAnnotation][SerializeField] private float _scaleSaw = 1f;

    [Header("Целевой объект")]
    [SerializeField] private Transform _target;

    protected virtual void Start()
    {
        if (_target == null)
            _target = transform;

        ApplyScale();
    }

    protected virtual void OnValidate()
    {
        if (_target == null)
            _target = transform;

        ApplyScale();
    }

    private void ApplyScale()
    {
        _target.localScale = new Vector3(_scaleSaw, _scaleSaw, _target.localScale.z);
    }
}
