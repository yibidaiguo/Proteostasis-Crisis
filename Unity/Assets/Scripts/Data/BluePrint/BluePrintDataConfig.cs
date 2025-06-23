using System.Collections.Generic;
using JKFrame;
using UnityEngine;

public class BluePrintDataConfig : ConfigBase
{
    public Dictionary<Vector3Int,int> hexagons = new();
}

public class BluePrintData
{
    public Dictionary<Vector3Int,int> hexagons = new();
}
