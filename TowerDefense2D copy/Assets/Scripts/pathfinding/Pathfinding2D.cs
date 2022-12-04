using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class Pathfinding2D
{

    private const int move_cost = 10;

    private List<Node2D> openSet;
    private List<Node2D> closedSet;

    public Pathfinding2D(Grid2D grid2D)
    {
        this.Grid = grid2D;
    }

    private Grid2D Grid { get; }


    public List<Node2D> FindPath()
    {

        Node2D start = Grid.Grid[Grid.StartTile.x, Grid.StartTile.y];
        Node2D end = Grid.Grid[Grid.EndTile.x, Grid.EndTile.y];

        for (int i = 0; i < Grid._gridSizeX; i++)
        {
            for (int j = 0; j < Grid._gridSizeY; j++)
            {
                var node = Grid.Grid[i, j];
                node.gCost = int.MaxValue;
                node.parent = null;
            }
        }

        //TODO_done: Implement a shortest-path-pathfinding algorithmus (A-Star recommended) to find the shortest path between the start and the endNode
        //TODO_done: Note: the "Node2D" class is prepared for the A-Star algorithm, you may need to modify them for any other algorithm

        List<Node2D> openSet = new List<Node2D>();
        HashSet<Node2D> closedSet = new HashSet<Node2D>();
        openSet.Add(start);

        //calculates path for pathfinding
        while (openSet.Count > 0)
        {

            //iterates through openSet and finds lowest FCost
            Node2D node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost <= node.FCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            //If target found, retrace path
            if (node == end)
            {
                return RetracePath(end); ;
            }

            

            List<Node2D> neighbors = Grid.GetNeighbors(node);

            //adds neighbor nodes to openSet
            foreach (var neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor)) continue;
                if (neighbor.obstacle && neighbor != end)
                {
                    closedSet.Add(neighbor); continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbor);
                if (newCostToNeighbour < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbour;
                    neighbor.hCost = GetDistance(neighbor, end);
                    neighbor.parent = node;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                        
                }
            }

           
        }
        return new List<Node2D>();
    }



int GetDistance(Node2D nodeA, Node2D nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    
    private List <Node2D>RetracePath( Node2D endNode)
    {
        List<Node2D> path = new List<Node2D>();
        Node2D currentNode = endNode;

        while (currentNode.parent != null)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();
        return path;
    }
    
    
    
    public void SetObstacle(bool isObstacle, int x, int y)
    {
        Grid.Grid[x,y].SetObstacle(isObstacle);
    }
    
    
}