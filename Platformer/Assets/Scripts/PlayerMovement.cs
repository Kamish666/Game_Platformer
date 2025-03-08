using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool _jumpInput;

    [SerializeField] private float _smoothInputSpeed = 10f; // скорость сглаживания

    public delegate void Act();
    public event Act jumpAct;


    public InputActionReference move;
    public InputActionReference jump;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _scale = transform.localScale;
        _gravityScale = _rigidbody.gravityScale;
        GetComponent<Health>().OnDie += DeactiveScript;
    }


    private void FixedUpdate()
    {
        float targetInput = move.action.ReadValue<float>();

        // Применяем сглаживание
        _horizontalInput = Mathf.Lerp(_horizontalInput, targetInput, Time.deltaTime * _smoothInputSpeed);

        //Debug.Log($"_horizontalInput: {Mathf.Sign(_horizontalInput)}     Mathf.Sign(transform.localScale.x) : {Mathf.Sign(transform.localScale.x)}");


        //_horizontalInput = move.action.ReadValue<float>();
        _jumpInput = jump.action.IsPressed();


        //_jumpInput = Input.GetKey(KeyCode.Space);
        //_horizontalInput = Input.GetAxis("Horizontal"); 


        if (_jumpInput)
            Jump();
        else
            _rigidbody.gravityScale = _gravityScale;

        if (_horizontalInput != 0)
        {
            RunAndFly();
            FlipSprite();
        }


    }

    private void Jump()
    {
        bool isGound = IsGround();

        if (isGound)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpPower);
            //_rigidbody.AddForce(Vector2.up * _jumpPower);
            jumpAct?.Invoke();
            Debug.Log("Я прыгаю от земли");
        }
        else if (OnWall() && !isGound)
        {
            if (_horizontalInput == 0 || Mathf.Sign(_horizontalInput) == Mathf.Sign(transform.localScale.x))
            {
                Debug.Log("Вишу на стене");
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.gravityScale = 0;
            }
            else if (_horizontalInput != 0 && Mathf.Sign(_horizontalInput) != Mathf.Sign(transform.localScale.x))
            {
                Debug.Log("Я прыгаю");
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.gravityScale = _gravityScale;
                transform.localScale = new Vector3(-transform.localScale.x, _scale.y, 1);
                _rigidbody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _jumpPower, _jumpPower);
                //_rigidbody.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * 10, 10));
                jumpAct?.Invoke();
            }
    
        }
        else
            _rigidbody.gravityScale = _gravityScale;
    }

    private void RunAndFly()
    {
        //if (Mathf.Abs(_rigidbody.velocity.x) < 4)
        //    _rigidbody.AddForce( new Vector2(_horizontalInput * _speed, 0));
        _rigidbody.velocity = new Vector2(_horizontalInput * _speed, _rigidbody.velocity.y); 
    }

    private void FlipSprite()
    {
        if (_horizontalInput > 0)
            transform.localScale = _scale;
        else if (_horizontalInput < 0)
            transform.localScale = new Vector3(-1 * _scale.x, _scale.y, 1);
    }

/*    private void FlipSprite()
    {
        if (_horizontalInput > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else if (_horizontalInput < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }*/

    private bool IsGround()
    {
        //RaycastHit2D ray = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, _platformLayer);

        //кастыль для звука прыжка
        RaycastHit2D ray = Physics2D.BoxCast(_collider.bounds.center, new Vector3(_collider.bounds.size.x * 0.7f, _collider.bounds.size.y, _collider.bounds.size.z), 0, Vector2.down, 0.1f, _platformLayer);
        return ray.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D ray = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.01f, _platformLayer);
        return ray.collider != null;
    }

    private void DeactiveScript()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }
}
