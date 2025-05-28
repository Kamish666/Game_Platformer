using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrol : Enemy
{
    [Range(0, 5f)]
    [GameEditorAnnotation("Speed")]
    [SerializeField] private float _speed = 2f;

    [SerializeField] private bool _moveLeft = true;
    [SerializeField] private Transform _detectWallAndGround;
    [SerializeField] private LayerMask _layerMack;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * _speed * Time.fixedDeltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(_detectWallAndGround.position, Vector2.down, 1f, _layerMack);

        Vector2 direction = _moveLeft ? Vector2.left : Vector2.right;
        RaycastHit2D wallInfo = Physics2D.Raycast(_detectWallAndGround.position, direction, 0.01f, _layerMack);

        if (groundInfo.collider == false || wallInfo.collider == true)
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
