using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum CategoryBlocks
{
    Red,
    Blue,
    Green,
    Black
}



[CreateAssetMenu(fileName = "BuildBlock", menuName = "BuildingObjects/Create BuildBlock")]
public class BuildingBlockBase : ScriptableObject
{
    [SerializeField] private CategoryBlocks _category;
    [SerializeField] private TileBase _tileBase;

    private Tilemap _tileMap;
    private TilemapRenderer _tileRenderer;

    public TilemapRenderer TileRenderer
    {
        get
        {
            return _tileRenderer;
        }
        set
        {
            _tileRenderer = value;
        }
    }
    public Tilemap TileMap
    {
        get
        {
            return _tileMap;
        }
        set
        {
            _tileMap = value;
        }
    }

    public TileBase TileBase
    {
        get
        {
            return _tileBase;
        }
    }

    public CategoryBlocks Category
    {
        get
        {
            return _category;
        }
    }

}
