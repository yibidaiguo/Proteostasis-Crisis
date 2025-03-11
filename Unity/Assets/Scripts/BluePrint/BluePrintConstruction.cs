using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 蓝图建造类，用于检查位置是否能建造
/// </summary>
public class BluePrintConstruction : MonoBehaviour
{
    private Tilemap tilemap;
    private BluePrintConfig config;
    private ConstructionManager.BuildState buildState => ConstructionManager.Instance.CurrentState;
    
    public void Init()
    {
        tilemap = GetComponent<Tilemap>();
        config = ConstructionManager.Instance.BluePrintConfig;
        
    }
     
    /// <summary>
    /// 生成默认的蓝图
    /// </summary>
    public void GenerateBaseBluePrint()
    {
        List<Vector3Int> positions = BluePrintUtility.GetTilesInRange(tilemap,config.DefaultTilePos, config.DefaultTileLayer);
        foreach (Vector3Int pos in positions)
        {
            tilemap.SetTile(pos, config.DefaultTile);
        }
    } 
    
    /// <summary>
    /// 获取可以添加蓝图的空白位置，并设置默认的UP蓝图
    /// </summary>
    private List<Vector3Int> GetCanAddBluePrintTile()
    {
        if (tilemap == null) return null;
        
        List<Vector3Int> empty = BluePrintUtility.GetNeighborsEmpty(tilemap);
        
        foreach (Vector3Int pos in empty)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile == null)
            {
                tilemap.SetTile(pos, config.DefaultUpTile);
            }
        }
        
        return empty;
    }

    /// <summary>
    /// 检查位置是否可以建造
    /// </summary>
    /// <param name="pos">建造的位置</param>
    /// <returns></returns>
    public bool CheckCanConstructed(Vector3 pos,ConstructedTileBase construction)
    {
        TileBase tile = tilemap.GetTile(tilemap.WorldToCell(pos));
        if (tile is BulePrintDefaultTile) return true;
        return false;
    }
    
    
}
