using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class BlockToolController : Singleton<BlockToolController>
{
    private List<Tilemap> _tilemaps = new List<Tilemap>();

    private void Start()
    {
        List<Tilemap> maps = FindObjectsOfType<Tilemap>().ToList();

        maps.ForEach(map =>
        {
            if (map.name != "PreviewMap")
            {
                _tilemaps.Add(map);
            }
        });
    }

    public void Eraser(Vector3Int position)
    {
        _tilemaps.ForEach(map =>
        {
            map.SetTile(position, null);
        });
    }

    public void Eraser(Vector3Int position, Tilemap map)
    {
        map.SetTile(position, null);
    }
}
