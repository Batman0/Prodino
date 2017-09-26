using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BulletProperties : ScriptableObject
{
    [Header("Enemy Bullet")]
    public float e_Speed;
    public float e_DestructionMargin;
    [Header("Player Bullet")]
    public float p_Speed;
    public float p_DestructionMargin;
}
