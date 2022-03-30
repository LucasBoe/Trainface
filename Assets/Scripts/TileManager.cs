using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    Dictionary<Vector2Int, Tile> tileRegistry = new Dictionary<Vector2Int, Tile>();
    internal void RegisterTile(Tile tile)
    {
        Vector3 worldPos = tile.transform.position;
        Vector2Int pos = WorldToTilePos(worldPos);
        tileRegistry.Add(pos, tile);
    }

    private static Vector2Int WorldToTilePos(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.z));
    }

    public Tile GetNeightbour(Tile tile, TileDirection direction)
    {
        Vector2Int tilePos = WorldToTilePos(tile.transform.position + direction.ToDirectionVector());

        if (tileRegistry.ContainsKey(tilePos))
            return tileRegistry[tilePos];

        return null;
    }
}
