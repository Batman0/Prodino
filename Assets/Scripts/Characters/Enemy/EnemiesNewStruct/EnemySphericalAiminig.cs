using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySphericalAiminig : Enemy
{
    
    //private new PropertiesSphericalAiming property;

    //private bool moveForward;
    private bool barrelRight;
    [SerializeField]
    private float zMovementSpeed;
    private float zMovementSpeedAdjustable;
    //[SerializeField]
    //private float destructionMargin;
    [SerializeField]
    private float amplitude;
    private float amplitudeAdjustable;
    private float targetPlayerDeltaDistance = 0.1f;
    [SerializeField]
    private float rotationDeadZone;
    [SerializeField]
    private float rotationSpeed;
    private Vector3 originalPos;
    private Vector3 topdownTarget;
    private Quaternion shooterTransformStartRotation;
    private Quaternion shooterTransformInverseRotation;
    private Collider playerCl;

    [SerializeField]
    private float fireRate;

    private Transform playerTr;

    void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();
        Shoot();

	}

    public override void ConstructEnemy()
    {
        base.ConstructEnemy();
        playerTr = register.player.transform;
        playerCl = register.player.sideBodyCollider;
        shooterTransformStartRotation = shooterTransform.rotation;
        shooterTransformInverseRotation = Quaternion.Inverse(shooterTransformStartRotation);
        targetPlayerDeltaDistance = zMovementSpeed / 10;
    }

    public override void InitEnemy()
    {
        base.InitEnemy();
        originalPos = transform.position;
        xSpeedAdjustable = isRight ? -xSpeedAdjustable : xSpeedAdjustable;
        barrelRight = isRight ? false : true;
        amplitudeAdjustable = amplitude;
        topdownTarget = new Vector3(transform.position.x, transform.position.y, originalPos.z + amplitude);
        zMovementSpeedAdjustable = zMovementSpeed;
    }

    public override void Move()
    {
        base.Move();
        if (Vector3.Distance(transform.position, topdownTarget) > targetPlayerDeltaDistance)
        {
            topdownTarget = new Vector3(transform.position.x, transform.position.y, topdownTarget.z);
        }
        else
        {
            zMovementSpeedAdjustable = -zMovementSpeedAdjustable;
            amplitudeAdjustable = -amplitudeAdjustable;
            topdownTarget = new Vector3(transform.position.x, transform.position.y, originalPos.z + amplitudeAdjustable);
        }

        transform.position = new Vector3(xSpeedAdjustable * Time.fixedDeltaTime + transform.position.x, transform.position.y, zMovementSpeedAdjustable * Time.fixedDeltaTime + transform.position.z);

        if (isRight)
        {
            if (transform.position.x <= xMin - destructionMargin)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.x >= xMax + destructionMargin)
            {
                gameObject.SetActive(false);
            }
        }

        if (gameManager.currentGameMode == GameMode.SIDESCROLL)
        {
            Vector3 playerTransform = new Vector3(playerCl.bounds.center.x - transform.position.x, playerCl.bounds.center.y - transform.position.y, 0);
            Vector3 barrelSpawnpointTransform = new Vector3(bulletSpawnpoint.position.x - transform.position.x, bulletSpawnpoint.position.y - transform.position.y, 0);
            float angle = Vector3.Angle(barrelSpawnpointTransform, playerTransform);
            Vector3 cross = Vector3.Cross(playerTransform, barrelSpawnpointTransform);

            if (angle > rotationDeadZone)
            {
                if (cross.z >= 0)
                {
                    shooterTransform.RotateAround(transform.position, Vector3.forward, -rotationSpeed);
                }
                else
                {
                    shooterTransform.RotateAround(transform.position, Vector3.forward, rotationSpeed);
                }
            }
        }
        else
        {
            if (transform.position.x < playerTr.position.x)
            {
                if (shooterTransform.rotation != shooterTransformInverseRotation)
                {
                    shooterTransform.rotation = shooterTransformInverseRotation;
                }
            }
            else
            {
                if (shooterTransform.rotation != shooterTransformStartRotation)
                {
                    shooterTransform.rotation = shooterTransformStartRotation;
                }
            }
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        if (fireRateTimer < fireRate)
        {
            fireRateTimer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.pooledBulletClass[enemyName].GetpooledBullet();
            bullet.transform.position = bulletSpawnpoint.position;
            bullet.transform.rotation = shooterTransform.rotation;
            bullet.SetActive(true);
            fireRateTimer = 0.0f;
        }
    }

    //public override void SetProperty(ScriptableObject _property)
    //{
    //    property = (PropertiesSphericalAiming)_property;
    //    InitEnemy();
    //}
}
