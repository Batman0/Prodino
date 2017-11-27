using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesDoubleAiming : EnemyProperties
{
    //public float xMovementSpeed;
    public float zMovementSpeed;
    public float destructionMargin;
    public float amplitude;
    //public float backDistance;
    public float bulletForwardDistance;
    public float bulletBackDistance;
    public float bulletSpeedDecrease;
    //public float waveLenght;
    //public float amplitude;
    //public float height;
    ////not used
    //public Transform[] rightTargets;
    ////not used
    //public Transform[] leftTargets;
    public float xBulletSpeed;
    public float sinusoideDuration;
    public float fireRate;
    [Range(0.94f, 10f)]
    public float easingValue;
    public GameObject sidescrollBulletPrefab;
    public GameObject topdownBulletPrefab;
}
