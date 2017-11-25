using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesDoubleAiming : EnemyProperties
{
    public float zMovementSpeed;
    public float destructionMargin;
    public float amplitude;
    public float bulletForwardDistance;
    public float bulletBackDistance;
    public float bulletSpeedDecrease;
    public float xBulletSpeed;
    public float zBulletSpeed;
    public float fireRate;
    public float bulletAmplitude;
    public GameObject sidescrollBulletPrefab;
    public GameObject topdownBulletPrefab;
}
