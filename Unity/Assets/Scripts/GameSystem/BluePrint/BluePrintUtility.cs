using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 蓝图工具类，使用的算法开销都较大，不建议频繁调用，如果n比较小就无所谓
/// </summary>
public static class BluePrintUtility
{
    public const float sqrt3 = 1.73205080757f; // Mathf.Sqrt(3)的近似值
    public const float cos60 = 0.5f;
    public const float sin60 = 0.8660254f; // Mathf.Sqrt(3)/2
    public static readonly Vector3Int[] Directions =
    {
        new Vector3Int(1, 0), // 右
        new Vector3Int(1, -1), // 右下
        new Vector3Int(0, -1), // 下
        new Vector3Int(-1, 0), // 左
        new Vector3Int(-1, 1), // 左上
        new Vector3Int(0, 1), // 上
    };
    
    #region 初始化

    /// <summary>
    /// 获取从中心点出发第1到第n圈的所有六边形Tile坐标
    /// </summary>
    /// <param name="tilemap">Tilemap对象</param>
    /// <param name="centerWorldPos">中心点的世界坐标</param>
    /// <param name="n">圈数</param>
    /// <returns>所有Tile列表</returns>
    public static List<Vector3Int> GetTilesInRange(Tilemap tilemap, Vector2 centerWorldPos, int n)
    {
        Vector3Int centerCell = tilemap.WorldToCell(centerWorldPos);
        HashSet<Vector3Int> allCoords = GetCoordsWithinLayers(centerCell, n);

        List<Vector3Int> positions = new List<Vector3Int>();
        float radius = tilemap.cellSize.y / 2;
        foreach (Vector3Int tileCoord in allCoords)
        {
            Vector3Int tileWorldPos =
                tilemap.WorldToCell(GetCoordsWithinRadius(centerWorldPos, tileCoord, radius));
            positions.Add(tileWorldPos);
        }

        return positions;
    }

    /// <summary>
    /// 广度优先搜索生成1~n层的tile坐标
    /// </summary>
    public static HashSet<Vector3Int> GetCoordsWithinLayers(Vector3Int start, int n)
    {
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(start);
        visited.Add(start);

        for (int layer = 0; layer < n; layer++)
        {
            int nodesInCurrentLayer = queue.Count;
            for (int i = 0; i < nodesInCurrentLayer; i++)
            {
                Vector3Int current = queue.Dequeue();
                foreach (Vector3Int dir in Directions)
                {
                    Vector3Int neighbor = current + dir;
                    if (visited.Add(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return visited;
    }

    #endregion

    #region 获取可用于放置蓝图的空Tile坐标

    /// <summary>
    /// 获取所有与有效Tile相邻的空Tile坐标
    /// </summary>
    public static List<Vector3Int> GetNeighborsEmpty(Tilemap tilemap)
    {
        HashSet<Vector3Int> occupiedTiles = GetAllOccupiedTiles(tilemap);
        HashSet<Vector3Int> emptyNeighbors = new HashSet<Vector3Int>();

        float radius = tilemap.cellSize.y / 2;

        foreach (Vector3Int tilePos in occupiedTiles)
        {
            Vector3 worldPos = tilemap.CellToWorld(tilePos);

            // 检查每个方向
            foreach (Vector3Int dir in Directions)
            {
                Vector3 neighborWorldPos = GetCoordsWithinRadius(worldPos, dir, radius);
                Vector3Int neighborCell = tilemap.WorldToCell(neighborWorldPos);

                // 如果邻居位置为空且未被记录
                if (!occupiedTiles.Contains(neighborCell) && !tilemap.HasTile(neighborCell))
                {
                    emptyNeighbors.Add(neighborCell);
                }
            }
        }

        return new List<Vector3Int>(emptyNeighbors);
    }

    /// <summary>
    /// 获取tile周围的空Tile坐标
    /// </summary>
    /// <param name="tilemap"></param>
    /// <param name="tile"></param>
    /// <returns></returns>
    public static List<Vector3Int> GetTileNeighborsEmpty(Tilemap tilemap, Vector3Int tile)
    {
        if (!(tilemap.GetTile(tile) is CanBulidTile)) return null;
        HashSet<Vector3Int> emptyNeighbors = new HashSet<Vector3Int>();
        float radius = tilemap.cellSize.y / 2;
        Vector3 worldPos = tilemap.CellToWorld(tile);
        foreach (Vector3Int dir in Directions)
        {
            Vector3 neighborWorldPos = GetCoordsWithinRadius(worldPos, dir, radius);
            Vector3Int neighborCell = tilemap.WorldToCell(neighborWorldPos);
            
            if (!tilemap.HasTile(neighborCell))
            {
                emptyNeighbors.Add(neighborCell);
            }
        }

        return new List<Vector3Int>(emptyNeighbors);
    }

    /// <summary>
    /// 获取tilemap中所有有效Tile的坐标
    /// </summary>
    private static HashSet<Vector3Int> GetAllOccupiedTiles(Tilemap tilemap)
    {
        HashSet<Vector3Int> occupied = new HashSet<Vector3Int>();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos) && tilemap.GetTile(pos) is CanBulidTile)
            {
                occupied.Add(pos);
            }
        }

        return occupied;
    }

    #endregion
    
    /// <summary>
    /// 根据中心点和offset坐标，计算出在半径为radius的范围内的偏移坐标
    /// </summary>
    public static Vector3 GetCoordsWithinRadius(Vector3 center, Vector3Int offset, float radius)
    {
        float x = offset.x * radius * 1.5f;
        float y = (offset.y + offset.x * 0.5f) * radius * sqrt3;
        return new Vector3(center.x + x, center.y + y, 0);
    }
    
    /// <summary>
    /// 计算坐标系中的层数
    /// </summary>
    /// <param name="coord">立方坐标系坐标</param>
    /// <returns>层数（0为中心层）</returns>
    public static int GetLayer(Vector3Int coord)
    {
        return (Mathf.Abs(coord.x) + Mathf.Abs(coord.y) + Mathf.Abs(coord.z)) / 2 + 1;
    }
    
    /// <summary>
    /// 旋转坐标
    /// </summary>
    /// <param name="original">原始坐标</param>
    /// <param name="rotationSteps">旋转步数（正数顺时针，负数逆时针，1步=60度）</param>
    /// <returns>旋转后的坐标</returns>
    public static List<Vector3> RotateHex(
        List<Vector3> original,Vector3 center,
        int rotationSteps)
    {
        rotationSteps %= 6;
        if (rotationSteps < 0) rotationSteps += 6; // 将负数步数转换为等效正数旋转
        
        List<Vector3> result = new ();
        foreach (var pos in original)
        {
            Vector3 rotatedPos = pos;
            // 应用旋转次数
            for (int i = 0; i < rotationSteps; i++)
            {
                rotatedPos = RotateSingleStep(rotatedPos,center);
            }
            result.Add(rotatedPos);
        }
        return result;
    }

    /// <summary>
    /// 单次顺时针旋转60度的坐标变换
    /// </summary>
    private static Vector3 RotateSingleStep(Vector3 pos,Vector3 center)
    {
        Vector3 relativePos = pos - center;
        
        float rotatedX = relativePos.x * cos60 + relativePos.y * sin60;
        float rotatedY = -relativePos.x * sin60 + relativePos.y * cos60;
        
        return new Vector3(
            rotatedX + center.x,
            rotatedY + center.y,
            pos.z  
        );
    }
}