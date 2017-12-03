using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesDoubleAiming : ScriptableObject
{
    public float zMovementSpeed;
    public float destructionMargin;
    public float amplitude;
    public float bulletForwardDistance;
    public float bulletBackDistance;
    public float xBulletSpeed;
    public float sinusoideDuration;
    public float fireRate;
    [Range(0.94f, 10f)]
    public float easingValue;
    public GameObject sidescrollBulletPrefab;
    public GameObject topdownBulletPrefab;
}
