using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SawRotator : Saw
{
    [Header("��������� ��������")]
    [GameEditorAnnotation][SerializeField] private float _rotationSpeed = 90f;

    [Header("��������� �����������")]
    [SerializeField] private Transform _stick; // �����
    [SerializeField] private Transform _saw;   // ����

    [Header("��������� �����")]
    [GameEditorAnnotation][SerializeField] private float _stickLength = 2f; // �������� ����� ����� � Unity-��������

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

        if (_isEditor == true)
            StartCoroutine(Editor());
    }

    private void UpdateStickAndSaw()
    {
        if (_stick == null || _saw == null) return;

        SpriteRenderer sr = _stick.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        // �������� ������ ������������� ������� � Unity-��������
        float originalWidth = sr.sprite.bounds.size.x;

        // ��������� �������, ����� ����� ����� = stickLength
        float scaleX = _stickLength / originalWidth;

        float scaleY = _scaleSaw / 5;

        // ��������� �������
        _stick.localScale = new Vector3(scaleX, scaleY, _stick.localScale.z);

        // ������� ���� �� ����� �����
        _saw.localPosition = new Vector3(_stickLength, 0f, 0f);
    }

    IEnumerator Editor()
    {
        while (true)
        {
            ApplyScale();
            UpdateStickAndSaw();
            yield return new WaitForSeconds(0.1f);

        }
    }
}

