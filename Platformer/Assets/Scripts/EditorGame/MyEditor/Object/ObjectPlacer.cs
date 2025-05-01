using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{
    //[SerializeField] private GameObject[] placeableObjects;
    private Camera _camera;
    private GameObject _selectedObject;
    private Vector2 _mousePos;
    private GameObject _previewObject;
    [SerializeField] private Transform _enemyParent;

    private void Awake()
    {
        _camera = Camera.main;

        if (_enemyParent == null)
        {
            _enemyParent = GameObject.Find("Enemy").transform;
        }
    }

    private void Update()
    {
        if (_selectedObject != null)
        {
            UpdatePreview();

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceObject();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelSelection();
            }
        }
    }

    public void SelectObject(GameObject objPrefab)
    {
        _selectedObject = objPrefab;
        if (_previewObject != null)
            Destroy(_previewObject);

        _previewObject = Instantiate(_selectedObject);

        foreach (var col in _previewObject.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // Делаем все SpriteRenderer полупрозрачными
        foreach (var sr in _previewObject.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
        }
    }

    private void UpdatePreview()
    {
        if (_previewObject != null)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            _previewObject.transform.position = worldPos;
        }
    }

    private void PlaceObject()
    {
        Vector3 worldPos = _previewObject.transform.position;
        GameObject newObj = Instantiate(_selectedObject, worldPos, Quaternion.identity);

        foreach (var col in newObj.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = true;
        }

        newObj.name = _selectedObject.name;

        newObj.transform.parent = _enemyParent;
    }

    public void CancelSelection()
    {
        _selectedObject = null;
        if (_previewObject != null)
            Destroy(_previewObject);
    }
}
