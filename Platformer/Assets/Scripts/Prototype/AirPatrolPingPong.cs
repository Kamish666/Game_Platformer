using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPatrolPingPong : AirPatrolLoop
{
    private bool _moveForward;

    protected override void Start()
    {
        base.Start();
        _moveForward = true;
    }

    protected override void ChangeTarget()
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
}
