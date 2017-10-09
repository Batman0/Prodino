using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Properties : ScriptableObject
{
    [Header("Enemy Movement")]
    [Header("ForwardShooter")]
    public float fs_Speed;
    public float fs_DestructionMargin;
    [Header("Forward")]
    public float f_Speed;
    public float f_DestructionMargin;
    [Header("LaserDiagonal")]
    public float ld_XMovementSpeed;
    public float ld_YMovementSpeed;
    public float ld_YMovementSpeedShot;
    public float ld_DestructionMargin;
    public Transform[] ld_RightTargets;
    public Transform[] ld_LeftTargets;
    [Header("SphericalAiming")]
    public float sa_XMovementSpeed;
    public float sa_ZMovementSpeed;
    public float sa_RotationSpeed;
    public float sa_DestructionMargin;
    public float sa_RotationDeadZone;
    public Transform[] sa_RightTargets;
    public Transform[] sa_LeftTargets;
    [Header("BombDrop")]
    public float bd_XMovementSpeed;
    public float bd_DestructionMargin;
    [Header("Trail")]
    public float t_XMovementSpeed;
    public float t_XReturnSpeed;
    public float t_RotationSpeed;
    public float t_MovementDuration;
    [Header("DoubleAiming")]
    public float da_XMovementSpeed;
    public float da_ZMovementSpeed;
    public float da_DestructionMargin;
    public Transform[] da_RightTargets;
    public Transform[] da_LeftTargets;
    [Header("Circular")]
    public float c_Speed;
    public float c_Radius;
    public float c_LifeTime;
    [Header("Square")]
    public float sq_Speed;
    public float sq_WaitingTime;
    public Transform[] sq_RightTargets;
    public Transform[] sq_LeftTargets;
   
    [Header("Enemy Fire")]
    [Header("ForwardShooter")]
    public float fs_FireRate;
    public float fs_BulletSpeed;
    [Header("LaserDiagonal")]
    public float l_LaserHeight;
    public float l_LaserDepth;
    public float l_Speed;
    public float l_WaitingTime;
    public float l_LoadingTime;
    public float l_ShootingTime;
    [Header("SphericalAiming")]
    public float sa_FireRate;
    public float sa_BulletSpeed;
    [Header("BombDrop")]
    public float bd_LoadingTime;
    public float bd_BombFallSpeed;
    public float bd_LifeTime;
    [Header("Trail")]
    public float t_FadeTime;
    [Header("DoubleAiming")]
    public float da_BulletSpeed;
    public float da_FireRate;
    

    [Header("Enemy Bullet")]
    public float e_Speed;
    public float e_DestructionMargin;

    [Header("Player Bullet")]
    public float p_Speed;
    public float p_DestructionMargin;

    [Header("Prefabs")]
    [Header("Enemies")]
    public GameObject forwardShooterPrefab;
    public GameObject forwardPrefab;
    public GameObject laserDiagonalPrefab;
    public GameObject sphericalAimingPrefab;
    public GameObject bombDropPrefab;
    public GameObject trailPrefab;
    public GameObject doubleAimingPrefab;
    [Header("Bullets")]
    public GameObject playerBulletPrefab;
    public GameObject enemyBulletPrefab;
    public GameObject enemyLaserPrefab;
    public GameObject bombPrefab;
    public GameObject trailBulletPrefab;
    public GameObject doubleAimingBulletPrefab;
    public GameObject sinusoideBulletPrefab;

}
