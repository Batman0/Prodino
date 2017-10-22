using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyMovement(Enemy enemy);
public delegate void MyShot(Enemy enemy);

public class Enemy: MonoBehaviour
{

    [HideInInspector]
    public bool rotateRight;
    [HideInInspector]
    public bool canShoot;
    [HideInInspector]
    public bool toDestroy;
    //[HideInInspector]
    public bool isRight;

    public int enemyLife;
    [HideInInspector]
    public int movementTargetIndex;

    //[HideInInspector]
    //public float timeToShoot;
    //[HideInInspector]
    //public float waitingTime;
    //[HideInInspector]
    //public float waitingTimer;
    //[HideInInspector]
    public float lifeTime;
    //[HideInInspector]
    //public float speed;
    //[HideInInspector]
    //public float backSpeed;
    //[HideInInspector]
    //public float rotationSpeed;
    //[HideInInspector]
    //public float movementDuration;
    //[HideInInspector]
    //public float doneRotation;
    //[HideInInspector]
    //public float destructionMargin;
    //[HideInInspector]
    //public float length;
    //[HideInInspector]
    //public float amplitude;
    //[HideInInspector]
    //public float height;
    //[HideInInspector]
    //public float time;
    //[HideInInspector]
    //public float radius;

    [HideInInspector]
    public Vector3 originalPos;

    [HideInInspector]
    public Quaternion barrelStartRot;
    [HideInInspector]
    public Quaternion barrelInvertedRot;

    public Transform bulletSpawnpoint;
    public Transform bulletSpawnpointOther;
    public Transform shooterTransform;
    [HideInInspector]
    public Transform[] targets;

    public Collider sideCollider;
    public Collider topCollider;

    private GameObject particleTrail;

    public MovementType movementType;

    public ShotType shotType;

    public MyMovement myMovementSidescroll;
    public MyMovement myMovementTopdown;

    public MyShot myShotSidescroll;
    public MyShot myShotTopdown;

    private GameManager gameManager;

    private Enemy instance;

    private EnemyMovement myMovementClass;

    private EnemyShot myShotClass;

    void Start()
    {
        instance = this;
        //myMovementProperties = Register.instance.enemyProperties[(int)movementType];
        //myShotProperties = Register.instance.enemyProperties[(int)shotType];
        gameManager = GameManager.instance;
        originalPos = transform.position;
        //timeToShoot = 0.0f;
        canShoot = false;
        rotateRight = isRight ? true : false;
        InitMovement();
        myMovementClass.Init(instance);
        myMovementSidescroll += myMovementClass.MoveSidescroll;
        myMovementTopdown += myMovementClass.MoveTopdown;
        if (shotType != ShotType.FORWARD)
        {
            InitShot();
            myShotClass.Init();
            myShotSidescroll += myShotClass.ShootSidescroll;
            myShotTopdown += myShotClass.ShootTopdown;
        }
        //movementTargetIndex = 0;
        //Register.instance.numberOfTransitableObjects++;
        //if (shooterTransform != null)
        //{
        //    barrelStartRot = shooterTransform.rotation;
        //    shooterTransform.RotateAround(transform.position, Vector3.forward, 180);
        //    barrelInvertedRot = shooterTransform.rotation;
        //    shooterTransform.rotation = barrelStartRot;
        //}
        if (movementType == MovementType.CIRCULAR)
        {
            lifeTime = Register.instance.propertiesCircular.lifeTime;
        }

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

    void Update()
    {
        ChangePerspective();
        Move();
        if (shotType != ShotType.FORWARD)
            Shoot();
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
            case MovementType.FORWARDSHOOTER:
                myMovementClass = new MovementForwardShooter();
                break;
            case MovementType.FORWARD:
                myMovementClass = new MovementForward();
                break;
            case MovementType.LASERDIAGONAL:
                myMovementClass = new MovementLaserDiagonal();
                break;
            case MovementType.SPHERICALAIMING:
                myMovementClass = new MovementSphericlaAiming();
                break;
            case MovementType.BOMBDROP:
                myMovementClass = new MovementBombDrop();
                break;
            case MovementType.TRAIL:
                myMovementClass = new MovementTrail();
                break;
            case MovementType.DOUBLEAIMING:
                myMovementClass = new MovementDoubleAiming();
                break;
        }
    }

    public void InitShot()
    {
        switch (shotType)
        {
            case ShotType.FORWARDSHOOTER:
                myShotClass = new ShotForwardShooter();
                break;
            case ShotType.LASERDIAGONAL:
                myShotClass = new ShotLaserDiagonal();
                break;
            case ShotType.SPHERICALAIMING:
                myShotClass = new ShotSphericalAiming();
                break;
            case ShotType.BOMBDROP:
                myShotClass = new ShotBombDrop();
                break;
            case ShotType.TRAIL:
                myShotClass = new ShotTrail();
                break;
            case ShotType.DOUBLEAIMING:
                myShotClass = new ShotDoubleAiming();
                break;
        }
    }

    public void Shoot()
    {
        if(!gameManager.transitionIsRunning)
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
            if (gameManager.currentGameMode == GameMode.SIDESCROLL)
            {
                myMovementSidescroll(instance);
            }
            else
            {
                myMovementTopdown(instance);
            }
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
        if(CheckEnemyLife() || toDestroy)
        {
            Destroy(gameObject);
        }
    }

    public bool CheckEnemyLife()
    {
        return enemyLife <= 0;
    }

    //public void AssignProperties()
    //{
    //    switch
    //}
}
