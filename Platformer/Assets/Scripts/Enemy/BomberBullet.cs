using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BomberBullet : Bullet
{
    [SerializeField] private float _speed = 5f;

    private void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
    }
}
