using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HidingEnemy : MonoBehaviour
{
    [Range(0f, 5f)]
    [GameEditorAnnotation("Speed")]
    [SerializeField] private float _speed = 2f;

    [SerializeField] private Transform _beetleBody; // ссылка на дочерний объект (сам жук)

    private bool _isWait = true;

    [SerializeField] private bool _isHidden = true;
    [GameEditorAnnotation("Wait time")]
    [SerializeField] private float _waitTime = 3f;

    [GameEditorAnnotation("Distance")]
    [SerializeField] private float _distance = 1.3f;

    [GameEditorAnnotation("Scale X")]
    [SerializeField] private float _scaleX = 1f;
    [GameEditorAnnotation("Scale Y")]
    [SerializeField] private float _scaleY = 1f;

    private float _targetY;

    private void Start()
    {
        _targetY = 0;

        ApplyScale();

        ChangeColor changeColor = ChangeColor.instance;
        if (changeColor == null)
            StartCoroutine(Editor());
    }


    private void OnValidate()
    {
        ApplyScale();
    }

    private void ApplyScale()
    {
        _beetleBody.localScale = new Vector3(_scaleX, _scaleY, _beetleBody.localScale.z);
    }

    IEnumerator Editor()
    {
        while (true)
        {
            ApplyScale();
            yield return new WaitForSeconds(0.1f);

        }
    }


    void Update()
    {
        if (!_isWait)
        {
            Vector3 currentPos = _beetleBody.localPosition;
            float newY = Mathf.MoveTowards(currentPos.y, _targetY, _speed * Time.deltaTime);
            _beetleBody.localPosition = new Vector3(currentPos.x, newY, currentPos.z);
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
                _targetY = 0;
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
