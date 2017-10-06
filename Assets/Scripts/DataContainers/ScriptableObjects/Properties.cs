using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Properties : ScriptableObject
{
    [Header("Enemy Movement")]
    [Header("Straight")]
    public float st_CanShoot_Speed;
    public float st_CannotShoot_Speed;
    public float st_DestructionMargin;
    [Header("Square")]
    public float sq_Speed;
    public float sq_WaitingTime;
    public Transform[] sq_RightTargets;
    public Transform[] sq_LeftTargets;
    [Header("Circular")]
    public float c_Speed;
    public float c_Radius;
    public float c_LifeTime;
    [Header("Diagonal")]
    public float diag_Speed;
    public float diag_WaitingTime;
    public Transform[] diag_RightTargets;
    public Transform[] diag_LeftTargets;
    [Header("Enemy Fire")]
    [Header("Default")]
    public float d_RatioOfFire;
    public GameObject defaultnemy;
    [Header("Laser")]
    public float l_Height;
    public float l_Width;
    public float l_Speed;
    public float l_WaitingTime;
    public float l_LoadingTime;
    public float l_ShootingTime;
    public GameObject laserEnemy;
    [Header("Trail")]
    public float t_Height;
    public float t_Width;
    public GameObject trailEnemy;
    [Header("Bomb")]
    public float b_SpawnTime;
    public GameObject bombEnemy;
    public GameObject b_Bullet;
    public float b_lifeTime;
    [Header("No Fire")]
    public GameObject noFireEnemy;

    [Header("Enemy Bullet")]
    public float e_Speed;
    public float e_DestructionMargin;

    [Header("Player Bullet")]
    public float p_Speed;
    public float p_DestructionMargin;

    [Header("Bullets")]
    public GameObject playerBulletPrefab;
    public GameObject enemyBulletPrefab;
    public GameObject enemyLaserPrefab;

}
