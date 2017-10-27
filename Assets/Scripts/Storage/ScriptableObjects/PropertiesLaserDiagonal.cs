using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesLaserDiagonal : EnemyProperties
{
    //public float xMovementSpeed;
    public float yMovementSpeed;
    public float yMovementSpeedShooting;
    public float destructionMargin;
    public float waveLenght;
    public float amplitude;
    public float height;
    public Transform[] rightTargets;
    public Transform[] leftTargets;
    public float laserHeight;
    public float laserWidth;
    //public float speed;
    public float waitingTime;
    public float loadingTime;
    public float shootingTime;
    public GameObject laserPrefab;
}
