using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BlockCreator : Singleton<BlockCreator>
{
    [SerializeField] private Tilemap _priviewMap, _defaultMap;
    private TileBase _tileBase;

    [SerializeField] private GameEditor _gameEditor;

    private BuildingBlockBase _selectObj;

    private Vector2 _mousePos;

    private Vector3Int _currentGridPosition;
    private Vector3Int _lastGridPosition;

    private Camera _camera;

    private BuildingBlockBase SelectedObj
    {
        set
        {
            _selectObj = value;

            _tileBase = _selectObj != null ? _selectObj.TileBase : null;
            UpdatePreview();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _gameEditor = new GameEditor();
        _camera = Camera.main;
    }



    private void Update()
    {
        if (_selectObj != null)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(_mousePos);
            Vector3Int gridPos = _priviewMap.WorldToCell(pos);

            if (gridPos != _currentGridPosition)
            {
                _lastGridPosition = _currentGridPosition;
                _currentGridPosition = gridPos;

                UpdatePreview();
            }
        }
    }

    private void UpdatePreview()
    {
        _priviewMap.SetTile(_lastGridPosition, null);
        _priviewMap.SetTile(_currentGridPosition, _tileBase);
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        _mousePos = context.ReadValue<Vector2>();
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        if (_selectObj != null && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleDrawing();
        }
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        SelectedObj = null;
    }

    public void ObjectSelected(BuildingBlockBase obj)
    {
        SelectedObj = obj;
    }

    private void HandleDrawing()
    {
        DrawItem();
    }

    private void DrawItem()
    {
        _defaultMap.SetTile(_currentGridPosition, _tileBase);
    }

    private void OnEnable()
    {
        _gameEditor.Enable();

        _gameEditor.Editor.MousePosition.performed += OnMouseMove;
        _gameEditor.Editor.MouseLeftClick.performed += OnLeftClick;
        _gameEditor.Editor.MouseRightClick.performed += OnRightClick;
    }

    private void OnDisable()
    {
        _gameEditor.Disable();

        _gameEditor.Editor.MousePosition.performed -= OnMouseMove;
        _gameEditor.Editor.MouseLeftClick.performed -= OnLeftClick;
        _gameEditor.Editor.MouseRightClick.performed -= OnRightClick;
    }
}
