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
    public BluePrintDataConfig curDataConfig;
    private ConstructionData constructionData = new ("",null, 0);
    private PlayerData playerConstructionData;

    void Start()
    {
        ResetTile();
        constructionData.bluePrintData = new BluePrintData();
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

    public void Update()
    {
        Dictionary<Vector3Int, int> neighbors = new ();
        foreach (var item in curDataConfig.hexagons)
        {
            neighbors.Add(item.Key, item.Value);
        }
        constructionData.bluePrintData.hexagons = neighbors;
        
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.transform.position.z > 0 ? Camera.main.transform.position.z : -Camera.main.transform.position.z));
            Debug.Log("开始建造"+ worldPos);
            ConstructionManager.Instance.Constructed(worldPos,constructionData);
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.transform.position.z > 0 ? Camera.main.transform.position.z : -Camera.main.transform.position.z));
            Debug.Log("开始移除"+ worldPos);
            ConstructionManager.Instance.DestoryConstruction(worldPos);
        }

        if (Input.GetKeyDown(KeyCode.R) && curDataConfig != null)
        {
            constructionData.rotation++;
            Debug.Log(constructionData.rotation);
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            ConstructionManager.Instance.curData = constructionData;
        }
    }
}