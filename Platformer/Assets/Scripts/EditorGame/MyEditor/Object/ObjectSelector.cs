using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask selectableLayer;
    private GameObject _selectedObject;
    private ObjectInspector _inspector;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
        _inspector = FindObjectOfType<ObjectInspector>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            SelectObject();
        }
    }

    private void SelectObject()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, selectableLayer);

        if (hit.collider != null)
        {
            _selectedObject = hit.collider.gameObject;
            _inspector.DisplayObjectParameters(_selectedObject);
        }
    }

    public GameObject GetSelectedObject()
    {
        return _selectedObject;
    }
}

