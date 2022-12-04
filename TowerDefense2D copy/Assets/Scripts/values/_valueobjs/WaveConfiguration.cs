using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/WaveConfiguration")]
public class WaveConfiguration : ScriptableObject
{
    public List<EnemyMapping> enemyConfig;
}

[Serializable]
public struct EnemyMapping
{
    public int amountSpawned;
    public EnemySettings enemySettings;
}