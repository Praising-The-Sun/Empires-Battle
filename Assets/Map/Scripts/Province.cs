using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Province
{
    public readonly int id; // Персональный id (позиция объекта в массиве WorldMap.instance.provinces)
    public readonly Color32 color; // Цвет провинции на изображении

    private readonly Tilemap m_tilemap;
    private readonly Tile m_tile;

    private HashSet<Vector2Int> m_tiles; // Тайлы, которые принадлежат данной провинции

    public Province(int id, Color32 color, ref Tilemap tilemap, ref Tile tile)
    {
        this.id = id;
        this.color = color;
        m_tile = tile;
        m_tilemap = tilemap;
        m_tiles = new HashSet<Vector2Int>();
    }

    /**
     * Добавляет новый тайл в провинцию, если такой есть - игнорирует
     */
    public void AddPosition(Vector2Int newPosition)
    {
        if (m_tiles.Contains(newPosition))
            return;
        m_tiles.Add(newPosition);

        m_tile.color = color;
        m_tilemap.SetTile((Vector3Int)newPosition, m_tile);
    }

    /**
     * Добавляет новый тайл в провинцию, если такой есть - игнорирует
     */
    public void AddPosition(int x, int y)
    {
        AddPosition(new Vector2Int(x, y));
    }
}
