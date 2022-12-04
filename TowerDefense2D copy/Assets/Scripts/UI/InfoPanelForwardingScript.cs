using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanelForwardingScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthpointsText;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _moneyText;

    private int _totalWaveCount = -1;
    private int _curWaveIndex = 0;

    private PlayerEvents _playerEvents;
    private WaveEvents _waveEvents;

    private void OnEnable()
    {
        SetupReferences();
        SetupEventHooks();
    }

    private void OnDisable()
    {
        RemoveEventHooks();
    }

    private void RemoveEventHooks()
    {
        // TODO: Uncomment these after implementing "PlayerEvents.cs" 
        _playerEvents.OnPlayerHealthChangedEvent -= HandleOnPlayerHealthChangedEvent;
        _playerEvents.OnPlayerMoneyChangedEvent -= HandleOnPlayerMoneyChangedEvent;
        _waveEvents.OnWavesGeneratedEvent -= HandleOnWavesGeneratedEvent;
        _waveEvents.OnWaveFinishedEvent -= HandleOnWaveFinishedEvent;
    }

    private void SetupReferences()
    {
        _playerEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<PlayerEvents>();
        _waveEvents = GameObject.Find(AppConstants.GAMEOBJECT_NAME_EVENTS).GetComponent<WaveEvents>();
    }

    private void SetupEventHooks()
    {
        // TODO: Uncomment these after implementing "PlayerEvents.cs" 
        _playerEvents.OnPlayerHealthChangedEvent += HandleOnPlayerHealthChangedEvent;
        _playerEvents.OnPlayerMoneyChangedEvent += HandleOnPlayerMoneyChangedEvent;
        _waveEvents.OnWavesGeneratedEvent += HandleOnWavesGeneratedEvent;
        _waveEvents.OnWaveFinishedEvent += HandleOnWaveFinishedEvent;
    }

    private void HandleOnPlayerHealthChangedEvent(int curHealth)
    {
        _healthpointsText.text = curHealth.ToString();
    }

    private void HandleOnWaveFinishedEvent(int curWaveindex)
    {
        _curWaveIndex++;
        UpdateWaveText(_waveText, _totalWaveCount, _curWaveIndex);
    }

    private void HandleOnWavesGeneratedEvent(List<EnemySettings[]> enemySettings)
    {
        _totalWaveCount = enemySettings.Count;
        UpdateWaveText(_waveText, _totalWaveCount, _curWaveIndex);
    }

    private void HandleOnPlayerMoneyChangedEvent(int curMoney)
    {
        _moneyText.text = curMoney + " $";
    }

    private void UpdateWaveText(TMP_Text waveText, int totalWaveCount, int curWave)
    {
        waveText.text = curWave + " / " + totalWaveCount;
    }
}