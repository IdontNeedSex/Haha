using System.Data;
using UnityEngine;

public class Node2D
{
    public int gCost, hCost;
    public bool obstacle;

    public int GridX, GridY;
    public Node2D parent;


    public Node2D(bool _obstacle, int _gridX, int _gridY)
    {
        obstacle = _obstacle;
        GridX = _gridX;
        GridY = _gridY;
    }

    public int FCost => gCost + hCost;


    public void SetObstacle(bool isOb)
    {
        obstacle = isOb;
    }
}
