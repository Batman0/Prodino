using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyMovement(Enemy enemy);
public delegate void MyShot(Enemy enemy);

public class Enemy : MonoBehaviour
{

    [HideInInspector]
    public bool barrelRight;
    public bool canShoot;
    public bool isShooting;
    [HideInInspector]
    public bool toDestroy;
    public bool isRight;
    private PoolManager.PoolBullet bulletPool;

    public int enemyLife;
    [HideInInspector]
    public int movementTargetIndex;
    public float lifeTime;


    [HideInInspector]
    public Vector3 originalPos;

    [HideInInspector]
    public Quaternion barrelStartRot;
    [HideInInspector]
    public Quaternion barrelInvertedRot;
    public ParticleSystemManager explosionParticleManager;

    public Transform bulletSpawnpoint;
    public Transform bulletSpawnpointOther;
    public Transform shooterTransform;
    [HideInInspector]
    public Transform meshTransform;
    [HideInInspector]
    public Transform[] targets;

    public Collider sideCollider;
    public Collider topCollider;

    private GameObject particleTrail;

    public MovementType movementType;

    public ShotType shotType;

    public MyMovement myMovement;

    public MyShot myShotSidescroll;
    public MyShot myShotTopdown;

    private GameManager gameManager;

    private Enemy instance;

    private EnemyMovement myMovementClass;

    private EnemyShot myShotClass;
    protected float enemyDeactivationDelay = 0.5f;
    private bool isDying = false;

    void OnEnable()
    {
        instance = this;
        gameManager = GameManager.instance;
        originalPos = transform.position;
        barrelRight = isRight ? false : true;

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
        if (movementType == MovementType.Trail)
        {
            meshTransform = transform.GetChild(0);
        }
        InitMovement();
        myMovementClass.Init(instance);
        myMovement += myMovementClass.Movement;
        //myMovementTopdown += myMovementClass.MoveTopdown;

        if (shotType != ShotType.Forward)
        {
            InitShot();
            myShotClass.Init();
            myShotSidescroll += myShotClass.ShootSidescroll;
            myShotTopdown += myShotClass.ShootTopdown;
        }

        if (movementType == MovementType.Circular)
        {
            lifeTime = Register.instance.propertiesCircular.lifeTime;
        }
    }

    void Update()
    {
        ChangePerspective();
        Move();
        if (shotType != ShotType.Forward)
        {
            Shoot();
        }

        Destroy();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBullet")
        {
            enemyLife--;
            other.gameObject.SetActive(false);
        }
    }

    public void InitMovement()
    {
        switch (movementType)
        {
            case MovementType.ForwardShooter:
                myMovementClass = new MovementForwardShooter();
                break;
            case MovementType.Forward:
                myMovementClass = new MovementForward();
                break;
            case MovementType.LaserDiagonal:
                myMovementClass = new MovementLaserDiagonal();
                break;
            case MovementType.SphericalAiming:
                myMovementClass = new MovementSphericalAiming();
                break;
            case MovementType.BombDrop:
                myMovementClass = new MovementBombDrop();
                break;
            case MovementType.Trail:
                myMovementClass = new MovementTrail();
                break;
            case MovementType.DoubleAiming:
                myMovementClass = new MovementDoubleAiming();
                break;
        }
    }

    public void InitShot()
    {
        switch (shotType)
        {
            case ShotType.ForwardShooter:
                myShotClass = new ShotForwardShooter();
                break;
            case ShotType.LaserDiagonal:
                myShotClass = new ShotLaserDiagonal();
                break;
            case ShotType.SphericalAiming:
                myShotClass = new ShotSphericalAiming();
                break;
            case ShotType.BombDrop:
                myShotClass = new ShotBombDrop();
                break;
            case ShotType.Trail:
                myShotClass = new ShotTrail();
                break;
            case ShotType.DoubleAiming:
                myShotClass = new ShotDoubleAiming();
                break;
        }
    }

    public void Shoot()
    {
        if (!gameManager.transitionIsRunning && !isDying)
        {
            if (gameManager.currentGameMode == GameMode.SIDESCROLL)
            {
                myShotSidescroll(instance);
            }
            else
            {
                myShotTopdown(instance);
            }
        }
    }

    public void Move()
    {
        if (!gameManager.transitionIsRunning)
        {
            myMovement(instance);
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

    public void Destroy()
    {
        if (CheckEnemyLife() || toDestroy)
        {
            if (explosionParticleManager != null)
            {
                explosionParticleManager.PlayAll();
            }
            isDying = true;
            StartCoroutine(DeactivateObject());
        }
    }

    protected IEnumerator DeactivateObject()
    {
        yield return new WaitForSeconds(enemyDeactivationDelay);
        isDying = false;
        gameObject.SetActive(false);
    }

    public bool CheckEnemyLife()
    {
        return enemyLife <= 0;
    }

    void OnDisable()
    {
        canShoot = false;
    }
}
