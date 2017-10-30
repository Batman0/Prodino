using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesPlayer : Properties
{
    public float jumpForce;
    public float bulletSpeed;
    public float glideSpeed;
    public float upRotationAngle;
    public float downRotationAngle;
    public float fireRatio;
    public float respawnTimer;
    public float gravity;
    public float bulletDestructionMargin;
    public float maxArmsRotation;
    public float tailMeleeSpeed;
    public float biteATKSpeed;
    public bool biteCoolDownActive;
    public float biteCoolDown;
    public float topdownPlayerHeight;
    public GameObject bulletPrefab;
}
