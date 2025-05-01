using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private GameObject _parentObject;
    private GameObject _selectedObject;
    private ObjectInspector _inspector;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
        _inspector = FindObjectOfType<ObjectInspector>();

        if (_parentObject == null) 
            _parentObject = GameObject.Find("Enemy");
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
            GameObject selected = hit.collider.gameObject;

            // Поднимаемся вверх по иерархии, пока не дойдём до _parentObject или корня
            while (selected.transform.parent != null && selected.transform.parent.gameObject != _parentObject)
            {
                selected = selected.transform.parent.gameObject;
            }
            Debug.Log(selected.name);
            _selectedObject = selected;
            _inspector.DisplayObjectParameters(_selectedObject);
        }
    }

    public GameObject GetSelectedObject()
    {
        return _selectedObject;
    }
}

