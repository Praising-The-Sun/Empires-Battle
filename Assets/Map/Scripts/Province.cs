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

    public Player player { get; private set; } = null;

    public Province(int id, Color32 color, ref Tilemap tilemap, ref Tile tile, Player player = null)
    {
        this.id = id;
        this.color = color;

        m_tile = tile;
        m_tilemap = tilemap;

        m_tiles = new HashSet<Vector2Int>();

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

        m_tile.flags = TileFlags.LockTransform;
        m_tilemap.SetTile((Vector3Int)newPosition, m_tile);
    }

    /**
     * В зависимости от принадлежности к игроку красит тайлы + рисует границы
     */
    public void Render()
    {
        Vector2Int[] delta = new Vector2Int[]{
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
        };

        foreach (Vector2Int tile in m_tiles)
        {
            int cnt = 0;
            for (int i = 0; i < delta.Length; ++i)
                cnt += m_tiles.Contains(new Vector2Int(tile.x + delta[i].x, tile.y + delta[i].y)) ? 1 : 0;
            
            m_tilemap.SetColor((Vector3Int)tile, cnt < 4 ? Color.black :
                player != null ? (Color)player.color : Color.white);
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
