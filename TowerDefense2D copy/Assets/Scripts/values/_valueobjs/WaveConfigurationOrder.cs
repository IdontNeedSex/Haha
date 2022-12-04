using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/WaveConfigurationOrder")]
public class WaveConfigurationOrder : ScriptableObject
{
    public List<WaveConfiguration> waveConfigs;
}
