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

    [Header("Player")]
    public PlayerController player;
    //public Transform playerStartPos;

    [Header("Aim")]
    public Transform aimTransform;

    [Header("Scriptables")]
    public EnemyProperties enemyProperties;
    public BulletProperties bulletProperties;

    [Header("Bullets")]
    public GameObject playerBullet;
    public GameObject enemyBullet;

    void Awake()
    {
        instance = this;
    }
}
