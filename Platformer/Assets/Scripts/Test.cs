using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody2D rigidbody2;

    void Start()
    {
       rigidbody2 = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Abs(rigidbody2.velocity.x) < 10)
        {
            rigidbody2.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0).normalized * 10);
        }
    }
}
