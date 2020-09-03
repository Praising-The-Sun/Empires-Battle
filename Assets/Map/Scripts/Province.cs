using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Province
{
    private readonly static Vector2Int[] delta = new Vector2Int[]{
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
    };

    public readonly int id; // Персональный id (позиция объекта в массиве WorldMap.instance.provinces)
    public readonly Color32 color; // Цвет провинции на изображении

    private readonly Tilemap m_tilemap;
    private readonly Tile m_tile;

    private HashSet<Vector2Int> m_tiles; // Тайлы, которые принадлежат данной провинции

    public Player player { get; private set; } = null;

    public HashSet<Province> neighbours { get; private set; }

    public Province(int id, Color32 color, ref Tilemap tilemap, ref Tile tile, Player player = null)
    {
        this.id = id;
        this.color = color;

        m_tile = tile;
        m_tile.flags = TileFlags.LockTransform;

        m_tilemap = tilemap;

        m_tiles = new HashSet<Vector2Int>();

        neighbours = new HashSet<Province>();

        SetPlayer(player);
    }

    /**
     * Добавляет новый тайл в провинцию, если такой есть - игнорирует
     */
    public void AddPosition(Vector2Int newPosition)
    {
        if (m_tiles.Contains(newPosition))
            return;
        m_tiles.Add(newPosition);
        
        m_tilemap.SetTile((Vector3Int)newPosition, m_tile);
    }

    public void FindNeighbours()
    {
        neighbours.Clear();

        foreach (Vector2Int tile in m_tiles)
        {

            for (int i = 0; i < delta.Length; ++i)
            {
                Vector2Int pos = tile + delta[i];
                if (!m_tiles.Contains(pos) && WorldMap.instance.tilemap.ContainsKey(pos))
                {
                    neighbours.Add(WorldMap.instance.tilemap[pos]);
                }
            }
        }
    }

    /**
     * Красит тайлы + рисует границы
     */
    public void Render(Color32? color = null)
    {
        foreach (Vector2Int tile in m_tiles)
        {
            int cnt = 0;
            for (int i = 0; i < delta.Length; ++i)
            {
                cnt += m_tiles.Contains(tile + delta[i]) ? 1 : 0;
            }

            Color resultColor = Color.white;
            if (player != null)
                resultColor = (Color)player.color;
            if (color != null)
                resultColor = (Color)color;
            if (cnt < 4)
                resultColor = Color.black; 
            
            m_tilemap.SetColor((Vector3Int)tile, resultColor);
        }
    }
    
    /**
     * Устанавлиает провинции нового хозяина, меняя цвет и прочее
     */
    public void SetPlayer(Player newPlayer)
    {
        if (player != null)
        {
            player.provinces.Remove(this);
        }

        player = newPlayer;

        if (player != null)
        {
            player.provinces.Add(this);
        }

        Render();
    }
}
