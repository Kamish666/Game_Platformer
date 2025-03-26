using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
        }
    }
}