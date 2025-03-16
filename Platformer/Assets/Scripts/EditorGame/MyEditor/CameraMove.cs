using UnityEngine;
using UnityEngine.UI;


public class CameraMove : MonoBehaviour
{
    [SerializeField] private Slider _cameraSpeedSlide;
    [SerializeField] private ManagerScript _ms;

    private float _xAxis;
    private float _yAxis;
    private float _zoom;
    private Camera _cam;

    private Vector3 _position;
    private float _startZoom;


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

            if (_xAxis != 0 || _yAxis != 0) 
                transform.Translate(new Vector3(_xAxis * _cameraSpeedSlide.value, _yAxis * _cameraSpeedSlide.value, 0.0f));


            if (_zoom != 0)
            {
                _cam.orthographicSize -= _zoom * _cameraSpeedSlide.value; // Уменьшаем размер при прокрутке вниз
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
