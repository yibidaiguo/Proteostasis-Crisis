using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructedTileBase : Tile
{ 
    public BluePrintData bluePrintData;

    public Vector3Int[] GetContructionPositions()
    {
        return bluePrintData.hexagons.Keys.ToArray();
    }
}
