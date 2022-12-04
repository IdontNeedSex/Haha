using System.Collections.Generic;
using UnityEngine;

public class Grid2D
{
    public Node2D[,] Grid;

    public int _gridSizeX;
    public int _gridSizeY;
    


    public Vector2Int StartTile { get; }

    public Vector2Int EndTile { get; }

    public Grid2D(Tile[,] tiles, Vector2Int startTile, Vector2Int endTile)
    {
        StartTile = startTile;
        EndTile = endTile;
        _gridSizeX = tiles.GetLength(0);
        _gridSizeY = tiles.GetLength(1);
        Grid = CreateGrid(tiles, _gridSizeX, _gridSizeY);
    }
    


    private Node2D[,] CreateGrid(Tile[,] tiles, int gridSizeX, int gridSizeY)
    {
        var grid = new Node2D[gridSizeX, gridSizeY];

        for (var x = 0; x < gridSizeX; x++)
        {
            for (var y = 0; y < gridSizeY; y++)
            {
                var tile = tiles[x, y];
                var isObstacle = tile.IsTowerSet || tile.IsBlocked;
                grid[x, y] = new Node2D(isObstacle, x, y);
                grid[x, y].gCost = int.MaxValue;
            }
        }

        return grid;
    }

    
    public List<Node2D> GetNeighbors(Node2D node)
    {
        var neighbors = new List<Node2D>();

        //TODO_done: get the neighboring squares, Hint: all nodes are stored in the "Grid" variable 

        //checks and adds top neighbor
        if (node.GridX >= 0 && node.GridX < _gridSizeX && node.GridY + 1 >= 0 && node.GridY + 1 < _gridSizeY)
            neighbors.Add(Grid[node.GridX, node.GridY + 1]);

        //checks and adds bottom neighbor
        if (node.GridX >= 0 && node.GridX < _gridSizeX && node.GridY - 1 >= 0 && node.GridY - 1 < _gridSizeY)
            neighbors.Add(Grid[node.GridX, node.GridY - 1]);

        //checks and adds right neighbor
        if (node.GridX + 1 >= 0 && node.GridX + 1 < _gridSizeX && node.GridY >= 0 && node.GridY < _gridSizeY)
            neighbors.Add(Grid[node.GridX + 1, node.GridY]);

        //checks and adds left neighbor
        if (node.GridX - 1 >= 0 && node.GridX - 1 < _gridSizeX && node.GridY >= 0 && node.GridY < _gridSizeY)
            neighbors.Add(Grid[node.GridX - 1, node.GridY]);
        
        return neighbors;
    }
}