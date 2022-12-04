using UnityEngine;

public class GameStateEvents : MonoBehaviour
{
    public delegate void GameStateChangedEvent(GameState gameState);

    public event GameStateChangedEvent OnGameStateChangedEvent;

    public void TriggerOnGameStateChangedEvent(GameState gameState)
    {
        Debug.Log("EVENT: GameState changed to " + gameState);
        OnGameStateChangedEvent?.Invoke(gameState);
    }
}