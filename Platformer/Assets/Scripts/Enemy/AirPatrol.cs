using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPatrol : Enemy
{
    [GameEditorAnnotation] [SerializeField] protected Transform[] _points;
    protected Transform _target;
    [Range(0, 0.5f)]
    [GameEditorAnnotation] [SerializeField] private float _speed = 0.1f;
    protected int _index;
    private bool _moveForward;

    [GameEditorAnnotation] public bool loop;

    protected virtual void Start()
    {
        gameObject.transform.position = new Vector3(_points[0].position.x, _points[0].position.y, transform.position.z);
        _target = _points[1];
        _index = 1;
        _moveForward = true;
    }

    protected void FixedUpdate()
    {
        MovePosition();

        if (transform.position == _points[_index].position)
        {
            if (loop)
                ChangeTargetLoop();
            else
                ChangeTargetPingPong();
        }
    }

    protected void MovePosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed);
    }

    protected void ChangeTargetLoop()
    {
        if (transform.position == _points[_index].position)
        {
            float location = _points[_index].position.x;
            _index++;
            if (_index >= _points.Length)
                _index = 0;
            _target = _points[_index];
            FlipSprite(location, _target.position.x);
        }
    }

    protected void ChangeTargetPingPong()
    {
        if (transform.position == _points[_index].position)
        {
            float location = _points[_index].position.x;
            if (_moveForward)
            {
                _index++;
                if (_index >= _points.Length)
                {
                    _moveForward = false;
                    _index -= 2;
                }
            }
            else
            {
                _index--;
                if (_index < 0)
                {
                    _moveForward = true;
                    _index += 2;
                }
            }
            _target = _points[_index];
            FlipSprite(location, _target.position.x);
        }
    }

    protected void FlipSprite(float location, float target)
    {
        if (location - target > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}   