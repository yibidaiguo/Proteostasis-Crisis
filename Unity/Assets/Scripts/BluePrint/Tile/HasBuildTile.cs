using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "HasBulidTile", menuName = "Custom Tiles/HasBulidTile")]
public class HasBuildTile : Tile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        tileData.colliderType = ColliderType.Grid;

    }
}
