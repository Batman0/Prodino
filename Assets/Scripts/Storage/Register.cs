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

    //[Header("Aim")]
    //public GameObject aimTransform;

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
    //[HideInInspector]
    //public Dictionary enemyProperties;

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
        //enemyProperties = new Properties[] { propertieseForwardShooter, propertiesForward, propertiesLaserDiagonal, propertiesSphericalAiming, propertiesBombDrop, propertiesTrail, propertiesDoubleAiming, propertiesCircular, propertiesSquare};
    }
}
