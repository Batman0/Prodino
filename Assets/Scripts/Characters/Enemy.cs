using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int movementTargetIndex;
    [HideInInspector]
    public bool isRight;
    private bool shoots;
    //private bool canShoot;
    private float waitingTimer;
    [HideInInspector]
    public MovementType movementType;
    [HideInInspector]
    public ShotType shotType;
    private bool toDestroy;
    private bool canRotate;
    public int enemyLife;
    [HideInInspector]
    public Vector3 originalPos;
    [HideInInspector]
    public Quaternion barrelStartRot;
    [HideInInspector]
    public Quaternion barrelInvertedRot;
    private float lifeTime;
    public Transform bulletSpawnpoint;
    public Transform bulletSpawnpointOther;
    private float timeToShoot;
    private float doneRotation;
    private float laserDiagonalTime;
    public Collider sideCollider;
    public Collider topCollider;
    public Transform shooterTransform;
    private GameObject particleTrail;

    void Start()
    {
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

        if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
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
            Destroy(other.transform.gameObject);
        }
    }

    //void OnDestroy()
    //{
    //    Register.instance.numberOfTransitableObjects--;
    //}

    public void Shoot()
    {
        if(!GameManager.instance.transitionIsRunning)
        {
            Shots.Shoot(shotType, barrelStartRot, barrelInvertedRot, ref timeToShoot, ref shoots, ref canRotate,ref particleTrail,bulletSpawnpoint, shooterTransform!=null ? shooterTransform : transform, transform, bulletSpawnpointOther);
        }
    }

    public void Move()
    {
        if (!GameManager.instance.transitionIsRunning)
        {
            Movements.Move(movementType, transform, isRight, ref shoots, originalPos, ref movementTargetIndex, ref lifeTime, ref waitingTimer, ref doneRotation, ref laserDiagonalTime, ref toDestroy);
        }
    }

    void ChangePerspective()
    {
        if (GameManager.instance.transitionIsRunning)
        {
            if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
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
}
