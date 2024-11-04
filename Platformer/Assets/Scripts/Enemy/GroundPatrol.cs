using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrol : Enemy
{
    [Range(0, 0.5f)]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private bool _moveLeft = true;
    [SerializeField] private Transform _groudDetect;
    [SerializeField] private LayerMask _groundLayer;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * _speed);
        RaycastHit2D groundInfo = Physics2D.Raycast(_groudDetect.position, Vector2.down, 1f, _groundLayer);

        if(groundInfo.collider == false)
        {
            if (_moveLeft == true)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                _moveLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                _moveLeft = true;
            }
        }
    }
    
}
