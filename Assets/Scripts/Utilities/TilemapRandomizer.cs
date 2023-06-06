using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRandomizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<Tile> tiles;
    void Start()
    {
        for (int y = (int)tilemap.localBounds.min.y; y < tilemap.localBounds.max.y; y++)
        {
            for (int x = (int)tilemap.localBounds.min.x; x < tilemap.localBounds.max.x; x++)
            {
                if (tilemap.GetTile(new Vector3Int(x, y, (int)tilemap.localBounds.min.z)) == tiles[0])
                {
                    tilemap.SetTile(new Vector3Int(x, y, (int)tilemap.localBounds.min.z), tiles[Random.Range(0, tiles.Count)]);
                }
            }
        }
    }

    void Update()
    {
        
    }
}
