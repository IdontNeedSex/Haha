using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Wave contains the enemies which should be spawned via the settings that should be applied. The wave does not create the enemies themself, only the information about them.
/// </summary>
public class Wave
{
    private int _waveCount;
    private int _initialWaveSize;
    private int _waveScaler;
    private EnemySettings _enemySettings;

    private readonly List<EnemySettings[]> _waves;
    
    /// <summary>
    /// The wave increases in size over time (see parameters). 
    /// </summary>
    /// <param name="waveCount"></param>
    /// <param name="initialWaveSize"></param>
    /// <param name="waveScaler"></param>
    /// <param name="enemySettings"></param>
    public Wave(WaveConfigurationOrder waveConfig)
    {
        _waves = CreateWaves(waveConfig);
    }

    public int CurWaveIndex { get; private set; } = 0;

    public List<EnemySettings[]> Waves => _waves;

    public EnemySettings[] NextWave()
    {
        var nextWave = CurWaveIndex >= Waves.Count ? Array.Empty<EnemySettings>() : Waves[CurWaveIndex];
        CurWaveIndex++;
        return nextWave;
    }

    private List<EnemySettings[]> CreateWaves(WaveConfigurationOrder waveConfigs)
    {
        var waves = new List<EnemySettings[]>();
        foreach (var waveConfiguration in waveConfigs.waveConfigs)
        {
            var enemyList = new List<EnemySettings>();
            foreach (var enemyMapping in waveConfiguration.enemyConfig)
            {
                for (var i = 0; i < enemyMapping.amountSpawned; i++)
                {
                    enemyList.Add(enemyMapping.enemySettings);
                }
            }
            waves.Add(enemyList.ToArray());
        }
        /*
        for (var waveIndex = 0; waveIndex < _waveCount; waveIndex++)
        {
            var size = _initialWaveSize + _waveCount * _waveScaler;
            var enemyList = new EnemySettings[size];
            for (var enemyIndex = 0; enemyIndex < size; enemyIndex++)
            {
                enemyList[enemyIndex] = _enemySettings;
            }

            waves.Add(enemyList);
        }
        */

        return waves;
    }

    public bool DidAllWavesSpawned()
    {
        return CurWaveIndex >= Waves.Count;
    }
}