using UnityEngine;
using UnityEngine.UI;


public class CameraMove : MonoBehaviour
{
    [SerializeField] private ManagerScript _ms;
    [SerializeField] private float _speed = 1;

    private float _xAxis;
    private float _yAxis;
    private float _zoom;
    private Camera _cam;

    private Vector3 _position;
    private float _startZoom;

    private float _actualSpeed;


    void Start()
    {
        _cam = GetComponent<Camera>(); 

        _startZoom = _cam.orthographicSize;
        _position = _cam.transform.position;
    }

    void Update()
    {
        //if (ms.saveLoadMenuOpen == false) // if no save or load menus are open.
        {
            _xAxis = Input.GetAxis("Horizontal"); 
            _yAxis = Input.GetAxis("Vertical");

            _zoom = Input.GetAxis("Mouse ScrollWheel") * 10;

            _actualSpeed = _cam.orthographicSize * Time.deltaTime * _speed;
            if (_xAxis != 0 || _yAxis != 0)
            {
                transform.Translate(new Vector3(_xAxis * _actualSpeed, _yAxis * _actualSpeed, 0.0f));
            }

            if (_zoom != 0)
            {
                _cam.orthographicSize -= _zoom * 10 * _actualSpeed; // Уменьшаем размер при прокрутке вниз
                _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, 1, 30); // Ограничиваем размер
                //Debug.Log("Zoom: " + _zoom + "  cam.orthographicSize: " + _cam.orthographicSize);
            }
        }
    }

    public void ReturnToInitialValue()
    {
        _cam.orthographicSize = _startZoom;
        transform.position = _position;
    }
}
