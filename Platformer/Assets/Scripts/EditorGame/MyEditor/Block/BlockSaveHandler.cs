using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Tilemaps;


public interface ISaveHandler
{
    void Save(string path);
    void Load(string path);
}

public class BlockSaveHandler : MonoBehaviour, ISaveHandler
{
    private Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();

    [SerializeField] private string _fileName = "tilemapData.json";
    private string _path;

    private void Awake()
    {
        initTilemap();
        Debug.Log("BlockSaveHandler");
    }

    private void initTilemap()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>(true);
        foreach (Tilemap map in maps)
        {
            _tilemaps[map.name] = map;
        }
    }

    public void Save(string pathForFolder)
    {
        GetFullPath(pathForFolder);

        List<TilemapData> data = new List<TilemapData>();

        foreach (var mapObj in _tilemaps)
        {
            TilemapData mapData = new TilemapData { key = mapObj.Key };
            BoundsInt bounds = mapObj.Value.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y);
                    TileBase tile = mapObj.Value.GetTile(pos);

                    if (tile != null)
                    {
                        TileInfo tileInfo = new TileInfo
                        {
                            tile = tile,
                            tileName = tile.name,
                            position = pos
                        };
                        mapData.tiles.Add(tileInfo);
                    }
                }
            }

            data.Add(mapData);
        }

        FileHandler.SaveToJSON(data, _path);
    }

    public void Load(string pathForFolder)
    {
        GetFullPath(pathForFolder);
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(_path);
        Dictionary<string, TileBase> tileCache = new Dictionary<string, TileBase>();

        foreach (var mapData in data)
        {
            if (!_tilemaps.ContainsKey(mapData.key))
            {
                Debug.LogWarning($"Tilemap {mapData.key} not found.");
                continue;
            }

            Tilemap map = _tilemaps[mapData.key];
            map.ClearAllTiles();

            foreach (var tileInfo in mapData.tiles)
            {
                TileBase tile = tileInfo.tile;

                // Если прямая ссылка не работает — пробуем через кэш и Resources
                if (tile == null)
                {
                    if (!tileCache.TryGetValue(tileInfo.tileName, out tile))
                    {
                        tile = Resources.Load<TileBase>($"Tiles/{tileInfo.tileName}");
                        tileCache[tileInfo.tileName] = tile;
                    }
                }

                if (tile != null)
                {
                    map.SetTile(tileInfo.position, tile);
                }
                else
                {
                    Debug.LogWarning($"Tile '{tileInfo.tileName}' not found in memory or Resources.");
                }
            }
        }
    }

    private string GetFullPath(string path) => _path = Path.Combine(path, _fileName);
}

[Serializable]
public class TilemapData
{
    public string key;
    public List<TileInfo> tiles = new List<TileInfo>();
}

[Serializable]
public class TileInfo
{    
    public TileBase tile;
    public string tileName;
    public Vector3Int position;
}
