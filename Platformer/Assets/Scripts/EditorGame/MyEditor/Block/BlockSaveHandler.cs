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
    void Load();
}

public class BlockSaveHandler : MonoBehaviour, ISaveHandler
{
    private Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();
    //[SerializeField] private BoundsInt _bounds;
    [SerializeField] private string _fileName = "tilemapData.json";

    private string _path;

    private void Start()
    {
        initTilemap();
    }

    private void initTilemap()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach (Tilemap map in maps)
        {
            _tilemaps.Add(map.name, map);
            //.Log(map.name);
        }
    }

    public void Save(string path)
    {
        _path = Path.Combine(path, _fileName);

        List<TilemapData> data = new List<TilemapData>();

        foreach (var mapObj in _tilemaps)
        {
            TilemapData mapData = new TilemapData();
            mapData.key = mapObj.Key;


            BoundsInt boundsForThisMap = mapObj.Value.cellBounds;

            for (int x = boundsForThisMap.xMin; x < boundsForThisMap.xMax; x++)
                for (int y = boundsForThisMap.yMin; y < boundsForThisMap.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y);
                    TileBase tile = mapObj.Value.GetTile(pos);

                    if (tile != null)
                    {
                        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(tile, out string guid, out long localId))
                        {
                            TileInfo ti = new TileInfo(tile, pos, guid);
                            mapData.tiles.Add(ti);
                        }
                        else
                        {
                            Debug.Log(tile.name + " ошибка");
                        }
                    }
                }

            data.Add(mapData);
        }

        FileHandler.SaveToJSON<TilemapData>(data, _path);
    }

    public void Load()
    {
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(_path);

        foreach (var mapData in data)
        {
            if (!_tilemaps.ContainsKey(mapData.key))
            {
                Debug.Log($"{mapData.key} нету");
                continue;
            }

            var map = _tilemaps[mapData.key];
            map.ClearAllTiles();

            if (mapData.tiles != null && mapData.tiles.Count > 0)
                foreach (var tile in mapData.tiles)
                {
                    TileBase tileBase = tile.tile;
                    if (tileBase == null)
                    {
                        Debug.Log("InstainceId not found");
                        string path = AssetDatabase.GUIDToAssetPath(tile.guidFromAssetDB);
                        tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(path);

                        if (tileBase == null)
                        {
                            Debug.Log("Tile not founded in asset store");
                            continue;
                        }
                    }

                    map.SetTile(tile.position, tile.tile);
                }
        }
    }
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
    public string guidFromAssetDB;
    public Vector3Int position;

    public TileInfo(TileBase tile, Vector3Int position, string guid)
    {
        this.tile = tile;
        this.position = position;  
        this.guidFromAssetDB = guid;
    }
}

