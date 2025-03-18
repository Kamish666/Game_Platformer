using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInitializer : Singleton<TilemapInitializer>
{
    [SerializeField] private List<BuildingBlockBase> _buildingBlockBase;
    [SerializeField] private Transform _grid;

    private void Start()
    {
        CreateMaps();
    }

    private void CreateMaps()
    {
        foreach (BuildingBlockBase category in _buildingBlockBase)
        {
            // Create new GameObject
            GameObject obj = new GameObject(category.name);

            // Assign Tilemap Features
            Tilemap map = obj.AddComponent<Tilemap>();
            category.TileRenderer = obj.AddComponent<TilemapRenderer>();

            // Set Parent
            obj.transform.SetParent(_grid);


            category.TileMap = map;
        }
    }
}
