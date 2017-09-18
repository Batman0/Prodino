using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{

    private int index;
    [HideInInspector]
    public bool isRight;
    private float waitingTimer;
    [HideInInspector]
    public MovementType movementType;
    private bool toDestroy;
    public EnemyProperties enemyProperties;
    [HideInInspector]
    public Vector3 originalPos;
    private float lifeTime;

    private void Awake()
    {
        index = 0;
    }

    private void Start()
    {
        Register.instance.numberOfEnemies++;
        originalPos = transform.position;
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
            lifeTime = enemyProperties.C_lifeTime;
        }
        transform.rotation = isRight ? transform.rotation : Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }

    private void Update()
    {
        ChangePerspective();
        Move();
        Destroy();
    }

    public void Move()
    {
        switch (movementType)
        {
            case MovementType.STRAIGHT:
                Movements.StraightMove(transform, isRight, enemyProperties.St_speed, enemyProperties.St_destructionMargin, ref toDestroy);
                break;
            case MovementType.CIRCULAR:
                Movements.CircularMove(transform, enemyProperties.C_speed, isRight, enemyProperties.C_radius, originalPos, ref lifeTime, ref toDestroy);
                break;
            case MovementType.SQUARE:
                Movements.SquareMove(ref index, enemyProperties.Sq_speed, enemyProperties.Sq_waitingTime, ref waitingTimer, enemyProperties.Sq_targets, transform, ref toDestroy);
                break;
            default:
                Debug.Log("Nothing");
                break;
        }
    }

    public void ChangePerspective()
    {
        if (Register.instance.canStartEnemyTransition)
        {
            switch (GameManager.instance.currentGameMode)
            {
                case GameMode.SIDESCROLL:
                    //if (movementType != MovementType.CIRCULAR)
                    //{
                        if (transform.position != new Vector3(transform.position.x, transform.position.y, originalPos.z))
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);
                        }
                    //}
                    //else
                    //{
                    //    if (transform.position != new Vector3(transform.position.x, transform.position.y, transform.position.y))
                    //    {
                    //        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
                    //    }
                    //}
                    break;
                case GameMode.TOPDOWN:
                    //if (movementType != MovementType.CIRCULAR)
                    //{
                        if (transform.position != new Vector3(transform.position.x, originalPos.y, transform.position.z))
                        {
                            transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
                        }
                    //}
                    //else
                    //{
                    //    if (transform.position != new Vector3(transform.position.x, transform.position.z, transform.position.z))
                    //    {
                    //        transform.position = new Vector3(transform.position.x, transform.position.z, transform.position.z);
                    //    }
                    //}
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
                    //if (movementType != MovementType.CIRCULAR)
                    //{
                        if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z))
                        {
                            transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
                        }
                    //}
                    //else
                    //{
                    //    if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, transform.position.z))
                    //    {
                    //        transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, transform.position.z);
                    //    }
                    //}
                    break;
                case GameMode.SIDESCROLL:
                    //if (movementType != MovementType.CIRCULAR)
                    //{
                        if (transform.position != new Vector3(transform.position.x, originalPos.y, 0))
                        {
                            transform.position = new Vector3(transform.position.x, originalPos.y, 0);
                        }
                    //}
                    //else
                    //{
                    //    if (transform.position != new Vector3(transform.position.x, transform.position.y, 0))
                    //    {
                    //        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                    //    }
                    //}
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
        if (toDestroy)
        {
            Destroy(gameObject);
        }
    }
}
