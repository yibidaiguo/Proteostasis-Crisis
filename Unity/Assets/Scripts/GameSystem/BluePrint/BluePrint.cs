using System;
using System.Collections.Generic;
using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 蓝图建造类，用于检查位置是否能建造
/// </summary>
public class BluePrint : MonoBehaviour
{
    [SerializeField] private Tilemap dataTilemap;
    [SerializeField] private Tilemap viewTilemap;
    private BluePrintConfig config;
    private float radius => dataTilemap.cellSize.y / 2;
    private Action showCheckAction;
    /// <summary>
    /// 存储建造的坐标数据，key为坐标，value为该坐标指向的建筑物的坐标
    /// </summary>
    [ShowInInspector]
    private Dictionary<Vector3Int, Vector3Int> posData = new();
    /// <summary>
    /// 存储建造的数据，key为坐标，value为该坐标指向的建筑物数据
    /// </summary>
    [ShowInInspector]
    private Dictionary<Vector3Int, ConstructionData> constructionData = new();
    
    public void Init()
    {
        config = ConstructionManager.Instance.BluePrintConfig;
        ConstructionManager.Instance.RegiesterBuildStateAction(ConstructionManager.BuildState.Start,OnStartBuilding); 
        ConstructionManager.Instance.RegiesterBuildStateAction(ConstructionManager.BuildState.Finished,OnStopBuilding);
        showCheckAction = ShowCheckResult;
    }

    private void OnStartBuilding()
    {
        MonoSystem.RemoveUpdateListener(showCheckAction);
        MonoSystem.AddUpdateListener(showCheckAction);
    }

    private void OnStopBuilding()
    {
        MonoSystem.RemoveUpdateListener(showCheckAction);
        viewTilemap.ClearAllTiles();
    }
    
    /// <summary>
    /// 显示可建造检查结果
    /// </summary>
    private void ShowCheckResult()
    {
        //TODO:目前使用鼠标位置作为检查位置，后续修改为动态获取
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.transform.position.z > 0
                ? Camera.main.transform.position.z
                : -Camera.main.transform.position.z));
        
        bool success = CheckCanConstructed(pos, ConstructionManager.Instance.curData, out var checkPoints);
        viewTilemap.ClearAllTiles();
        TileBase tile = success ? config.DefaultViewCanTile : config.DefaultViewCannotTile;
        foreach (Vector3Int checkPoint in checkPoints)
        {
            viewTilemap.SetTile(checkPoint, tile);
        }
    }
    
    /// <summary>
    /// 生成默认的蓝图
    /// </summary>
    public void GenerateBaseBluePrint()
    {
        if (dataTilemap == null) Debug.LogError("Tilemap is null");
        if (config == null) Debug.LogError("BluePrintConfig is null");
        if (config.DefaultCanBuildTile == null) Debug.LogError("DefaultCanBuildTile is null");
        List<Vector3Int> positions = BluePrintUtility.GetTilesInRange(dataTilemap,config.DefaultTilePos, config.DefaultTileLayer);
        foreach (Vector3Int pos in positions)
        {
            dataTilemap.SetTile(pos, config.DefaultCanBuildTile);
        }
    } 
    
    /// <summary>
    /// 获取可以添加蓝图的空白位置，并设置默认的UP蓝图
    /// </summary>
    private List<Vector3Int> GetCanAddBluePrintTile()
    {
        if (dataTilemap == null) return null;
        
        List<Vector3Int> empty = BluePrintUtility.GetNeighborsEmpty(dataTilemap);
        
        foreach (Vector3Int pos in empty)
        {
            TileBase tile = dataTilemap.GetTile(pos);
            if (tile == null)
            {
                dataTilemap.SetTile(pos, config.DefaultUpTile);
            }
        }
        return empty;
    }

    /// <summary>
    /// 检查位置是否可以建造
    /// </summary>
    /// <param name="pos">建造的位置</param>
    /// <returns></returns>
    public bool CheckCanConstructed(Vector3 pos,ConstructionData data,out List<Vector3Int> constructedPos)
    {
        constructedPos = GetDataV3IntInTilemap(pos,data);
        for (int i = 0;i < constructedPos.Count; i++)
        {
            if (!(dataTilemap.GetTile(constructedPos[i]) is CanBulidTile))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 在指定位置建造
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Constructed(Vector3 pos, ConstructionData data)
    {
        if (!CheckCanConstructed(pos, data,out var posList)) return false;
        foreach (Vector3Int constructionPos in posList)
        {
            dataTilemap.SetTile(constructionPos,config.DefaultHasBuildTile);
            posData.Add(constructionPos,dataTilemap.WorldToCell(pos));
        }
        ConstructionData newData = data.Clone();
        constructionData.Add(dataTilemap.WorldToCell(pos),newData);
        return true;
    }

    /// <summary>
    /// 获取指定位置的建造数据在Tilemap中的坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public List<Vector3Int> GetDataV3IntInTilemap(Vector3 pos, ConstructionData data)
    {
        List<Vector3> v3 = new ();
        foreach (Vector3Int neighbor in data.bluePrintData.hexagons.Keys)
        {
            Vector3 neighborPos = BluePrintUtility.GetCoordsWithinRadius(pos, neighbor, radius);
            v3.Add(neighborPos);
        }
        List<Vector3> positions = BluePrintUtility.RotateHex(v3,pos,data.rotation);
        List<Vector3Int> v3Ints = new();
        for (int i = 0;i < positions.Count; i++)
        {
            v3Ints.Add(dataTilemap.WorldToCell(positions[i]));
        }
        return v3Ints;
    }

    /// <summary>
    /// 移除指定位置的建造数据
    /// </summary>
    /// <param name="pos"></param>
    public bool RemoveConstructionData(Vector3 pos)
    {
        Vector3Int tilemapPos = dataTilemap.WorldToCell(pos);
        if (!posData.TryGetValue(tilemapPos, out var value)) return false;
        ConstructionData data = constructionData[value];
        Vector3 constructionPos = dataTilemap.CellToWorld(posData[tilemapPos]);
        List<Vector3Int> v3Ints = GetDataV3IntInTilemap(constructionPos, data);
        constructionData.Remove(posData[tilemapPos]);
        for (int i = 0;i < v3Ints.Count; i++)
        {
            posData.Remove(v3Ints[i]);
            dataTilemap.SetTile(v3Ints[i], ConstructionManager.Instance.BluePrintConfig.DefaultCanBuildTile);
        }
        return true;
    }
}
