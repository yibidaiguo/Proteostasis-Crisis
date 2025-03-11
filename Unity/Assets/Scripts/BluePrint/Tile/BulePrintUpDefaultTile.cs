using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BulePrintUpDafaultTile", menuName = "Custom Tiles/BulePrintUpDafaultTile")]
public class BulePrintUpDefaultTile : Tile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        
        tileData.colliderType = ColliderType.Grid;

    }
}
