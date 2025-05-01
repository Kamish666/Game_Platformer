using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInitializer : Singleton<TilemapInitializer>
{
    [SerializeField] private List<BlocksOfTheSameCategory> _blocksOfTheSameCategory;
    [SerializeField] private Transform _grid;
    [SerializeField] private LayerMask _layerMask;

    private void Start()
    {
        CreateMaps();
    }

    private void CreateMaps()
    {
        foreach (BlocksOfTheSameCategory category in _blocksOfTheSameCategory)
        {

            string name = category.buildingBlockBase.name;
            GameObject obj = GameObject.Find(name);

            //Debug.Log("Èìÿ tilemapa: " + obj.name);
            if (obj != null)
            {
                category.buildingBlockBase.TileMap = obj.GetComponent<Tilemap>();
                category.buildingBlockBase.TileRenderer = obj.GetComponent<TilemapRenderer>();
                continue;
            }

            // Create new GameObject
            obj = new GameObject(name);

            // Assign Tilemap Features
            Tilemap map = obj.AddComponent<Tilemap>();
            TilemapRenderer render = obj.AddComponent<TilemapRenderer>();

            // Set Parent
            obj.transform.SetParent(_grid);

            obj.layer = _layerMask;

            category.buildingBlockBase.TileMap = map;
            category.buildingBlockBase.TileRenderer = render;

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