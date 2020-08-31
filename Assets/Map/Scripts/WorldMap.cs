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

    private void Awake()
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

            var color2Id = new Dictionary<Color32, int>();

            provinces = new List<Province>();
            using (StreamReader provincesId = new StreamReader(m_provincesIdPath))
            {
                if (provincesId == null)
                    throw new FileNotFoundException("World Map: couldn't find the provinces color id conventor!");
                
                string line;
                while ((line = provincesId.ReadLine()) != null)
                {
                    string[] tokens = line.Trim().Split(' ');

                    // Определение цвета
                    byte red = Convert.ToByte(tokens[0].Substring(0, 2), 16);
                    byte green = Convert.ToByte(tokens[0].Substring(2, 2), 16);
                    byte blue = Convert.ToByte(tokens[0].Substring(4, 2), 16);
                    
                    int id = provinces.Count + 1;
                    Color32 color = new Color32(red, green, blue, 255);
                    provinces.Add(new Province(id, color, ref m_tilemap, ref m_tile));
                    color2Id.Add(color, id);
                }
            }

            Color32[] pixels = provincesMap.GetPixels32();
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    int i = y * width + x;
                    if (color2Id.ContainsKey(pixels[i]))
                    {
                        provinces[color2Id[pixels[i]] - 1].AddPosition(x, y);
                    }
                }
            }
        }
    }
}
