using UnityEngine;
using Random = System.Random;

public class TileGenerator
{
    private static int SIZE_X = 5;
    private static int SIZE_Y = 5;
    
    public static Tile[,] CreateTestGameboard()
    {
        var tiles = new Tile[SIZE_X, SIZE_Y];
        for (var x = 0; x < SIZE_X; x++)
        {
            for (var y = 0; y < SIZE_Y; y++)
            {
                tiles[x, y] = new Tile(false);
            }
        }
        tiles[2, 1].IsBlocked = true;
        tiles[2, 2].IsBlocked = true;
        tiles[2, 3].IsBlocked = true;
        tiles[0, 1].IsBlocked = true;
        tiles[0, 3].IsBlocked = true;
        tiles[4, 3].IsBlocked = true;
        return tiles;
    }


    public static bool IsPassable(Tile[,] tiles, Vector2Int startTile, Vector2Int endTile)
    {
        var pathfinding = new Pathfinding2D(new Grid2D(tiles, startTile, endTile));
        return pathfinding.FindPath().Count > 0;
    }
}