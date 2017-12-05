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

    //CommonProperties
    private const string enemyLayerName = "Enemy";
    private const string enemyBulletTag = "EnemyBullet";
    private const string playerLayerName = "Player";
    private int enemyLayer;
    private int playerLayer;
    public int EnemyLayer
    {
        get { return enemyLayer; }
    }
    public int PlayerLayer
    {
        get { return playerLayer; }
    }
    public string EnemyBulletTag
    {
        get { return enemyBulletTag; }
    }

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
        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);
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
