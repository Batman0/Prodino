using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesDoubleAiming : EnemyProperties
{
    //public float xMovementSpeed;
    public float zMovementSpeed;
    public float destructionMargin;
    public float forwardDistance;
    public float backDistance;
    //public float waveLenght;
    //public float amplitude;
    //public float height;
    ////not used
    //public Transform[] rightTargets;
    ////not used
    //public Transform[] leftTargets;
    public float xBulletSpeed;
    public float zBulletSpeed;
    public float fireRate;
    public float bulletAmplitude;
    public GameObject sidescrollBulletPrefab;
    public GameObject topdownBulletPrefab;
}
