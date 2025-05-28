using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPatrol : Enemy
{
    [GameEditorAnnotation("Points")]
    [SerializeField] protected Vector2[] _points;
    protected Vector2 _target;
    [Range(0, 5f)]
    [GameEditorAnnotation("Speed")]
    [SerializeField] private float _speed = 1f;
    protected int _index;
    private bool _moveForward;

    [GameEditorAnnotation("Loop")] 
    public bool loop;

#if UNITY_EDITOR
    [SerializeField] private Transform[] _editorPoints;
#endif

    public Vector2[] GetPoints() => _points;
    public void SetPoints(Vector2[] points) => _points = points;
    public float Speed { get => _speed; set => _speed = value; }


    protected virtual void Start()
    {
        if (_points.Length != 0)
        {
            gameObject.transform.position = new Vector3(_points[0].x, _points[0].y, transform.position.z);
            _target = _points[1];
            _index = 1;
            _moveForward = true;
        }
    }

    protected void Update()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            //Debug.Log(i + " : " + _points[i].x + "   " + _points[i].y);
        }

        if (_points.Length > 1)
        {
            MovePosition();

            Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(currentPos2D, _target) < 0.01f)
            {
                if (loop)
                    ChangeTargetLoop();
                else
                    ChangeTargetPingPong();
            }
        }
    }

    protected void MovePosition()
    {
        Vector3 targetPos = new Vector3(_target.x, _target.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);
    }

    protected void ChangeTargetLoop()
    {
        float location = transform.position.x;
        _index++;
        if (_index >= _points.Length)
            _index = 0;
        _target = _points[_index];
        FlipSprite(location, _target.x);
    }

    protected void ChangeTargetPingPong()
    {
        for (; _index > _points.Length; _index--)
            if (_index < 0)
                return;

        float location = transform.position.x;
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
        FlipSprite(location, _target.x);
    }

    protected void FlipSprite(float location, float target)
    {
        if (location - target > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_editorPoints != null && _editorPoints.Length > 0)
        {
            _points = new Vector2[_editorPoints.Length];
            for (int i = 0; i < _editorPoints.Length; i++)
            {
                if (_editorPoints[i] != null)
                    _points[i] = _editorPoints[i].position;
            }
        }
    }
#endif


}
