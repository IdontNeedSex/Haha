using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

/// <summary>
/// The Toolmanager class handles
/// </summary>
public class ToolManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    private ToolType _toolType;
    
    private ToolEvents _toolEvents;
    private GameStateManager _gameStateManager;

    private void OnEnable()
    {
        SetupEventReferences();
    }

    private void SetupEventReferences()
    {
        _gameStateManager = GameObject.Find(AppConstants.GAMEOBJECT_NAME_MANAGERS).GetComponent<GameStateManager>();
        _toolEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<ToolEvents>();
    }

    private void Update()
    {
        var mousePos = Utils.GetMousePositionOnTilemap(Camera.main, tilemap);
        ProcessInputs(mousePos);
    }
    
    private void ProcessInputs(Vector3Int mousePos)
    {
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            ChangeTool(ToolType.PLACE_FAST_PROJECTILE_TOWER);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            ChangeTool(ToolType.PLACE_SLOW_PROJECTILE_TOWER);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            TriggerToolAction(mousePos);
        }
        
    }

    private void ChangeTool(ToolType toolType)
    {
        if (toolType == ToolType) return;
        ToolType = toolType;
        _toolEvents.TriggerOnToolChange(toolType);
    }

    private void TriggerToolAction(Vector3Int mousePos)
    {
        switch (_gameStateManager.GameState)
        {
            case GameState.TOWER:
                _toolEvents.TriggerOnToolAction(ToolType, mousePos);
                break;
            case GameState.START:
            case GameState.WAVE:
            case GameState.FINISHED:
            default:
                break;
        }
    }


    public ToolType ToolType
    {
        get => _toolType;
        private set => _toolType = value;
    }    
}

