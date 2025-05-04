using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingEnemy : Enemy
{
    [Range(0f, 5f)]
    [GameEditorAnnotation][SerializeField] private float _speed = 2f;

    [SerializeField] private Transform _beetleBody; // ссылка на дочерний объект (сам жук)

    private bool _isWait = true;

    [GameEditorAnnotation][SerializeField] private bool _isHidden = true;
    [GameEditorAnnotation][SerializeField] private float _waitTime = 3f;

    [GameEditorAnnotation][SerializeField] private float _distance = 1.3f;

    private float _targetY;

    void Start()
    {
        _targetY = _beetleBody.localPosition.y;
    }

    void Update()
    {
        if (!_isWait)
        {
            Vector3 currentPos = _beetleBody.localPosition;
            float newY = Mathf.MoveTowards(currentPos.y, _targetY, _speed * Time.deltaTime);
            _beetleBody.position = new Vector3(currentPos.x, newY, currentPos.z);
        }

        if (Mathf.Approximately(_beetleBody.localPosition.y, _targetY))
        {
            if (_isHidden)
            {
                _targetY = _beetleBody.localPosition.y + _distance;
                _isHidden = false;
            }
            else
            {
                _targetY = _beetleBody.localPosition.y - _distance;
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
