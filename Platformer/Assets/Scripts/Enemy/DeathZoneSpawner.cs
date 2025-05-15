using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeathZoneSpawner : MonoBehaviour
{
    public GameObject deathZonePrefab;
    public float offsetY = 10f;

    void Start()
    {
        SpawnDeathZone();
    }

    void SpawnDeathZone()
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();

        float lowestY = float.MaxValue;
        bool foundTile = false;

        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    Vector3 worldPos = tilemap.CellToWorld(pos);
                    if (worldPos.y < lowestY)
                    {
                        lowestY = worldPos.y;
                        foundTile = true;
                    }
                }
            }
        }

        if (foundTile)
        {
            Vector3 spawnPos = new Vector3(0, lowestY - offsetY, 0);
            Instantiate(deathZonePrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Ни одна плитка не найдена.");
        }
    }
}
