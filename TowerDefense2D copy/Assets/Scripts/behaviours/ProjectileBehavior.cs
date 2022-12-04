using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ProjectileBehaviour script controls one flying tower projectile
/// </summary>
public class ProjectileBehavior : MonoBehaviour
{
    private float _speed;
    private float _projectileDamage;
    private EnemyBehaviour _target;
    private bool _isFlying = false;

    /// <summary>
    /// Set the target the projectile should chase after, including the flyingSpeed and damage
    /// </summary>
    /// <param name="target">target to chase after</param>
    /// <param name="projectileSpeed">speed the projectile is flying</param>
    /// <param name="projectileDamage">damage done when reaching target</param>
    public void Fire(EnemyBehaviour target, float projectileSpeed, float projectileDamage)
    {
        _target = target;
        _speed = projectileSpeed;
        _projectileDamage = projectileDamage;
        _isFlying = true;
    }

    /// <summary>
    /// Checks if the projectile 
    /// </summary>
    private void Update()
    {
        if (_target == null) // needed to stop the projectile from destroying itself right after object creation
        {
            if(_isFlying) Destroy(gameObject);
            return;
        }
        //TODO: Move the projectile towards the target
        //TODO: Check if the target is reached
        //TODO: if target is reached, call the Hit method in the targets EnemyBehaviour Component and Destroy the projectiles gameobject
    }
}
