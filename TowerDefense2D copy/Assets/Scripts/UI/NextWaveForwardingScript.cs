using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveForwardingScript : MonoBehaviour
{
    //TODO: "startWaveButton" is currently not set, cause the button is not added to the editor yet, finished the todo in "StartWaveButtonTrigger" and pass the button reference to the component (located at the Canvas GameObject in the hierarchy) via the editor
    [SerializeField] private Button startWaveButton;
    
    [SerializeField] private string towerPhaseText = "Start Wave";
    [SerializeField] private string wavePhaseText = "Wave Running";

    private GameStateManager _gameStateManager;
    private GameStateEvents _gameStateEvents;

    private void OnEnable()
    {
        SetupEventReferences();
        SetupEventHooks();
    }

    void Start()
    {
        SetButtonContent(false);
    }

    private void SetupEventHooks()
    {
        _gameStateEvents.OnGameStateChangedEvent += HandleOnGameStateChangedEvent;
    }

    private void SetupEventReferences()
    {
        _gameStateManager = GameObject.Find(AppConstants.GAMEOBJECT_NAME_MANAGERS).GetComponent<GameStateManager>();
        _gameStateEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<GameStateEvents>();
    }

    private void HandleOnGameStateChangedEvent(GameState gamestate)
    {
        SetButtonContent(gamestate == GameState.WAVE);
    }

    private void SetButtonContent(bool isWavePhase)
    {
        //TODO: Uncomment this when the "StartWaveButtonTrigger" Todos are done 
        //startWaveButton.enabled = !isWavePhase;
        //startWaveButton.GetComponentInChildren<TMP_Text>().text = isWavePhase ? wavePhaseText : towerPhaseText;
    }

    public void StartWaveButtonTrigger()
    {
        //TODO: Place the script as component in the scene (recommended GameObject "Canvas") 
        //TODO: Add a UI button and call this (StartWaveButtonTrigger) function as onClick
        _gameStateManager.SetGameState(GameState.WAVE);
    }
}