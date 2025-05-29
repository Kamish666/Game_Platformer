using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Beetle : MonoBehaviour
{
    [Range(0f, 5f)]
    [GameEditorAnnotation][SerializeField] private float _speed = 2f;
    private bool _isWait = true;
    [GameEditorAnnotation][SerializeField] private bool _isHidden = true;
    [GameEditorAnnotation][SerializeField] private float _waitTime = 3f;
    private Vector3 _point;
    [GameEditorAnnotation][SerializeField] private float _distance = 1.3f;
    

    void Start()
    {
        //_point = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _point = transform.position;
    }


    void Update()
    {
        if (!_isWait)
            transform.position = Vector3.MoveTowards(transform.position, _point, _speed * Time.deltaTime);
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
