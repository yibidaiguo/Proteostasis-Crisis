using JKFrame;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BluePrintConfig", menuName = "Config/BluePrintConfig")]
public class BluePrintConfig : ConfigBase
{
    [Header("默认蓝图Tile")]
    public Tile DefaultTile;
    [Header("默认可放置蓝图Tile")]
    public Tile DefaultUpTile;
    [Header("初始生成的蓝图位置")]
    public Vector2 DefaultTilePos;
    [Header("初始生成的蓝图圈数")][Range(1, 100)]
    public int DefaultTileLayer;
    
}
