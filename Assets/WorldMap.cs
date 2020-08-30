using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WorldMap : MonoBehaviour
{
    public static WorldMap instance = null;
    [SerializeField, Header("Tilemap")]
    private Tilemap m_tilemap;
    [SerializeField]
    private Tile m_tile;

    [SerializeField, Header("Paths")]
    private string m_provincesImagePath = "";
    [SerializeField]
    private string m_provincesIdPath = "";

    [Header("Non optional parametors")]
    public int width;
    public int height;

    public List<Province> provinces;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        var provincesMap = new Texture2D(0, 0);
        bool loaded = provincesMap.LoadImage(File.ReadAllBytes(m_provincesImagePath));
        if (loaded)
        {
            if (provincesMap == null)
                throw new FileNotFoundException("World Map: couldn't find the provinces map!");

            width = provincesMap.width;
            height = provincesMap.height;

            var color2Id = new Dictionary<Color, int>();

            provinces = new List<Province>();
            using (StreamReader provincesId = new StreamReader(m_provincesIdPath))
            {
                if (provincesId == null)
                    throw new ArgumentNullException("World Map: couldn't find the provinces color id conventor!");
                string line;
                while ((line = provincesId.ReadLine()) != null)
                {
                    string[] tokens = line.Trim().Split(' ');

                    // Определение цвета
                    float red = Convert.ToInt32(tokens[1].Substring(0, 2), 16) / 255f;
                    float blue = Convert.ToInt32(tokens[1].Substring(2, 2), 16) / 255f;
                    float green = Convert.ToInt32(tokens[1].Substring(4, 2), 16) / 255f;

                    int id = int.Parse(tokens[0]);
                    Color color = new Color(red, blue, green);

                    provinces.Add(new Province(id, color));
                    color2Id.Add(color, id);
                }
            }

            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    Color color = provincesMap.GetPixel(x, y);
                    if (color2Id.ContainsKey(color))
                    {
                        provinces[color2Id[color] - 1].AddPosition(x, y);
                        
                    } else
                    {
                        m_tilemap.SetTile(new Vector3Int(x, y, 0), m_tile);
                    }
                }
            }
        }
    }
}
