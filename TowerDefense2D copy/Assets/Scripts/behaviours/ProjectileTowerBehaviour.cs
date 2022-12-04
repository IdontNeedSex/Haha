using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ProjectileTowerBehaviour script controls one tower placed on the gameboard
/// </summary>
public class ProjectileTowerBehaviour : MonoBehaviour
{
    
    [SerializeField] private Transform _projectilePrefab;
    
    private float _curCooldown = 0;

    private EnemyBehaviour _curTarget = null;
    private float _projectileCooldown;
    private float _projectileSpeed;
    private float _projectileDamage;
    private float _towerRange;

    private bool _isInitialized = false;
    private GameboardManager _gameBoardManagerRef;
    private GameStateManager _gameStateManagerRef;

    /// <summary>
    /// Grabs references to manager scripts
    /// </summary>
    private void Start()
    {
        _gameBoardManagerRef = GameObject.Find("_managers").GetComponent<GameboardManager>();
        _gameStateManagerRef = GameObject.Find("_managers").GetComponent<GameStateManager>();
    }

    /// <summary>
    /// Initializes based on the provided towerSettings object
    /// </summary>
    /// <param name="towerSettings">setting object, can be instantiated via editor create menu (right click asset folder)</param>
    public void Initialize(TowerSettings towerSettings)
    {
        _projectileCooldown = towerSettings.ProjectileCooldown;
        _projectileSpeed = towerSettings.ProjectileSpeed;
        _projectileDamage = towerSettings.ProjectileDamage;
        _towerRange = towerSettings.TowerRange;
        GetComponent<SpriteRenderer>().sprite = towerSettings.TowerSprite;
        _isInitialized = true;
    }

    /// <summary>
    /// Deactives tower if not initilaized or not in WAVE gamestate
    /// Otherwise scans for target and shoots perodically to a target in range
    /// </summary>
    void Update()
    {
        if (!_isInitialized || _gameStateManagerRef.GameState != GameState.WAVE) return;
        
        _curCooldown -= Time.deltaTime;
        if (_curTarget == null)
        {
            var nextTarget = SearchTarget(_gameBoardManagerRef.CurEnemies.FindAll(enemy => enemy != null));
            if (nextTarget == null) return;
            _curTarget = nextTarget;
        }

        if (!(_curCooldown <= 0)) return;
        
        ShootProjectile(_curTarget);
        _curCooldown = _projectileCooldown;
    }

    /// <summary>
    /// Instantiotes the projectile Prefab and fires it at the respective target
    /// </summary>
    /// <param name="target">target the projectile chases</param>
    private void ShootProjectile(EnemyBehaviour target)
    {
        //TODO: Instantiate a new projectile and fire at the target by calling the "Fire" method on the projectiles "ProjectileBehavior" script
    }

    /// <summary>
    /// Checks if a target is in range
    /// </summary>
    /// <param name="possibleTargets">list of targets, obtained from the wave</param>
    /// <returns></returns>
    private EnemyBehaviour SearchTarget(List<EnemyBehaviour> possibleTargets)
    {
        foreach (var possibleTarget in possibleTargets)
        {
            if (Vector3.Distance(possibleTarget.transform.position, transform.position) < _towerRange 
                && possibleTarget != null) // gameobjects null check is True if they are destroyed
            {
                return possibleTarget;
            }
        }

        return null;
    }
}
