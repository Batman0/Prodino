using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

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
    public EnemyProperties enemyProperties;
    [HideInInspector]
    public Vector3 originalPos;
    private float lifeTime;
    public Transform bulletSpawnpoint;
    private float timeToShoot;

    private void Start()
    {
        index = 0;
        Register.instance.numberOfEnemies++;
        originalPos = transform.position;
        timeToShoot = 0.0f;
        switch (GameManager.instance.currentGameMode)
        {
            case GameMode.SIDESCROLL:
                transform.position = new Vector3(transform.position.x, originalPos.y, 0);
                break;
            case GameMode.TOPDOWN:
                transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
                break;
        }
        if (movementType==MovementType.CIRCULAR)
        {
            lifeTime = enemyProperties.c_LifeTime;
        }
        transform.rotation = isRight ? transform.rotation : Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }

    private void Update()
    {
        ChangePerspective();
        Move();
        Shoot();
        Destroy();
    }
    
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
                        Shoots.straightShoot(bulletSpawnpoint, enemyProperties.bullet, transform);
                        timeToShoot = 0.0f;
                    }
                    break;
                case ShootType.LASER:
                    //Debug.Log("Laser");
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
        if (Register.instance.canStartEnemyTransition)
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
            Register.instance.translatedEnemies++;
            if (Register.instance.translatedEnemies == Register.instance.numberOfEnemies)
            {
                Register.instance.translatedEnemies = 0;
                Register.instance.canStartEnemyTransition = false;
            }
        }
        else if (Register.instance.canEndEnemyTransition)
        {
            switch (GameManager.instance.currentGameMode)
            {
                case GameMode.TOPDOWN:
                    if (movementType != MovementType.CIRCULAR)
                    {
                        if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z))
                        {
                            transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
                        }
                    }
                    else
                    {
                        if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y))
                        {
                            transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, enemyProperties.c_Radius * Mathf.Sin(Time.time * enemyProperties.c_Speed) + originalPos.y);
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
            Register.instance.translatedEnemies++;
            if (Register.instance.translatedEnemies == Register.instance.numberOfEnemies)
            {
                Register.instance.translatedEnemies = 0;
                Register.instance.canEndEnemyTransition = false;
            }
        }
    }

    public void Destroy()
    {
        if(EnemyLife())
        {
            Destroy(gameObject);
        }
        else if (toDestroy)
        {
            Destroy(gameObject);
        }
    }

    public bool EnemyLife()
    {
        return enemyLife <= 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            enemyLife--;
        }
    }
}
