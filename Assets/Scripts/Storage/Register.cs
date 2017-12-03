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
    public Enemy[] enemyScripts;

    public Dictionary<string, Enemy> enemyPropertiesDictionary = new Dictionary<string, Enemy>();

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
        Enemy currentEnemy;
        for (int i = 0; i < enemyScripts.Length; i++)
        {
            currentEnemy = enemyScripts[i];
            enemyPropertiesDictionary.Add(currentEnemy.enemyName, currentEnemy);
        }
    }

}
