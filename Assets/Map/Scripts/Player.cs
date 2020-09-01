using System;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public HashSet<Province> provinces;
    public readonly Color32 color;

    public Player(Color32 color)
    {
        provinces = new HashSet<Province>();
        this.color = color;
    }

}