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
