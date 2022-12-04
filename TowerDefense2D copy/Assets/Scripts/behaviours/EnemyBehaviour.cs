using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// The EnemyBehaviour script handles the movement and attack for one enemy instance.
/// </summary>
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float halfTileSize = 0.5f;
    [SerializeField] private Transform healthBar;
    
    private float _curHealth;
    private float _speed;
    private int _enemyIndex;
    private int _enemyDamage;
    private List<Vector3> _path;
    private bool _isInitialized = false;
    private float _maxHealth;
    private GameboardEvents _gameboardEvents;
    
    /// <summary>
    /// Used to initliaze object.
    /// <param name="path">The path, the enemy should walk after Initialization</param>
    /// <param name="enemyIndex">Identifier for this specific enemy instance, used for identification on callback after death</param>
    /// <param name="enemySettings">parameter collection with specs like moveSpeed etc.</param>
    public void Initialize(List<Vector2Int> path, int enemyIndex,
        EnemySettings enemySettings)
    {
        _gameboardEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<GameboardEvents>();
        _path = ConvertTilePositionToRealPosition(path);
        _enemyIndex = enemyIndex;
        _maxHealth = enemySettings.maxHealth;
        _curHealth = enemySettings.maxHealth;
        //TODO_done: read enemyDamage and speed from the enemySettings
        _enemyDamage = enemySettings.enemyDamage;
        //TODO_done: set the sprite (stored in enemySettings) to the SpriteRenderer to show the correct sprite
        GetComponent<SpriteRenderer>().sprite = enemySettings.sprite;
        _isInitialized = true;
    }

    /// <summary>
    /// Converts the index based tilemap to real positions, based on  the tile size
    /// index(0,2) => position(x = 0 + halfTileSize, y= 2 x tileSize + halfTileSize ,z = 0)
    /// The halfTileSize is added to move the position in the middle of the tile
    /// </summary>
    /// <param name="path">path with index based positions</param>
    /// <returns></returns>
    private List<Vector3> ConvertTilePositionToRealPosition(List<Vector2Int> path)
    {
        return path.Select(pos => new Vector3(pos.x + halfTileSize, pos.y + halfTileSize, 0)).ToList();
    }
    
    private void Update()
    {
        if (!_isInitialized) return;
        if (CheckIfWaypointsRemain()) return;
        MoveToCurrentOrNextWaypoint();
    }

    /// <summary>
    /// Checks if the waypoint was reached, if yes, removes it from the list
    /// </summary>
    private void MoveToCurrentOrNextWaypoint()
    {
        var reachedWaypoint = MoveToNextWaypoint(_path.First());

        if (reachedWaypoint) _path.RemoveAt(0);
    }

    /// <summary>
    /// Checks if all waypoints are reached, or other, if the finish is reached
    /// </summary>
    /// <returns></returns>
    private bool CheckIfWaypointsRemain()
    {
        if (_path.Count > 0) return false;
        
        _gameboardEvents.TriggerOnEnemyDeadEvent(_enemyIndex, true);
        Destroy(gameObject);
        return true;

    }

    /// <summary>
    /// Moves the enemy closer to the current waypoint (in a straight line)
    /// </summary>
    /// <param name="target">Waypoint we currently chase after</param>
    /// <returns>Returns true if the waypoint is reached, and false if the distance is still greater 0.1</returns>
    private bool MoveToNextWaypoint(Vector3 target)
    {
        //TODO: move the enemy towards the next waypoint, Hint: you need to translate your own transform here
        return false; //remove this line, only here to ensure compiling, check above what the method need to return
    }

    /// <summary>
    /// Is called when a tower hit the enemy
    /// </summary>
    /// <param name="projectileDamage">amount of damage done</param>
    public void Hit(float projectileDamage)
    {
        _curHealth -= projectileDamage;
        ScaleHealthBar();
        
        if (!(_curHealth <= 0)) return;
        
        _gameboardEvents.TriggerOnEnemyDeadEvent(_enemyIndex, false);
        Destroy(gameObject);
    }

    /// <summary>
    /// Visually scales the healthbar depending on the remaining health
    /// </summary>
    private void ScaleHealthBar()
    {
        var healthBarTransform = healthBar.transform;
        var curHealthBarScale = healthBarTransform.localScale;
        
        curHealthBarScale.x = _curHealth / _maxHealth;
        
        healthBarTransform.localScale = curHealthBarScale;
    }
}