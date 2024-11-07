using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : Enemy
{
    [Range(0f, 0.5f)]
    [SerializeField] private float _speed = 2f;
    private bool _isWait = false;
    [SerializeField] private bool _isHidden = true;
    [SerializeField] private float _waitTime = 3f;
    private Vector3 _point;
    [SerializeField] private float _distance = 1.3f;
    

    void Start()
    {
        //_point = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _point = transform.position;
    }


    void FixedUpdate()
    {
        if (!_isWait)
            transform.position = Vector3.MoveTowards(transform.position, _point, _speed);
        if (transform.position == _point)
        {
            if (_isHidden)
            {
                _point = transform.position + transform.up * _distance;
                //_point = new Vector3(transform.position.x, transform.position.y + _distance, transform.position.z);
                _isHidden = false;
            }
            else
            {
                _point = transform.position - transform.up * _distance;
                //_point = new Vector3(transform.position.x, transform.position.y - _distance, transform.position.z);
                _isHidden = true;
            }
            _isWait = true;
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(_waitTime);
        _isWait = false;
    }
}
