using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

/// <summary>
/// The gameboard is the logical representation of the different tiles. It also provides utility methods to manipulate the tiles stored in the gameboard.
/// Each tile instance is stored in the 2D "Tiles" array at their respective x and y coordinate.
/// </summary>
public class Gameboard
{
    public Tile[,] Tiles { get; private set; }
    public Vector2Int StartTile { get; }
    public Vector2Int EndTile { get; }

    public Gameboard()
    {
        //TODO: json
        int check = -1;
        Map board = loadJson("Assets/example_board_1.json");

        Tile[,] tiles = null;

        for (int i = 0; i < board.map.Count; i++)
        {
            for (int j = 0; j < board.map[i].Count; j++)
            {
                if (check == -1)
                {
                    tiles = new Tile[board.map.Count, board.map[i].Count];
                    check = board.map[i].Count;
                }
                else if (check != board.map[i].Count)
                {
                    notValiDJson();
                }

                tiles[i, j] = new Tile(false);

                if (!board.map[i][j].Equals("GRASS"))
                {
                    tiles[i, j].IsBlocked = true;

                    if (!board.map[i][j].Equals("START"))
                    {
                        StartTile = new Vector2Int(i, j);
                    }

                    if (board.map[i][j].Equals("END"))
                    {
                        EndTile = new Vector2Int(i, j);
                    }
                }
            }

            //Tiles = TileGenerator.CreateTestGameboard();
        }

        Tiles = tiles;
    }

    public bool IsTowerSpotOccupied(int x, int y)
    {
        return Utils.IsInRange2D(Tiles, x, y) && Tiles[x, y].IsTowerSet;
    }

        public bool IsTileBlocked(int x, int y)
    {
        return Utils.IsInRange2D(Tiles, x, y) && Tiles[x, y].IsBlocked;
    }

        public void PlaceTower(int x, int y)
    {
        Tiles[x, y].SetTower();
    }

    public class Map
    {
        public IList<IList<string>> map { get; set; }
    }

    private void notValiDJson()
    {
        throw new FileLoadException("File not valid");
    }

    private Map loadJson(string path)
    {
        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                var root = JsonConvert.DeserializeObject<Map>(json);
                Debug.Log("return json root");
                return root;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Not Valid Format.");
            throw;
        }
    }
}