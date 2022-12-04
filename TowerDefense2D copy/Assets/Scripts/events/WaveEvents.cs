using System.Collections.Generic;
using UnityEngine;

public class WaveEvents : MonoBehaviour
{
    public delegate void OnWavesGenerated(List<EnemySettings[]> enemySettings);

    public event OnWavesGenerated OnWavesGeneratedEvent;


    public delegate void OnWaveFinished(int curWaveIndex);


    public event OnWaveFinished OnWaveFinishedEvent;

    public void TriggerOnWavesGeneratedEvent(List<EnemySettings[]> enemySettings)
    {
        Debug.Log("EVENT: Wave generated...");
        OnWavesGeneratedEvent?.Invoke(enemySettings);
    }

    public void TriggerOnWavesFinishedEvent(int curWaveIndex)
    {
        Debug.Log("EVENT: Wave finished, current wave index=" + curWaveIndex);
        OnWaveFinishedEvent?.Invoke(curWaveIndex);
    }
}