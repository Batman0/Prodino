using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyMovement(Enemy enemy);

public class Enemy: MonoBehaviour
{

    private bool canRotate;
    [HideInInspector]
    public bool shoots;
    [HideInInspector]
    public bool toDestroy;
    [HideInInspector]
    public bool isRight;

    public int enemyLife;
    [HideInInspector]
    public int movementTargetIndex;

    [HideInInspector]
    public float timeToShoot;
    [HideInInspector]
    public float waitingTime;
    [HideInInspector]
    public float waitingTimer;
    [HideInInspector]
    public float lifeTime;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float backSpeed;
    [HideInInspector]
    public float rotationSpeed;
    [HideInInspector]
    public float movementDuration;
    [HideInInspector]
    public float doneRotation;
    [HideInInspector]
    public float destructionMargin;
    [HideInInspector]
    public float length;
    [HideInInspector]
    public float amplitude;
    [HideInInspector]
    public float height;
    [HideInInspector]
    public float time;
    [HideInInspector]
    public float radius;

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

    private GameManager gameManager;

    private EnemyMovement myEnemyMovement;

    private Enemy instance;

    public Properties myMovementProperties;
    private Properties myShotProperties;

    void Start()
    {
        instance = this;
        if (movementType == MovementType.FORWARDSHOOTER)
        {
            myEnemyMovement = new MovementForwardShooter();
        }
        else if (movementType == MovementType.FORWARD)
        {
            myEnemyMovement = new MovementForward();
        }
        else if (movementType == MovementType.LASERDIAGONAL)
        {
            myEnemyMovement = new MovementLaserDiagonal();
        }
        else if (movementType == MovementType.SPHERICALAIMING)
        {
            myEnemyMovement = new MovementSphericalAiming();
        }
        else if (movementType == MovementType.BOMBDROP)
        {
            myEnemyMovement = new MovementBombDrop();
        }
        else if (movementType == MovementType.TRAIL)
        {
            myEnemyMovement = new MovementTrail();
        }
        else if (movementType == MovementType.DOUBLEAIMING)
        {
            myEnemyMovement = new MovementDoubleAiming();
        }
        myEnemyMovement.Init(this);
        myMovementSidescroll += myEnemyMovement.MoveSidescroll;
        myMovementTopdown += myEnemyMovement.MoveTopdown;
        //myMovementProperties = Register.instance.enemyProperties[(int)movementType];
        //myShotProperties = Register.instance.enemyProperties[(int)shotType];
        gameManager = GameManager.instance;
        //Movements.SetMovement(this);
        movementTargetIndex = 0;
        //Register.instance.numberOfTransitableObjects++;
        originalPos = transform.position;
        timeToShoot = 0.0f;
        shoots = false;
        canRotate = isRight ? true : false;
        if (shooterTransform != null)
        {
            barrelStartRot = shooterTransform.rotation;
            shooterTransform.RotateAround(transform.position, Vector3.forward, 180);
            barrelInvertedRot = shooterTransform.rotation;
            shooterTransform.rotation = barrelStartRot;
        }

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

    //void OnDestroy()
    //{
    //    Register.instance.numberOfTransitableObjects--;
    //}

    public void Shoot()
    {
        if(!gameManager.transitionIsRunning)
        {
            Shots.Shoot(shotType, barrelStartRot, barrelInvertedRot, ref timeToShoot, ref shoots, ref canRotate,ref particleTrail,bulletSpawnpoint, shooterTransform!=null ? shooterTransform : transform, transform, bulletSpawnpointOther);
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
