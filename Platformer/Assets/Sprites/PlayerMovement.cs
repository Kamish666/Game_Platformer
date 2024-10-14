using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range (0f, 10f)]
    [SerializeField] private float _spead;
    private Rigidbody2D _rigidbody;
    private Vector3 _scale;
    private bool _grounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _scale = transform.localScale;
    }

    private void Start()
    {
        _grounded = true;
    }

    private void Update()
    {
        float horizpntalInput = Input.GetAxis("Horizontal");
        if (horizpntalInput != 0)
        {
            Run(horizpntalInput);
            FlipSprite(horizpntalInput);
        }

        if (Input.GetKey(KeyCode.Space) && _grounded)
            Jump();

    }

    private void Run(float horizpntalInput) => _rigidbody.velocity = new Vector2(horizpntalInput * _spead, _rigidbody.velocity.y);

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _spead);
        _grounded = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            _grounded = true;
    }

    private void FlipSprite(float horizontalInput)
    {
        if (horizontalInput > 0)
            transform.localScale = _scale;
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1 * _scale.x, _scale.y, 1);
    }
}
