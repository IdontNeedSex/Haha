using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// The script handles visualizing tools and the current path
/// </summary>
public class HighlightManager : MonoBehaviour
{
    //TODO_done: The tilemap which should be passed to the "highlightTileMap" variable does not exist yet, add a "Tilemap" via the editor and pass the reference to the HighlightManager component (located in the hierarchy at the "_managers" gameobject)
    //TODO_done+: ensure that the Tilemaps "Order in Layer" parameter (Tilemap Renderer component located at the created Tilemap) is set to -7, you should now be able to see a tower sprite following your mouse when you hover the gameboard in playmode
    /// <summary>
    /// Tilemaps can be layered, this tilemap only contains the highlighting
    /// </summary>
    [SerializeField] private Tilemap highlightTileMap;
    /// <summary>
    /// Visual tile to highlight the path the enemies will follow
    /// </summary>
    [SerializeField] private TileBase pathTileBase;
    /// <summary>
    /// Visual tile to indicate where the start is
    /// </summary>
    [SerializeField] private TileBase startTileBase;
    /// <summary>
    /// Visual tile to indicate where the end is
    /// </summary>
    [SerializeField] private TileBase endTileBase;
    /// <summary>
    /// Tilemaps can be layered, this tilemap only contains the start and end tile
    /// </summary>
    [SerializeField] private Tilemap startEndPathTileMap;
    /// <summary>
    /// Visual tile when a fast tower is placed
    /// </summary>
    [SerializeField] private TileBase fastProjectileTowerTileBase;
    /// <summary>
    /// Visual tile when a slow tower is placed
    /// </summary>
    [SerializeField] private TileBase slowProjectileTowerTileBase;
    [SerializeField] private Image slowTowerHighlightImage;
    [SerializeField] private Image fasterTowerHighlightImage;
    
    private Vector3Int _curHighlightPos = new(0, 0, 0);
    private bool _forceHightlightUpdate = false;

    private ToolManager _toolManager;
    private Vector2Int _startTile;
    private Vector2Int _endTile;

    private bool _isHighlightingActive = true;
    private GameboardEvents _gameboardEvents;
    private GameStateEvents _gameStateEvents;
    private ToolEvents _toolEvents;

    private void OnEnable()
    {
        SetToolHighlightEnabledByToolType(ToolType.PLACE_FAST_PROJECTILE_TOWER);
        SetupEventReferences();
        SetupEventHooks();
    }

    private void OnDisable()
    {
        RemoveEventHooks();
    }

    private void RemoveEventHooks()
    {
        _toolEvents.OnToolTypeChangedEvent -= HandleOnToolTypeChangedEvent;
        _gameboardEvents.OnMapInitializedEvent -= HandleOnMapInitializedEvent;
        _gameboardEvents.OnTowerPlacedEvent -= HandleOnTowerPlacedEvent;
        _gameStateEvents.OnGameStateChangedEvent -= HandleOnGameStateChangedEvent;
    }

    private void SetupEventReferences()
    {
        _toolManager = GameObject.Find(AppConstants.GAMEOBJECT_NAME_MANAGERS).GetComponent<ToolManager>();
        _gameboardEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<GameboardEvents>();
        _gameStateEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<GameStateEvents>();
        _toolEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<ToolEvents>();
    }
    
    private void SetupEventHooks()
    {
        _toolEvents.OnToolTypeChangedEvent += HandleOnToolTypeChangedEvent;
        _gameboardEvents.OnMapInitializedEvent += HandleOnMapInitializedEvent;
        _gameboardEvents.OnTowerPlacedEvent += HandleOnTowerPlacedEvent;
        _gameStateEvents.OnGameStateChangedEvent += HandleOnGameStateChangedEvent;
    }

    private void HandleOnToolTypeChangedEvent(ToolType tooltype)
    {
        _forceHightlightUpdate = true;
        SetToolHighlightEnabledByToolType(tooltype);
    }

    private void SetToolHighlightEnabledByToolType(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.PLACE_FAST_PROJECTILE_TOWER:
                SetToolHighlightEnabled(true);
                break;
            case ToolType.PLACE_SLOW_PROJECTILE_TOWER:
                SetToolHighlightEnabled(false);
                break;
        }
    }

    private void SetToolHighlightEnabled(bool isFastTowerHighlight)
    {
        fasterTowerHighlightImage.enabled = isFastTowerHighlight;
        slowTowerHighlightImage.enabled = !isFastTowerHighlight;
    }

    private void HandleOnGameStateChangedEvent(GameState gameState)
    {
        _isHighlightingActive = gameState == GameState.TOWER;
        ClearLastHighlightedTile(_curHighlightPos);
    }

    private void HandleOnTowerPlacedEvent(TowerType projectileTower, int tileX, int tileY, List<Vector2Int> newPath)
    {
       HighlightStartEndAndPath(newPath, _startTile, _endTile);
    }

    private void HandleOnMapInitializedEvent(Tile[,] tiles, Vector2Int startTile, Vector2Int endTile, List<Vector2Int> path)
    {
        _startTile = startTile;
        _endTile = endTile;
        HighlightStartEndAndPath(path, startTile, endTile);
    }

    private void HighlightStartEndAndPath(List<Vector2Int> path, Vector2Int startTile, Vector2Int endTile)
    {
        startEndPathTileMap.ClearAllTiles();
        foreach (var tilePos in path)
        {
            startEndPathTileMap.SetTile(new Vector3Int(tilePos.x, tilePos.y), pathTileBase);
        }
        
        startEndPathTileMap.SetTile(new Vector3Int(startTile.x, startTile.y), startTileBase);
        startEndPathTileMap.SetTile(new Vector3Int(endTile.x, endTile.y), endTileBase);
    }

    private void ClearLastHighlightedTile(Vector3Int curHighlightedPos)
    {
        highlightTileMap.SetTile(curHighlightedPos, null);
    }

    private void SetHighlightedTile(Vector3Int pos, TileBase tile, bool forceUpdate)
    {
        if (_curHighlightPos == pos && !forceUpdate) return;
        ClearLastHighlightedTile(_curHighlightPos);

        _curHighlightPos = pos;
        highlightTileMap.SetTile(pos, tile);
    }

    void Update()
    {
        if (!_isHighlightingActive) return;
        
        if(highlightTileMap != null){
            var mousePos = Utils.GetMousePositionOnTilemap(Camera.main, highlightTileMap);
            HighlightTileBaseOnTool(mousePos);
        }
    }

    private void HighlightTileBaseOnTool(Vector3Int mousePos)
    {
        var tile = _toolManager.ToolType switch
        {
            ToolType.PLACE_FAST_PROJECTILE_TOWER => fastProjectileTowerTileBase,
            ToolType.PLACE_SLOW_PROJECTILE_TOWER => slowProjectileTowerTileBase,
            _ => null
        };

        SetHighlightedTile(mousePos, tile, _forceHightlightUpdate);
        _forceHightlightUpdate = false;
    }
}