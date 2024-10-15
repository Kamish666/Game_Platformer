using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range (0f, 10f)]
    [SerializeField] private float _spead;
    [SerializeField] private LayerMask _platformLayer;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider; 
    private Vector3 _scale;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _scale = transform.localScale;
    }


    private void Update()
    {
        float horizpntalInput = Input.GetAxis("Horizontal");
        if (horizpntalInput != 0)
        {
            Run(horizpntalInput);
            FlipSprite(horizpntalInput);
        }

        if (Input.GetKey(KeyCode.Space) && isPlatform())
            Jump();

    }

    private void Run(float horizpntalInput) => _rigidbody.velocity = new Vector2(horizpntalInput * _spead, _rigidbody.velocity.y);

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _spead);

    }

    private void FlipSprite(float horizontalInput)
    {
        if (horizontalInput > 0)
            transform.localScale = _scale;
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1 * _scale.x, _scale.y, 1);
    }

    private bool isPlatform()
    {
        RaycastHit2D ray = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, _platformLayer);
        if (ray.collider != null)
            return true;
        ray = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, _platformLayer);
        return ray.collider != null;
    }
}
