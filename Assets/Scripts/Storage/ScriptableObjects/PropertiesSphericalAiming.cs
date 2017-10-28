using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesSphericalAiming : EnemyProperties
{
    //public float xMovementSpeed;
    public float zMovementSpeed;
    public float rotationSpeed;
    public float destructionMargin;
    public float forwardDistance;
    public float backDistance;
    public float rotationDeadZone;
    public float waveLenght;
    public float amplitude;
    public Transform[] rightTargets;
    public Transform[] leftTargets;
    public float fireRate;
    public float bulletSpeed;
    public float bulletDestructionMargin;
    public GameObject bulletPrefab;
}
