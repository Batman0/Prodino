using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int movementTargetIndex;
    [HideInInspector]
    public bool isRight;
    private float waitingTimer;
    [HideInInspector]
    public MovementType movementType;
    [HideInInspector]
    public ShootType shootType;
    private bool toDestroy;
    public int enemyLife;
    private Properties properties;
    [HideInInspector]
    public Vector3 originalPos;
    private float lifeTime;
    public Transform bulletSpawnpoint;
    private float timeToShoot;
    private bool canShoot;

    void Start()
    {
        properties = Register.instance.properties;
        movementTargetIndex = 0;
        //Register.instance.numberOfTransitableObjects++;
        originalPos = transform.position;
        timeToShoot = 0.0f;
        
        if (movementType==MovementType.CIRCULAR)
        {
            lifeTime = properties.c_LifeTime;
        }

        if (!isRight)
        {
            transform.Rotate(Vector3.up, 180, Space.World);
        }

      
    }

    void Update()
    {
        Move();
        Shoot();
        Destroy();
    }

    void OnTriggerEnter(Collider other)
    {
        Transform parentTr = other.transform.parent;
        if (parentTr.tag == "PlayerBullet")
        {
            enemyLife--;
            Destroy(parentTr.gameObject);
        }
    }

    void OnDestroy()
    {
        Register.instance.numberOfTransitableObjects--;
    }

    public void Shoot()
    {
        if(!GameManager.instance.transitionIsRunning)
        {
            Shoots.Shoot(shootType, properties, ref timeToShoot, ref canShoot, bulletSpawnpoint, transform);
        }
    }

    public void Move()
    {
        if (!GameManager.instance.transitionIsRunning)
        {
            Movements.Move(movementType, transform, isRight, properties, originalPos, ref movementTargetIndex, ref lifeTime, ref waitingTimer, ref toDestroy);
        }
    }

    public void Destroy()
    {
        if(CheckEnemyLife() || toDestroy)
        {
            Destroy(gameObject);
        }
    }

    //GURRA nome dubbio su questo metodo, non fa quello che mi aspettavo quando l'ho letto
    //Hai ragione mi ero scordato di mettere CheckEnemyLife cambio
    public bool CheckEnemyLife()
    {
        return enemyLife <= 0;
    }
}
