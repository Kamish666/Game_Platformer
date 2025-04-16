using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


enum BlockToolType
{
    None,
    EraserAll,
    EraserOne
}


[CreateAssetMenu(fileName = "BuildBlock", menuName = "BuildingObjects/Create BlockTool")]
public class BuildingBlockTool : BuildingBlockBase
{
    [SerializeField] private BlockToolType _toolType;

    public void Use(Vector3Int position)
    {
        BlockToolController tc = BlockToolController.GetInstance();

        switch (_toolType)
        {
            case BlockToolType.EraserAll: tc.Eraser(position); break;
            case BlockToolType.EraserOne: tc.Eraser(position, TileMap); break;
        }
    }

    
}
