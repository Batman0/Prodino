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
    public bool bulletsCanRotate;

    public int numberOfTransitableObjects;

    public int translatedObjects;

    [Header("PlayerAim")]
    public Transform aimTransform;

    [Header("Scriptables")]
    public EnemyProperties enemyProperties;
    public BulletProperties bulletProperties;

    void Awake()
    {
        instance = this;
    }
}
