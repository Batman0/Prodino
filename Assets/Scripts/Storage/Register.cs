using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    public static Register instance;
    [HideInInspector]
    public bool canStartTransitions = false;
    [HideInInspector]
    public bool canEndTransitions = false;
    [HideInInspector]
    public bool bulletsCanRotate;
    [HideInInspector]
    public int numberOfTransitableObjects;
    [HideInInspector]
    public int translatedObjects;

    [HideInInspector]
    public PlayerController player;

    [HideInInspector]
    public Enemy enemyScript;

    [Header("Scriptables")]
    public PropertiesPlayer propertiesPlayer;
    public PropertiesForwardShooter propertiesForwardShooter;
    public PropertiesForward propertiesForward;
    public PropertiesLaserDiagonal propertiesLaserDiagonal;
    public PropertiesSphericalAiming propertiesSphericalAiming;
    public PropertiesBombDrop propertiesBombDrop;
    public PropertiesTrail propertiesTrail;
    public PropertiesDoubleAiming propertiesDoubleAiming;
    public PropertiesCircular propertiesCircular;
    public PropertiesSquare propertiesSquare;

    public Dictionary<string, EnemyProperties> enemyProperties;

    [Header("Bounds")]
    [HideInInspector]
    public float xMin;
    [HideInInspector]
    public float xMax;
    [HideInInspector]
    public float yMin;
    [HideInInspector]
    public float yMax;
    [HideInInspector]
    public float? zMin = null;
    [HideInInspector]
    public float? zMax = null;

    void Awake()
    {
        instance = this;
        enemyProperties = new Dictionary<string, EnemyProperties>();
        enemyProperties.Add("ForwardShooter", propertiesForwardShooter);
        enemyProperties.Add("Forward", propertiesForward);
        enemyProperties.Add("LaserDiagonal", propertiesLaserDiagonal);
        enemyProperties.Add("SphericalAiming", propertiesSphericalAiming);
        enemyProperties.Add("BombDrop", propertiesBombDrop);
        enemyProperties.Add("Trail", propertiesTrail);
        enemyProperties.Add("DoubleAiming", propertiesDoubleAiming);
    }
}
