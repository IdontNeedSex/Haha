using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower/TowerSettings")]
public class TowerSettings : ScriptableObject
{
    public int TowerCost = 40;
    public float ProjectileCooldown = 1f;
    public float ProjectileDamage = 1f;
    public float ProjectileSpeed = 1f;
    public float TowerRange = 10f;
    public Sprite TowerSprite;
}
