using System.Collections.Generic;
using UnityEngine;

public class GameboardEvents : MonoBehaviour
{

    public delegate void OnMapInitialized(Tile[,] tiles, Vector2Int startTile, Vector2Int endTile, List<Vector2Int> newPath);

    public event OnMapInitialized OnMapInitializedEvent;
    
    public delegate void OnTowerPlaced(TowerType projectileTower, int tileX, int tileY,List<Vector2Int> newPath);

    public event OnTowerPlaced OnTowerPlacedEvent;
    
    public delegate void OnWaveSpawned(EnemyBehaviour[] enemies);

    public event OnWaveSpawned OnWaveSpawnedEvent;

    public delegate void OnEnemyDead(int enemyIndex, bool reachedTarget);

    public event OnEnemyDead OnEnemyDeadEvent;

    public void TriggerOnWaveSpawnedEvent(EnemyBehaviour[] enemies)
    {
        OnWaveSpawnedEvent?.Invoke(enemies);
    }
    
    public void TriggerPlaceTowerEvent(TowerType projectileTower, int tileX, int tileY, List<Vector2Int> newPath)
    {
        Debug.Log("EVENT: Tower of type=" + projectileTower + " should be placed at x=" + tileX + " | y=" + tileY);
        OnTowerPlacedEvent?.Invoke(projectileTower, tileX, tileY, newPath);
    }

    public void TriggerOnMapInitializedEvent(Tile[,] tiles, Vector2Int startTile, Vector2Int endTile, List<Vector2Int> newPath)
    {
        Debug.Log("EVENT: Map initialized...");
        OnMapInitializedEvent?.Invoke(tiles, startTile, endTile, newPath);
    }
    
    public void TriggerOnEnemyDeadEvent(int enemyIndex, bool reachedTarget)
    {
        Debug.Log("EVENT: Enemy with index=" + enemyIndex + " is dead, reachedTarget=" + reachedTarget);
        OnEnemyDeadEvent?.Invoke(enemyIndex, reachedTarget);
    }
}