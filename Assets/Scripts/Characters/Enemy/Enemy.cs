using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyMovement();
public delegate void MyShot();

public class Enemy : MonoBehaviour
{
    [Header("Objects")]
    private Enemy enemy;
    private GameManager gameManager;
    private EnemyBehaviour myBehaviourClass;
    private PoolManager.PoolBullet bulletPool;
    public ParticleSystemManager explosionParticleManager;
    public Transform bulletSpawnpoint;
    public Transform bulletSpawnpointSecond;
    public Transform shooterTransform;
    public Collider sideCollider;
    public Collider topCollider;

    [Header("Statistics")]
    private int enemyLives;
    private float enemyDeactivationDelay = 0.5f;
    private bool isDying = false;
    [HideInInspector]
    public bool isRight;
    public BehaviourType behaviourType;
    public MyMovement myMovement;
    public MyShot myShot;

    void OnEnable()
    {
        enemy = this;
        gameManager = GameManager.instance;
        enemyLives = Register.instance.enemyProperties[behaviourType.ToString()].lives;
        if (!isRight)
        {
            transform.Rotate(Vector3.up, 180, Space.World);
        }

        if (gameManager.currentGameMode == GameMode.SIDESCROLL)
        {
            if (!sideCollider.enabled || topCollider.enabled)
            {
                topCollider.enabled = false;
                sideCollider.enabled = true;
            }
        }
        else
        {
            if (!topCollider.enabled || sideCollider.enabled)
            {
                sideCollider.enabled = false;
                topCollider.enabled = true;
            }
        }
    }

    void Start()
    {
        InitBehaviour();
        myBehaviourClass.Init(enemy);
        myMovement += myBehaviourClass.Move;
        myShot += myBehaviourClass.Shoot;
    }

    void Update()
    {
        ChangePerspective();
        Move();
        Shoot();
        if (CheckEnemyDead())
        {
            Deactivate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBullet")
        {
            enemyLives--;
            other.gameObject.SetActive(false);
        }
    }

    public void InitBehaviour()
    {
        switch (behaviourType)
        {
            case BehaviourType.ForwardShooter:
                myBehaviourClass = new ForwardShooterBehaviour();
                break;
            case BehaviourType.Forward:
                myBehaviourClass = new ForwardBehaviour();
                break;
            case BehaviourType.LaserDiagonal:
                myBehaviourClass = new LaserDiagonalBehaviour();
                break;
            case BehaviourType.SphericalAiming:
                myBehaviourClass = new SphericalAimingBehaviour();
                break;
            case BehaviourType.BombDrop:
                myBehaviourClass = new BombDropBehaviour();
                break;
            case BehaviourType.Trail:
                myBehaviourClass = new TrailBehaviour();
                break;
            case BehaviourType.DoubleAiming:
                myBehaviourClass = new DoubleAimingBehaviour();
                break;
        }
    }

    public void Shoot()
    {
        if (!gameManager.transitionIsRunning && !isDying)
        {
            myShot();
        }
    }

    public void Move()
    {
        if (!gameManager.transitionIsRunning)
        {
            myMovement();
        }

    }

    void ChangePerspective()
    {
        if (gameManager.transitionIsRunning)
        {
            if (gameManager.currentGameMode == GameMode.TOPDOWN)
            {
                if (!sideCollider.enabled)
                {
                    topCollider.enabled = false;
                    sideCollider.enabled = true;
                }
            }
            else
            {
                if (!topCollider.enabled)
                {
                    sideCollider.enabled = false;
                    topCollider.enabled = true;
                }
            }
        }
    }

    public void Deactivate()
    {
        if (explosionParticleManager != null)
        {
            explosionParticleManager.PlayAll();
        }
        isDying = true;
        StartCoroutine(DeactivateObject());
    }

    protected IEnumerator DeactivateObject()
    {
        yield return new WaitForSeconds(enemyDeactivationDelay);
        isDying = false;
        gameObject.SetActive(false);
    }

    public bool CheckEnemyDead()
    {
        return enemyLives <= 0;
    }
}
