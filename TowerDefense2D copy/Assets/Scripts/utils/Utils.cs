using UnityEngine;
using UnityEngine.Tilemaps;

public class Utils
{
    public static Vector3Int GetMousePositionOnTilemap (Camera camera, Tilemap tilemap) {
        var mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        return tilemap.WorldToCell(mouseWorldPos);
    }

    /// <summary>
    /// Simple generic index check for 2D array
    /// </summary>
    /// <param name="arr">array where the index should be in</param>
    /// <param name="x">x index</param>
    /// <param name="y">y index</param>
    /// <returns>True if both indexes are in the boundaries of the array</returns>
    public static bool IsInRange2D(object[,] arr, int x, int y)
    {
        return x >= 0 
               && x < arr.GetLength(0)
               && y >= 0 
               && y < arr.GetLength(1);
    }
}