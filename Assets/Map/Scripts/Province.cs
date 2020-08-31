using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Province
{
    public readonly int id;
    public readonly Color32 color;

    private readonly Tilemap m_tilemap;
    private readonly Tile m_tile;

    private HashSet<Vector3Int> m_tiles;

    public Province(int id, Color32 color, ref Tilemap tilemap, ref Tile tile)
    {
        this.id = id;
        this.color = color;
        m_tile = tile;
        m_tilemap = tilemap;
        m_tiles = new HashSet<Vector3Int>();
    }

    public void AddPosition(Vector2Int newPosition)
    {
        if (m_tiles.Contains((Vector3Int)newPosition))
            return;
        m_tiles.Add((Vector3Int)newPosition);

        m_tile.color = color;
        m_tilemap.SetTile((Vector3Int)newPosition, m_tile);
    }

    public void AddPosition(int x, int y)
    {
        AddPosition(new Vector2Int(x, y));
    }
}
