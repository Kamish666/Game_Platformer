using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SawRotator : Saw
{
    [Header("Настройки вращения")]
    [GameEditorAnnotation][SerializeField] private float _rotationSpeed = 90f;

    [Header("Настройки компонентов")]
    [SerializeField] private Transform _stick; // Палка
    [SerializeField] private Transform _saw;   // Пила

    [Header("Настройки длины")]
    [GameEditorAnnotation][SerializeField] private float _stickLength = 2f; // Желаемая длина палки в Unity-единицах

    private void Update()
    {
        transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateStickAndSaw();
    }

    protected override void Start()
    {
        base.Start();
        UpdateStickAndSaw();
    }

    private void UpdateStickAndSaw()
    {
        if (_stick == null || _saw == null) return;

        SpriteRenderer sr = _stick.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        // Получаем ширину оригинального спрайта в Unity-единицах
        float originalWidth = sr.sprite.bounds.size.x;

        // Вычисляем масштаб, чтобы длина палки = stickLength
        float scaleX = _stickLength / originalWidth;

        // Применяем масштаб
        _stick.localScale = new Vector3(scaleX, _stick.localScale.y, _stick.localScale.z);

        // Смещаем пилу на конец палки
        _saw.localPosition = new Vector3(_stickLength, 0f, 0f);
    }
}

