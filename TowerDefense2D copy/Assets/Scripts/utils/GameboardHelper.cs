using UnityEngine;
using UnityEngine.Tilemaps;

public class GameboardHelper
{
    /// <summary>
    /// Generates the visual tiles in the tilemap based on obstacle or no obstacle
    /// TileBase is a superclass for most of the Unity classes which can be placed in Unitys Tilemap. In our example we use RuleTiles (which derive from TileBase). But you can also use for example AnimatedTiles (which also derive from TileBase)
    /// </summary>
    /// <param name="tiles">2D Tile array, data from the model</param>
    /// <param name="tilemap">tilemap instance where the tiles should be placed in</param>
    /// <param name="tileBaseNotBlocked">Tile which should be placed for non blocked tiles</param>
    /// <param name="tileBaseBlocked">Tile which should be placed for blocked tiles</param>
    public static void GenerateTilemap(Tile[,] tiles, Tilemap tilemap, TileBase tileBaseNotBlocked,
        TileBase tileBaseBlocked)
    {
        for (var x = 0; x < tiles.GetLength(0); x++)
        {
            for (var y = 0; y < tiles.GetLength(1); y++)
            {
                var tileBase = tiles[x, y].IsBlocked ? tileBaseBlocked : tileBaseNotBlocked;
                tilemap.SetTile(new Vector3Int(x, y, 0), tileBase);
            }
        }
    }
}