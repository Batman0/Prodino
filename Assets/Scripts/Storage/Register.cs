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
    public ScriptableObject[] enemyProperties;

    public Dictionary<string, ScriptableObject> enemyPropertiesDictionary = new Dictionary<string, ScriptableObject>();

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
    public float zMin;
    [HideInInspector]
    public float zMax;

    void Awake()
    {
        instance = this;
        EnemyPropertiesDictionary();
    }

    void EnemyPropertiesDictionary()
    {
        ScriptableObject currentProperty;
        for (int i = 0; i < enemyProperties.Length; i++)
        {
            currentProperty = enemyProperties[i];
            enemyPropertiesDictionary.Add(currentProperty.enemyName, enemyProperties[i]);
        }
    }

}
