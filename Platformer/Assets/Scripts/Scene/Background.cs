using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float _lengthX, _lengthY, _startPosX, _startPosY;
    [SerializeField] private GameObject _background;
    [Range(0, 1)]
    [SerializeField] private float _paralaxEffect;

    private void Start()
    {
        _startPosX = _background.transform.position.x;
        _startPosY = _background.transform.position.y;
        _lengthX = _background.GetComponent<SpriteRenderer>().bounds.size.x;
        _lengthY = _background.GetComponent<SpriteRenderer>().bounds.size.y; // Добавлено для Y
    }

    private void FixedUpdate()
    {
        float tempX = transform.position.x * (1 - _paralaxEffect);
        float tempY = transform.position.y * (1 - _paralaxEffect); // Добавлено для Y
        float distX = transform.position.x * _paralaxEffect;
        float distY = transform.position.y * _paralaxEffect; // Добавлено для Y

        _background.transform.position = new Vector3(_startPosX + distX, _startPosY + distY, _background.transform.position.z); // Обновлено для Y

        // Проверка границ для X
        if (tempX > _startPosX + _lengthX)
            _startPosX += _lengthX;
        else if (tempX < _startPosX - _lengthX)
            _startPosX -= _lengthX;

        // Проверка границ для Y
        if (tempY > _startPosY + _lengthY)
            _startPosY += _lengthY;
        else if (tempY < _startPosY - _lengthY)
            _startPosY -= _lengthY;
    }
}
