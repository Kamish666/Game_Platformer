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
    [SerializeField] private int _layerIndex;

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

            Tilemap map;
            TilemapRenderer render;

            if (obj != null)
            {
                // Существующий объект
                map = obj.GetComponent<Tilemap>();
                render = obj.GetComponent<TilemapRenderer>();

                if (map == null) map = obj.AddComponent<Tilemap>();
                if (render == null) render = obj.AddComponent<TilemapRenderer>();
            }
            else
            {
                // Новый объект
                obj = new GameObject(name);
                map = obj.AddComponent<Tilemap>();
                render = obj.AddComponent<TilemapRenderer>();

                obj.transform.SetParent(_grid);
                obj.layer = _layerIndex;
                obj.AddComponent<TilemapCollider2D>();
            }

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