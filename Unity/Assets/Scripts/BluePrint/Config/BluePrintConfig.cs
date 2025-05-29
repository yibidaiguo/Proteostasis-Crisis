using JKFrame;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BluePrintConfig", menuName = "Config/BluePrintConfig")]
public class BluePrintConfig : ConfigBase
{
    [Header("默认未放置蓝图Tile")]
    public CanBulidTile DefaultCanBuildTile;
    [Header("默认升级蓝图Tile")]
    public BulePrintUpTile DefaultUpTile;
    [Header("默认已放置蓝图Tile")]
    public HasBuildTile DefaultHasBuildTile;
    [Header("View层可放置Tile")]
    public TileBase DefaultViewCanTile;
    [Header("View层不可放置Tile")]
    public TileBase DefaultViewCannotTile;
    [Header("初始生成的蓝图位置")]
    public Vector2 DefaultTilePos;
    [Header("初始生成的蓝图圈数")][Range(1, 100)]
    public int DefaultTileLayer;
    
}
