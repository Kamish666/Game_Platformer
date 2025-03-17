using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Tilemaps;

public class BlockCreator : Singleton<BlockCreator>
{
    [SerializeField] private Tilemap _previewMap, _defaultMap;

    TilemapRenderer _defaultRenderer;

    [SerializeField] private GameEditor _gameEditor;

    private TileBase _tileBase;
    private BuildingBlockBase _selectObj;

    private Vector2 _mousePos;
    private Vector3Int _currentGridPosition;
    private Vector3Int _lastGridPosition;

    private bool _holdAction;
    private Vector3Int _holdStartPosition;
    private BoundsInt _bounds;

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
            Vector3Int gridPos = _previewMap.WorldToCell(pos);

            if (gridPos != _currentGridPosition)
            {
                _lastGridPosition = _currentGridPosition;
                _currentGridPosition = gridPos;

                UpdatePreview();

                if (_holdAction)
                {
                    HandleDrawing();
                }
            }
        }
    }

    private void UpdatePreview()
    {
        _previewMap.SetTile(_lastGridPosition, null);
        _previewMap.SetTile(_currentGridPosition, _tileBase);
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        _mousePos = context.ReadValue<Vector2>();
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        if (_selectObj != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (context.phase == InputActionPhase.Started)
            {
                _holdAction = true;

                if (context.interaction is TapInteraction)
                {
                    _holdStartPosition = _currentGridPosition;
                }
                HandleDrawing();
            }
            else
            {
                if (context.interaction is SlowTapInteraction ||
                    context.interaction is TapInteraction && context.phase == InputActionPhase.Performed)
                {
                    _holdAction = false;
                    HandleDrawRelease();
                }
            }
        }
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        SelectedObj = null;
    }

    public void ObjectSelected(IBuildingBase objBase)
    {
        BuildingBlockBase obj = objBase as BuildingBlockBase;

        SelectedObj = obj;

        if (_defaultRenderer != null)
            _defaultRenderer.sortingOrder = 0;
        _defaultMap = obj.TileMap;
        _defaultRenderer = obj.TileRenderer;
        _defaultRenderer.sortingOrder = 1;
    }

    private void HandleDrawing()
    {
        if (_selectObj != null)
        {
            RectangleRenderer();
        }
        //DrawItem();
    }

    private void HandleDrawRelease()
    {
        if (_selectObj != null)
        {
            DrawBounds(_defaultMap);
            _previewMap.ClearAllTiles();
        } 
    }

    private void RectangleRenderer()
    {
        _previewMap.ClearAllTiles();

        _bounds.xMin = _currentGridPosition.x < _holdStartPosition.x ? _currentGridPosition.x : _holdStartPosition.x;
        _bounds.xMax = _currentGridPosition.x > _holdStartPosition.x ? _currentGridPosition.x : _holdStartPosition.x;
        _bounds.yMin = _currentGridPosition.y < _holdStartPosition.y ? _currentGridPosition.y : _holdStartPosition.y;
        _bounds.yMax = _currentGridPosition.y > _holdStartPosition.y ? _currentGridPosition.y : _holdStartPosition.y;

        DrawBounds(_previewMap);
    }

    private void DrawBounds(Tilemap tilemap)
    {
        for (int x = _bounds.xMin; x <= _bounds.xMax; x++)
        {
            for (int y = _bounds.yMin; y <= _bounds.yMax; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), _tileBase);
            }
        }
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
        _gameEditor.Editor.MouseLeftClick.started += OnLeftClick;
        _gameEditor.Editor.MouseLeftClick.canceled += OnLeftClick;

        _gameEditor.Editor.MouseRightClick.performed += OnRightClick;
    }

    private void OnDisable()
    {
        _gameEditor.Disable();

        _gameEditor.Editor.MousePosition.performed -= OnMouseMove;

        _gameEditor.Editor.MouseLeftClick.performed -= OnLeftClick;
        _gameEditor.Editor.MouseLeftClick.started -= OnLeftClick;
        _gameEditor.Editor.MouseLeftClick.canceled -= OnLeftClick;

        _gameEditor.Editor.MouseRightClick.performed -= OnRightClick;
    }
}
