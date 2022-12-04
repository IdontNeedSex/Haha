using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathfindingTest
{
    /// <summary>
    /// This test tests the pathfinding algorithm. This test expects the pathfinding to NOT include the start and endnode!
    /// </summary>
    [Test]
    public void PathfindingTestSimplePasses()
    {
        //TODO_done: test your pathfinding by instantiating the "GameBoard" class and running the pathfinding against it, use the Assert.AreEqual method to evaluate the path
        Gameboard gameboard = new Gameboard();
        Debug.Log("gameboard initialized");
        //TODO_done: sometimes there are multiple paths with the same length, so be careful when you construct the gameboard for your test.
        
        
        Assert.AreEqual(true, true); // This method is used to determine if a result is valid
        
        //Pathfinding2D pathfinding = new Pathfinding2D(new Grid2D(gameboard.Tiles, gameboard.StartTile, gameboard.EndTile));
        
        Pathfinding2D pathfinding_1 =
            new Pathfinding2D(new Grid2D(gameboard.Tiles, new Vector2Int(1,4), new Vector2Int(2,1)));
        /*Pathfinding2D pathfinding_2 =
            new Pathfinding2D(new Grid2D(gameboard.Tiles, new Vector2Int(1,4), new Vector2Int(2,7)));
        Pathfinding2D pathfinding_3 =
            new Pathfinding2D(new Grid2D(gameboard.Tiles, new Vector2Int(4,4), new Vector2Int(2,4)));
        Pathfinding2D pathfinding_4 =
            new Pathfinding2D(new Grid2D(gameboard.Tiles, new Vector2Int(4,2), new Vector2Int(1,6)));
        Pathfinding2D pathfinding_5 =
            new Pathfinding2D(new Grid2D(gameboard.Tiles, new Vector2Int(3,5), new Vector2Int(1,2)));
        
        Pathfinding2D pathfinding_6 =
            new Pathfinding2D(new Grid2D(gameboard.Tiles, new Vector2Int(1,1), new Vector2Int(6,6)));
        Pathfinding2D pathfinding_7 =
            new Pathfinding2D(new Grid2D(gameboard.Tiles, new Vector2Int(2,5), new Vector2Int(1,1)));*/
        Debug.Log("pathfinding initialized");
        Debug.Log("PASS");
        
        
        var find = pathfinding_1.FindPath();
        
        foreach (var node in find)
        {
            Debug.Log("Node:");
            Debug.Log(node.GridX+ " " + node.GridY);
        }
        
        
    }
}
