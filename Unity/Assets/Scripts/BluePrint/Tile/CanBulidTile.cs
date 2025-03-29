using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CanBulidTile", menuName = "Custom Tiles/CanBulidTile")]
public class CanBulidTile : Tile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        
        tileData.colliderType = ColliderType.Grid;

    }
}
