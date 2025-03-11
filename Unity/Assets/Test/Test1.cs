using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test1 : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;
    public Tile tile2;
    public int range = 3;

    void Start()
    {
        ResetTile();
    }

    [Button("初始化蓝图")]
    public void ResetTile()
    {
        List<Vector3Int> positions = BluePrintUtility.GetTilesInRange(tilemap, new Vector2(0, 0), range);
        foreach (Vector3Int pos in positions)
        {
            tilemap.SetTile(pos, tile);
        }
    }

    [Button("显示可增加的Tile")]
    private void ShowNeighbors()
    {
        if (tilemap == null) return;

        List<Vector3Int> empty = BluePrintUtility.GetNeighborsEmpty(tilemap);

        foreach (Vector3Int pos in empty)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile == null)
            {
                tilemap.SetTile(pos, tile2);
            }
        }
    }


    [Button("Clear")]
    public void ClearTile()
    {
        tilemap.ClearAllTiles();
    }
}