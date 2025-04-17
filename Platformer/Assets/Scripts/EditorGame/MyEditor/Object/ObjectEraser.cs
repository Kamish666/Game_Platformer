using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectEraser : MonoBehaviour
{
    [SerializeField] private GameObject eraserPrefab;
    [SerializeField] private float desiredScreenRadius = 50f; // в пикселях
    private Camera _camera;
    private GameObject _eraserPreview;
    private bool _isErasing = false;
    private float _eraseRadius;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isErasing && _eraserPreview != null)
        {
            UpdatePreview();

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                EraseObjects();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelErasing();
            }
        }
    }

    public void ActivateEraser()
    {
        _isErasing = true;

        if (_eraserPreview != null)
            Destroy(_eraserPreview);

        _eraserPreview = Instantiate(eraserPrefab);
        var sr = _eraserPreview.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            var c = sr.color;
            c.a = 0.5f;
            sr.color = c;
        }
    }

    private void UpdatePreview()
    {
        Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        _eraserPreview.transform.position = mouseWorld;

        // Вычисляем размер в мире для заданного радиуса в пикселях
        Vector3 p1 = _camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, _camera.nearClipPlane));
        Vector3 p2 = _camera.ScreenToWorldPoint(new Vector3(Screen.width / 2 + desiredScreenRadius, Screen.height / 2, _camera.nearClipPlane));

        _eraseRadius = Vector3.Distance(p1, p2);

        // Масштаб круга так, чтобы он совпадал с радиусом
        _eraserPreview.transform.localScale = Vector3.one * _eraseRadius * 2f;
    }

    private void EraseObjects()
    {
        Vector3 worldPos = _eraserPreview.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, _eraseRadius);
        foreach (var hit in hits)
        {
            if (hit.transform.parent != null && hit.transform.parent.name == "Enemy")
            {
                Destroy(hit.gameObject);
            }
        }
    }

    private void CancelErasing()
    {
        _isErasing = false;
        if (_eraserPreview != null)
            Destroy(_eraserPreview);
    }
}
