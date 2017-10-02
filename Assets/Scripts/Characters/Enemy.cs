using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //GURRA index di cosa?
    private int index;
    [HideInInspector]
    public bool isRight;
    private float waitingTimer;
    [HideInInspector]
    public MovementType movementType;
    [HideInInspector]
    public ShootType shootType;
    private bool toDestroy;
    public int enemyLife;
    private EnemyProperties enemyProperties;
    [HideInInspector]
    public Vector3 originalPos;
    private float lifeTime;
    public Transform bulletSpawnpoint;
    private float timeToShoot;
    //public GameObject myBullet;
    //public EnemyBullet myBulletScript;

    void Start()
    {
        enemyProperties = Register.instance.enemyProperties;
        index = 0;
        //Register.instance.numberOfTransitableObjects++;
        originalPos = transform.position;
        timeToShoot = 0.0f;
        //switch (GameManager.instance.currentGameMode)
        //{
        //    case GameMode.SIDESCROLL:
        //        transform.position = new Vector3(transform.position.x, originalPos.y, 0);
        //        break;
        //    case GameMode.TOPDOWN:
        //        transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
        //        break;
        //}
        if (movementType==MovementType.CIRCULAR)
        {
            lifeTime = enemyProperties.c_LifeTime;
        }

        if (!isRight)
        {
            transform.Rotate(Vector3.up, 180, Space.World);
        }

        //if (shootType != ShootType.DEFAULT)
        //{
        //    Destroy(myBullet);
        //}
        //else
        //{
        //    myBulletScript.speed = bulletProperties.e_Speed;
        //    myBulletScript.destructionMargin = bulletProperties.e_DestructionMargin;
        //    myBulletScript.originalPos = originalPos;
        //}
    }

    void Update()
    {
    //    ChangePerspective();
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

    //GURRA quindi se io ho 20 tipi di sparo ho nella classe enemy uno switch enorme con scritti dentro tutti i tipi di sparo? non penso sia il modo migliore
    //CARLO rivedremo questo 
    public void Shoot()
    {
        if(!GameManager.instance.transitionIsRunning)
        {
            switch(shootType)
            {
                case ShootType.DEFAULT:
                    if(timeToShoot < enemyProperties.d_RatioOfFire)
                    {
                        timeToShoot += Time.deltaTime;
                    }
                    else 
                    {
                        Shoots.straightShoot(Register.instance.enemyBullet, bulletSpawnpoint, transform);
                        timeToShoot = 0.0f;
                    }
                    break;
                case ShootType.LASER:
                    //Debug.Log("Laser");
                    break;
                case ShootType.TRAIL:
                    break;
                case ShootType.BOMB:
                    if(timeToShoot < enemyProperties.b_SpawnTime)
                    {
                        timeToShoot += Time.deltaTime;
                    }
                    else
                    {
                       Shoots.bombShoot(enemyProperties.b_Bullet, bulletSpawnpoint, bulletSpawnpoint);
                        timeToShoot = 0.0f;
                    }
                    break;
                case ShootType.NOFIRE:
                    break;
            }
        }
    }

    public void Move()
    {
        if (!GameManager.instance.transitionIsRunning)
        {
            switch (movementType)
            {
                case MovementType.STRAIGHT:
                    Movements.StraightMove(transform, isRight, enemyProperties.st_Speed, enemyProperties.st_DestructionMargin, ref toDestroy);
                    break;
                case MovementType.CIRCULAR:
                    Movements.CircularMove(transform, enemyProperties.c_Speed, isRight, enemyProperties.c_Radius, originalPos, ref lifeTime, ref toDestroy);
                    break;
                case MovementType.SQUARE:
                    if (isRight)
                    {
                        Movements.SquareMove(ref index, enemyProperties.sq_Speed, enemyProperties.sq_WaitingTime, ref waitingTimer, enemyProperties.sq_RightTargets, transform, ref toDestroy);
                    }
                    else
                    {
                        Movements.SquareMove(ref index, enemyProperties.sq_Speed, enemyProperties.sq_WaitingTime, ref waitingTimer, enemyProperties.sq_LeftTargets, transform, ref toDestroy);
                    }
                    break;
            }
        }
    }

    public void ChangePerspective()
    {
        if (Register.instance.canStartTransitions)
        {
            switch (GameManager.instance.currentGameMode)
            {
                case GameMode.SIDESCROLL:
                    if (movementType != MovementType.CIRCULAR)
                    {
                        if (transform.position != new Vector3(transform.position.x, transform.position.y, originalPos.z))
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);
                        }
                    }
                    else
                    {
                        if (transform.position != new Vector3(transform.position.x, transform.position.y, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y))
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y);
                        }
                    }
                    break;
                case GameMode.TOPDOWN:
                    if (movementType != MovementType.CIRCULAR)
                    {
                        if (transform.position != new Vector3(transform.position.x, originalPos.y, transform.position.z))
                        {
                            transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
                        }
                    }
                    else
                    {
                        if (transform.position != new Vector3(transform.position.x, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y, transform.position.z))
                        {
                            transform.position = new Vector3(transform.position.x, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y, transform.position.z);
                        }
                    }
                    break;
            }
            Register.instance.translatedObjects++;
            if (Register.instance.translatedObjects == Register.instance.numberOfTransitableObjects)
            {
                Register.instance.translatedObjects = 0;
                Register.instance.canStartTransitions = false;
            }
        }
        else if (Register.instance.canEndTransitions)
        {
            switch (GameManager.instance.currentGameMode)
            {
                case GameMode.TOPDOWN:
                    if (movementType != MovementType.CIRCULAR)
                    {
                        if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnpoointY, originalPos.z))
                        {
                            transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnpoointY, originalPos.z);
                        }
                    }
                    else
                    {
                        if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnpoointY, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y))
                        {
                            transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnpoointY, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y);
                        }
                    }
                    break;
                case GameMode.SIDESCROLL:
                    if (movementType != MovementType.CIRCULAR)
                    {
                        if (transform.position != new Vector3(transform.position.x, originalPos.y, 0))
                        {
                            transform.position = new Vector3(transform.position.x, originalPos.y, 0);
                        }
                    }
                    else
                    {
                        if (transform.position != new Vector3(transform.position.x, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y, 0))
                        {
                            transform.position = new Vector3(transform.position.x, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y, 0);
                        }
                    }
                    break;
            }
            Register.instance.translatedObjects++;
            if (Register.instance.translatedObjects == Register.instance.numberOfTransitableObjects)
            {
                Register.instance.translatedObjects = 0;
                Register.instance.canEndTransitions = false;
            }
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
