using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range (0f, 10f)]
    [SerializeField] private float _speed;
    [Range(0, 50)]
    [SerializeField] private float _jumpPower;
    [SerializeField] private LayerMask _platformLayer;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider; 
    private Vector3 _scale;
    private float _gravityScale;
    private float _horizontalInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _scale = transform.localScale;
        _gravityScale = _rigidbody.gravityScale;
    }


    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        if (_horizontalInput != 0)
        {
            Run();
            FlipSprite();
        }

        if (Input.GetKey(KeyCode.Space))
            Jump();
        else
            _rigidbody.gravityScale = _gravityScale;
    }

    private void Jump()
    {
        if (isGround())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpPower);
        }
        else if (onWall() && !isGround())
        {
            if (_horizontalInput == 0 || _horizontalInput == Mathf.Sign(transform.localScale.x))
            {
                print("¬ишу на стене");
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.gravityScale = 0;
            }
            else if (_horizontalInput != Mathf.Sign(transform.localScale.x))
            {
                print("я прыгаю");
                _rigidbody.gravityScale = _gravityScale;
                _rigidbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 10);
                transform.localScale = new Vector3(-transform.localScale.x, _scale.y, 1);
            }

        }
        else
            _rigidbody.gravityScale = _gravityScale;
    }

    private void Run()
    {
        _rigidbody.AddForce(new Vector2(_horizontalInput, _rigidbody.velocity.y).normalized * _speed, ForceMode2D.Impulse); 
    }

    private void FlipSprite()
    {
        if (_horizontalInput > 0)
            transform.localScale = _scale;
        else if (_horizontalInput < 0)
            transform.localScale = new Vector3(-1 * _scale.x, _scale.y, 1);
    }

    private bool isGround()
    {
        RaycastHit2D ray = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, _platformLayer);
        return ray.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D ray = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, _platformLayer);
        return ray.collider != null;
    }
}
