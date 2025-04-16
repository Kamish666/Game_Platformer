using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInitializer : Singleton<TilemapInitializer>
{
    [SerializeField] private List<BlocksOfTheSameCategory> _blocksOfTheSameCategory;
    [SerializeField] private Transform _grid;

    private void Start()
    {
        CreateMaps();
    }

    private void CreateMaps()
    {
        foreach (BlocksOfTheSameCategory category in _blocksOfTheSameCategory)
        {

            // Create new GameObject
            GameObject obj = new GameObject(category.buildingBlockBase.name);

            // Assign Tilemap Features
            Tilemap map = obj.AddComponent<Tilemap>();
            TilemapRenderer render = obj.AddComponent<TilemapRenderer>();
            category.buildingBlockBase.TileRenderer = render;

            // Set Parent
            obj.transform.SetParent(_grid);


            category.buildingBlockBase.TileMap = map;
            foreach (BuildingBlockTool tool in category.buildingBlockTools)
            {
                tool.TileMap = map;
                tool.TileRenderer = render;
            }
        }
    }
}

[Serializable]
public class BlocksOfTheSameCategory
{
    public BuildingBlockBase buildingBlockBase;
    public List<BuildingBlockTool> buildingBlockTools;
}