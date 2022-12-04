using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameboardManager : MonoBehaviour
{
    /// <summary>
    /// Prefab for the tower entity
    /// </summary>
    [Header("Tower Settings")] [SerializeField]
    private ProjectileTowerBehaviour _projectileTowerPrefab;
    /// <summary>
    /// Tower settings, for each defined tower (defined in TowerType enum, in the enum order)
    /// </summary>
    [SerializeField] private TowerSettings[] _towerSettings;

    /// <summary>
    /// Prefab for the enemy entity
    /// </summary>
    [Header("Enemy Settings")] [SerializeField]
    private EnemyBehaviour _enemyPrefab;

    /// <summary>
    /// WaveConfiguration for spawning waves
    /// </summary>
    [SerializeField] private WaveConfigurationOrder _waveConfig;

    /// <summary>
    /// Gameboard size, start and endtile position
    /// </summary>
    [Header("General GameBoard Settings")] 
    [SerializeField] private Vector2Int _startTilePosition = new Vector2Int(0,0);
    [SerializeField] private Vector2Int _endTilePosition = new Vector2Int(4,4); //TODO: Note: the start position and the end position are later read from the json file.
    /// <summary>
    /// Encarddata is a ScriptableObject to store and load data between scenes (Warning! It is persistent)
    /// </summary>
    [SerializeField] private EndcardData _endcardData;

    /// <summary>
    /// Tilemap which visualizes the gameboard
    /// </summary>
    [Header("Tilemap Painting Settings")] [SerializeField]
    private Tilemap _tilemap;

    /// <summary>
    /// Tilebase (visuals) for non blocked tiles
    /// </summary>
    [SerializeField] private TileBase _tileBaseNotBlocked;
    /// <summary>
    /// Tilebase (visuals) for blocked tiles
    /// </summary>
    [SerializeField] private TileBase _tileBaseBlocked;

    /// <summary>
    /// List of all currently running enemies
    /// </summary>
    public List<EnemyBehaviour> CurEnemies { get; private set; }
    
    private Player _player;
    private Wave _wave;

    private GameStateEvents _gameStateEvents;
    private GameboardEvents _gameboardEvents;
    private PlayerEvents _playerEvents;
    private WaveEvents _waveEvents;
    private ToolEvents _toolEvents;
    private GameStateManager _gameStateManager;
    
    private Pathfinding2D _pathfinding2D;

    private bool _didAllEnemiesSpawned = false;
    private Gameboard GameBoard { get; set; }

    /// <summary>
    /// Collects all needed references to other script instances and registers eventhandlers
    /// </summary>
    private void OnEnable()
    {
        SetupEventAndManagerReferences();
        RegisterEventHandler();
    }

    /// <summary>
    /// If script gets inactive, unregisters eventhandlers
    /// </summary>
    private void OnDisable()
    {
        UnRegisterEventHandler();
    }

    /// <summary>
    /// Generates all entites in the game and generates waves and visual tiles based on the board setting
    /// </summary>
    private void Start()
    {
        SetupPlayer();
        GenerateWaveSetup(_waveConfig);
        GenerateGameBoardAndTilemap();
        InitializePathfinding(GameBoard.Tiles, _startTilePosition, _endTilePosition);

        _gameboardEvents.TriggerOnMapInitializedEvent(GameBoard.Tiles, _startTilePosition, _endTilePosition, GetPath());
    }
    
    /// <summary>
    /// Called on enemy is dead event.
    /// Disables the respective enemy and checks if all enemies in the wave are dead or reached the target
    /// </summary>
    /// <param name="enemyIndex">identifier for specific enemy instance</param>
    /// <param name="reachedEndTile">false=enemy died, true=enemy reached finish and so does player damage</param>
    private void HandleOnEnemyDeadEvent(int enemyIndex, bool reachedEndTile)
    {
        DisableEnemy(enemyIndex);
        UpdatePlayerOnEnemyCallback(enemyIndex, reachedEndTile);
        CheckIfWaveFinished();
    }

    /// <summary>
    /// marks enemy as dead
    /// </summary>
    /// <param name="enemyIndex">identifier for specific enemy instance</param>
    private void DisableEnemy(int enemyIndex)
    {
        CurEnemies[enemyIndex] = null;
    }

    /// <summary>
    /// Checks for alle enemies if they are all dead and if all spawned
    /// If all waves finished it ends the game
    /// </summary>
    private void CheckIfWaveFinished()
    {
        var doAllEnemiesDied = CurEnemies.TrueForAll(e => e == null);
        if (!doAllEnemiesDied || !_didAllEnemiesSpawned) return;
        
        _waveEvents.TriggerOnWavesFinishedEvent(_wave.CurWaveIndex);
        
        if (_wave.DidAllWavesSpawned())
        {
            FinishGame(true);
            return;
        }
        _gameStateManager.SetGameState(GameState.TOWER);
    }

    /// <summary>
    /// Finished the game and moves to the EndScene
    /// </summary>
    /// <param name="isWon">passes if it is won on to the a data object</param>
    private void FinishGame(bool isWon)
    {
        _endcardData.isWon = isWon;
        //TODO: add one or more metric to the "_endcardData" variable and set the value here, e.g. track the time the game took, the amount of enemies killed,e.g., this todo belongs to the todo in the "EndCardData" class
        
        //TODO: Create a new Scene in the "Assets/Scene" folder and use the SceneManager (from Unity) to load the scene here
        
        //TODO: use the already created instance ("Assets/Scripts/Values/EndcardData") and show the data in the new scene. It is up to you how you visualize/build the content of the end scene
    }

    /// <summary>
    /// Checks after enemy termination if the player should take damage and how much
    /// If the player is dead, game is finished
    /// </summary>
    /// <param name="enemyIndex">identifier for specific enemy instance</param>
    /// <param name="reachedEndTile">false=enemy died, true=enemy reached finish and so does player damage</param>
    private void UpdatePlayerOnEnemyCallback(int enemyIndex, bool reachedEndTile)
    {
        var enemySettings = _wave.Waves[_wave.CurWaveIndex - 1][enemyIndex];
        if (reachedEndTile)
        {
            _player.Hit(enemySettings.enemyDamage);
            if (_player.IsPlayerDead())
            {
                FinishGame(false);
            }
            else
            {
                _playerEvents.TriggerOnPlayerHealthChanged(_player.HealthPoints);
            }
        }
        else
        {
            _player.AddMoney(enemySettings.killReward);
            _playerEvents.TriggerOnPlayerMoneyChangedEvent(_player.Money);
        }
    }

    private void InitializePathfinding(Tile[,] tiles, Vector2Int startTile, Vector2Int endTile)
    {
        _pathfinding2D = new Pathfinding2D(new Grid2D(tiles, startTile, endTile));
    }
    
    private void GenerateGameBoardAndTilemap()
    {
        GameBoard = new Gameboard();
        GameboardHelper.GenerateTilemap(GameBoard.Tiles, _tilemap, _tileBaseNotBlocked, _tileBaseBlocked);
    }

    /// <summary>
    /// Generates next enemy wave
    /// </summary>
    /// <param name="waveCount">amount of waves</param>
    /// <param name="initialWaveSize">initial amount of enemies per wave</param>
    /// <param name="waveScaler">how many more enemies after per wave (not absolut, scalar)</param>
    private void GenerateWaveSetup(WaveConfigurationOrder waveConfigurationOrder)
    {
        _wave = new Wave(waveConfigurationOrder);
        _waveEvents.TriggerOnWavesGeneratedEvent(_wave.Waves);
    }

    /// <summary>
    /// Creates the player object and triggers initial money and health changes to update the UI
    /// </summary>
    private void SetupPlayer()
    {
        _player = new Player(100, 100);
        _playerEvents.TriggerOnPlayerMoneyChangedEvent(_player.Money);
        _playerEvents.TriggerOnPlayerHealthChanged(_player.HealthPoints);
    }

    /// <summary>
    /// Registers to all needed events
    /// </summary>
    private void RegisterEventHandler()
    {
        _toolEvents.OnToolActionTriggered += HandleOnToolActionTriggered;
        _gameStateEvents.OnGameStateChangedEvent += HandleOnGameStateChangedEvent;
        _gameboardEvents.OnEnemyDeadEvent += HandleOnEnemyDeadEvent;
    }
    
    /// <summary>
    /// Unregisters from all events
    /// </summary>
    private void UnRegisterEventHandler()
    {
        _toolEvents.OnToolActionTriggered -= HandleOnToolActionTriggered;
        _gameStateEvents.OnGameStateChangedEvent -= HandleOnGameStateChangedEvent;
        _gameboardEvents.OnEnemyDeadEvent -= HandleOnEnemyDeadEvent;
    }

    /// <summary>
    /// Grabs all references to needed gameobjects from scene 
    /// </summary>
    private void SetupEventAndManagerReferences()
    {
        _toolEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<ToolEvents>();
        _gameboardEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<GameboardEvents>();
        _gameStateEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<GameStateEvents>();
        _playerEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<PlayerEvents>();
        _waveEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<WaveEvents>();
        _gameStateManager = GameObject.Find(AppConstants.GAMEOBJECT_NAME_MANAGERS).GetComponent<GameStateManager>();
    }

    /// <summary>
    /// Starts coroutine to spawn enemies
    /// Coroutines are a bit tricky at first. While basically all frontend frameworks across any programming language is single threaded, is a solution for parallel tasks neeeded.
    /// Coroutines are the Unity solution to this problem. Details are explained in the "SpawnEnemy" method summary.
    /// </summary>
    private void StartNextWave()
    {
        var nextEnemySettings = _wave.NextWave();
        CurEnemies = new List<EnemyBehaviour>();
        StartCoroutine(SpawnEnemies(nextEnemySettings));
    }

    /// <summary>
    /// Spawns an enemy instance at specific location and afterwards waits a couple of seconds (spawnDelay).
    /// Coroutines: A method which should be run in pseudo parallism need to return an "IEnumerator".
    /// Unity internally divides the function in execution blocks, split at the "yield return".
    /// Each block is then executedion the mainthread sequentially with any other operation Unity need to do.
    /// So while the code block is running, any other Unity operation. That´s why only small amount of computations should be done between two yield returns.
    /// If you have a long running task which do not need to interact with the Unity API (e.g. Terrain creation algorithms),
    /// then you should create a "real" thread with the respective sychronization to transfer the created data to the main thread. 
    /// </summary>
    /// <param name="enemySettings">enemy settings which describe the wave</param>
    /// <returns>see coroutine explanation above</returns>
    private IEnumerator SpawnEnemies(EnemySettings[] enemySettings)
    {
        _didAllEnemiesSpawned = false;
        for (var enemyIndex = 0; enemyIndex < enemySettings.Length; enemyIndex++)
        {
            var enemy = Instantiate(_enemyPrefab, new Vector3(_startTilePosition.x + 0.5f, _startTilePosition.y + 0.5f, 0),
                Quaternion.identity);
            var enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
            enemyBehaviour.Initialize(GetPath(), enemyIndex, enemySettings[enemyIndex]);
            CurEnemies.Add(enemyBehaviour);
            if (enemyIndex == enemySettings.Length - 1)
            {
                _didAllEnemiesSpawned = true;
            }
            yield return new WaitForSeconds(enemySettings[enemyIndex].spawnDelay);
        }
    }

    /// <summary>
    /// Starts next wave when the Gamestate switch to GameState.WAVE
    /// </summary>
    /// <param name="gamestate">Gamestate the game just switched to</param>
    private void HandleOnGameStateChangedEvent(GameState gamestate)
    {
        if (gamestate == GameState.WAVE) StartNextWave();
    }

    /// <summary>
    /// Is triggered when a Tool executes an action (normally tower is placed)
    /// Retrieve the towertype based on the Tooltype and checks if user has enough money to place it
    /// If enough money is present, the tower is placed at the respective position
    /// </summary>
    /// <param name="tooltype">ToolType which was used</param>
    /// <param name="mousepos">position where the user clicked in world space</param>
    private void HandleOnToolActionTriggered(ToolType tooltype, Vector3Int mousepos)
    {
        var towerType = GetTowerTypeFromToolType(tooltype);
        

        //TODO: read towercost from _towerSettings for the respective towertype
        
        int towercost = 1;

        switch (towerType)
        {
            case TowerType.FAST_PROJECTILE_TOWER: towercost = 20;
                break;
            case TowerType.SLOW_PROJECTILE_TOWER: towercost = 30;
                break;
        }
        
        
        //TODO_done: check if the player has enough money
        if (_player.Money >= towercost)
        {
            if (PlaceTowerIfPossible(towerType, mousepos.x, mousepos.y))
            {
                SubtractTowerCostFromPlayerMoney(_player, towercost); //TODO_done: insert the towercost from above instead the -1
            }
        }
    }

    /// <summary>
    /// Converts the Tooltype to respective tower
    /// </summary>
    /// <param name="tooltype">see ToolType enum</param>
    /// <returns>Respective TowerType</returns>
    /// <exception cref="ArgumentOutOfRangeException">Is raised when a tool has no tower assigned to it</exception>
    private static TowerType GetTowerTypeFromToolType(ToolType tooltype)
    {
        return tooltype switch
        {
            ToolType.PLACE_FAST_PROJECTILE_TOWER => TowerType.FAST_PROJECTILE_TOWER,
            ToolType.PLACE_SLOW_PROJECTILE_TOWER => TowerType.SLOW_PROJECTILE_TOWER,
            _ => throw new ArgumentOutOfRangeException(nameof(tooltype), tooltype, null)
        };
    }

    /*
    private static bool HasPlayerEnoughMoney(Player player, int towerCost)
    {
        return player.Money >= towerCost;
    }
    */

    
    private void SubtractTowerCostFromPlayerMoney(Player player, int towerCost)
    {
        //TODO_done: subtract money from player
        player.Money -= towerCost;
        //TODO_done: trigger the "TriggerOnPlayerMoneyChangedEvent" event
        _playerEvents.TriggerOnPlayerMoneyChangedEvent(player.Money);
        
    }
    

    /// <summary>
    /// Checks if a) the position is in the board range, b) the tile is ot already occupied and c) if the board is still passable
    /// </summary>
    /// <param name="towerType">Tower which should be placed</param>
    /// <param name="x">x position (as tile index)</param>
    /// <param name="y">y position (as tile index)</param>
    /// <returns>Returns if tower placing was successfull</returns>
    private bool PlaceTowerIfPossible(TowerType towerType, int x, int y)
    {
         //TODO_done: Use the "GameBoard" class to check if the tower spot is occupied and/or blocked, they are already methods available for use in the GameBoard class 
         //TODO_done: check that the field at x/y is neither the _startTile nor the _endTile
         if (GameBoard.IsTileBlocked(x, y))
         {
             return false;
         }
         if (GameBoard.IsTowerSpotOccupied(x, y))
         {
             return false;
         }
         
         
         // Check StartTile
         if (GameBoard.Tiles[x, y] == GameBoard.Tiles[_startTilePosition.x,_startTilePosition.y])
         {
             return false;
         }
         
         // Check EndTile
         if (GameBoard.Tiles[x, y] == GameBoard.Tiles[_endTilePosition.x,_endTilePosition.y])
         {
             return false;
         }

         //Note: watch out that you implement the pathfinding, otherwise the path will be an empty list all the time
        if (!CheckIfBoardStillPassable(x, y, out var path)) return false;

        //TODO_done: uncomment this line after finishing the two todos above
        SetupAndPlaceTowerObject(towerType, x, y, path);
        
        return true; // above, return false if the tower could not be placed
    }

    /// <summary>
    /// Checks if still a valid path is present from start to finish
    /// </summary>
    /// <param name="x">newly occupied tiled position x index</param>
    /// <param name="y">newly occupied tiled position y index</param>
    /// <param name="path">Path as out parameter (explanation: https://www.educative.io/answers/what-is-the-out-parameter-in-c-sharp )</param>
    /// <returns>If board is still passable</returns>
    private bool CheckIfBoardStillPassable(int x, int y, out List<Vector2Int> path)
    {
        _pathfinding2D.SetObstacle(true, x, y);
        path = GetPath();

        if (path.Count != 0) return true;

        _pathfinding2D.SetObstacle(false, x, y);
        return false;
    }

    /// <summary>
    /// Creates the tower object, and moves it to the respective world position
    /// </summary>
    /// <param name="towerType">Tower type which should be created</param>
    /// <param name="x">x index position</param>
    /// <param name="y">y index position</param>
    /// <param name="path">New path after placing tower, used for highlighting and enemy pathing</param>
    private void SetupAndPlaceTowerObject(TowerType towerType, int x, int y, List<Vector2Int> path)
    {
        var tower = Instantiate(_projectileTowerPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
        
        var towerScript = tower.GetComponent<ProjectileTowerBehaviour>();
        towerScript.Initialize(_towerSettings[(int)towerType]);
        GameBoard.PlaceTower(x, y);

        _gameboardEvents.TriggerPlaceTowerEvent(towerType, x, y, path);
    }

    /// <summary>
    /// Run the pathfinding
    /// </summary>
    /// <returns>List of index positions</returns>
    private List<Vector2Int> GetPath()
    {
        return _pathfinding2D.FindPath().Select(n => new Vector2Int(n.GridX, n.GridY)).ToList();
    }
}