using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    [SerializeField] float distance; //можно заменить на габариты collider 

    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float forceWall; //сила толчка от стены 

    float _horizontalInput;
    Rigidbody2D _rb;

    bool Flip = false; //false == left, true == right 

    float gravityScale;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        gravityScale = _rb.gravityScale;
    }

    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxis("Horizontal");


        if (IsGround())
        {
            Run();
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

        }
        else if (IsWall())
        {
            if (Input.GetKey(KeyCode.Space) && _horizontalInput > 0.1f) // _horizontalInput == 1 прыжое только вправо 
            {
                Jump();
                if (Flip)
                {
                    _rb.AddForce(Vector2.left * forceWall);
                }
                else
                {
                    _rb.AddForce(Vector2.right * forceWall);
                }
                _rb.gravityScale = gravityScale;

            }
            else if (Input.GetKey(KeyCode.Space))
            {
                if (_rb.gravityScale != 0)
                {
                    _rb.velocity = Vector2.zero;
                    _rb.gravityScale = 0;
                }
            }
        }
        if (!IsGround() && !IsWall())
        {
            Run();
        }

    }
    bool IsGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance, mask);
        return hit.collider != null;
    }
    void Jump()
    {
        _rb.velocity = Vector2.up * jumpForce;
    }
    private void Run()
    {
        _rb.velocity = new Vector2(_horizontalInput * speed, _rb.velocity.y);
    }
    private bool IsWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, distance, mask);// 
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, distance, mask);// 
        if (hit.collider == null &&  hit2.collider == null) { return false; }

        //костыль 
        if (hit.collider != null)
            Flip = false;
        else
            Flip = true;
        return true;
    }

}