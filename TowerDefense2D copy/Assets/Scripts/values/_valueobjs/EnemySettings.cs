using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemySettings")]
public class EnemySettings : ScriptableObject
{
    public int killReward;
    public int enemyDamage;
    public float maxHealth;
    public float runningSpeed;
    public float spawnDelay;
    public Sprite sprite;
}