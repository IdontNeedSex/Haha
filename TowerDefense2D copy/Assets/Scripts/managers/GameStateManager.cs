using System;
using UnityEngine;

/// <summary>
/// This class handles the overall gamestate and propagates it via events
/// </summary>
public class GameStateManager : MonoBehaviour
{
    private GameState _gameState = GameState.TOWER;
    private GameStateEvents _gameStateEvents;

    public GameState GameState
    {
        get => _gameState;
        private set => _gameState = value;
    }

    private void Start()
    {
        SetupEventReferences();
    }

    private void SetupEventReferences()
    {
        _gameStateEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<GameStateEvents>();
    }

    public void SetGameState(GameState gameState)
    {
        if (gameState == GameState) return;
        GameState = gameState;
        _gameStateEvents.TriggerOnGameStateChangedEvent(gameState);
    }
}

public enum GameState
{
    START,
    TOWER,
    WAVE,
    FINISHED
}