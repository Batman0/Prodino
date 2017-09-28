using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyProperties : ScriptableObject
{
    [Header("Movement")]
    [Header("Straight")]
    public float st_Speed;
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

    [Header("Fire")]
    [Header("Default")]
    //public float D_bulletSpeed;
    public float d_RatioOfFire;
    public GameObject straightEnemy;
    //public GameObject bullet;
    [Header("Laser")]
    public float l_Height;
    public float l_Width;
    public GameObject diagonalEnemy;
    //public GameObject l_Bullet;
    [Header("Trail")]
    public float t_Height;
    public float t_Width;
    public GameObject aimEnemy;
    //public GameObject t_Bullet;
    [Header("Bomb")]
    public float b_SpawnTime;
    public GameObject bombEnemy;
    //public GameObject b_Bullet;
    [Header("NoFire")]
    public GameObject noFireEnemy;
}
