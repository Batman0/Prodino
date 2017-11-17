using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyMovement();
public delegate void MyShot();

public class Enemy : MonoBehaviour
{
    [Header("References")]
    private Enemy instance;
    private GameManager gameManager;
    private EnemyBehaviour myBehaviourClass;
    //private EnemyShot myShotClass;
    private PoolManager.PoolBullet bulletPool;

    [Header("Statistics")]
    private int enemyLives;
    //private float lifeTime;

    //public bool isShooting;
    //[HideInInspector]
    //public bool toDeactivate;
    public bool isRight;

    //[HideInInspector]
    //public int movementTargetIndex;

    //[HideInInspector]
    //public Quaternion barrelStartRot;
    //[HideInInspector]
    //public Quaternion barrelInvertedRot;
    public ParticleSystemManager explosionParticleManager;

    public Transform bulletSpawnpoint;
    public Transform bulletSpawnpointOther;
    public Transform shooterTransform;
    [HideInInspector]
    public Transform meshTransform;
    //[HideInInspector]
    //public Transform[] targets;

    public Collider sideCollider;
    public Collider topCollider;

    //private GameObject particleTrail;

    public BehaviourType behaviourType;

    //public ShotType shotType;

    public MyMovement myMovement;

    public MyShot myShot;
    //public MyShot myShotTopdown;

    protected float enemyDeactivationDelay = 0.5f;
    private bool isDying = false;

    void OnEnable()
    {
        instance = this;
        gameManager = GameManager.instance;
        enemyLives = Register.instance.enemyProperties[behaviourType.ToString()].lives;
        Debug.Log(behaviourType);
        //Debug.Log(enemyLives + " - " + name);
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
        if (behaviourType == BehaviourType.Trail)
        {
            meshTransform = transform.GetChild(0);
        }
        InitBehaviour();
        myBehaviourClass.Init(instance);
        myMovement += myBehaviourClass.Move;
        myShot += myBehaviourClass.Shoot;

        //if (movementType == MovementType.Circular)
        //{
        //    lifeTime = Register.instance.propertiesCircular.lifeTime;
        //}
    }

    void Update()
    {
        ChangePerspective();
        Move();
        Shoot();
        if (CheckEnemyDead() /*|| toDeactivate*/)
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

    //public void InitShot()
    //{
    //    switch (shotType)
    //    {
    //        case ShotType.ForwardShooter:
    //            myShotClass = new ShotForwardShooter();
    //            break;
    //        case ShotType.LaserDiagonal:
    //            myShotClass = new ShotLaserDiagonal();
    //            break;
    //        case ShotType.SphericalAiming:
    //            myShotClass = new ShotSphericalAiming();
    //            break;
    //        case ShotType.BombDrop:
    //            myShotClass = new ShotBombDrop();
    //            break;
    //        case ShotType.Trail:
    //            myShotClass = new ShotTrail();
    //            break;
    //        case ShotType.DoubleAiming:
    //            myShotClass = new ShotDoubleAiming();
    //            break;
    //    }
    //}

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

    //void OnDisable()
    //{
    //    
    //}
}
