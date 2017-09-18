using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyProperties : ScriptableObject
{
    [Header("Movement")]
    [Header("Straight")]
    public float St_speed;
    public float St_destructionMargin;
    [Header("Square")]
    public float Sq_speed;
    public float Sq_waitingTime;
    public Transform[] Sq_targets;
    [Header("Circular")]
    public float C_speed;
    public float C_radius;
    public float C_lifeTime;

    [Header("Fire")]
    [Header("Default")]
    public float D_bulletSpeed;
    public float D_ratioOfFire;
    [Header("Laser")]
    public float L_height;
    public float L_width;
    [Header("Trail")]
    public float T_height;
    public float T_width;
    [Header("Bomb")]
    public float B_spawnTime;
}
