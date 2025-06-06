using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BulePrintUpTile", menuName = "Custom Tiles/BulePrintUpTile")]
public class BulePrintUpTile : Tile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        
        tileData.colliderType = ColliderType.Grid;

    }
}
