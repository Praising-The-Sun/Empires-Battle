using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
    public readonly int id;
    public readonly Color color;
    private HashSet<Vector3Int> m_tiles;

    public Province(int id, Color color)
    {
        this.id = id;
        this.color = color;
        m_tiles = new HashSet<Vector3Int>();
    }

    public void AddPosition(Vector2Int newPosition)
    {
        if (m_tiles.Contains((Vector3Int)newPosition))
            return;
        m_tiles.Add((Vector3Int)newPosition);
    }

    public void AddPosition(int x, int y)
    {
        AddPosition(new Vector2Int(x, y));
    }
}
