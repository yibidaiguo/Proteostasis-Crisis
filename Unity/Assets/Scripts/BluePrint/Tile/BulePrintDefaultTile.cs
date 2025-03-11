using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BulePrintDefaultTile", menuName = "Custom Tiles/BulePrintDefaultTile")]
public class BulePrintDefaultTile : Tile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        
        tileData.colliderType = ColliderType.Grid;

    }
    
}
