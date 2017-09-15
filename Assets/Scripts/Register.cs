using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    public static Register instance;
    [Header("Enemies")]
    public GameObject[] enemies;
    /// <summary>
    /// New enemy prefab.
    /// </summary>
    public GameObject enemyPrefab;
    [HideInInspector]
    public bool canStartEnemyTransition = false;
    [HideInInspector]
    public bool canEndEnemyTransition = false;
    [HideInInspector]
    public int numberOfEnemies;
    [HideInInspector]
    public int translatedEnemies;

    void Awake()
    {
        instance = this;
    }

}
