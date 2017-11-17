using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalAimingBehaviour : EnemyBehaviour
{

    [Header("Common")]
    private Transform playerTr;
    private Register register;
    private PropertiesSphericalAiming properties;

    [Header("Movement")]
    private bool moveForward;
    [HideInInspector]
    public bool barrelRight;
    private float zMovementSpeed;
    private float destructionMargin;
    private float forwardDistance;
    private float backDistance;
    private float targetPlayerDeltaDistance = 0.1f;
    private float rotationDeadZone;
    private float rotationSpeed;
    private float xMin;
    private float xMax;
    private Vector3 originalPos;
    private Vector3 topdownTarget;
    private Quaternion shooterTransformStartRotation;
    private Quaternion shooterTransformInverseRotation;
    private Collider playerCl;
    private GameManager gameManager;

    [Header("Shot")]
    private float fireRate;
    private float timer;
    private PoolManager.PoolBullet bulletPool;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        properties = register.propertiesSphericalAiming;
        playerTr = register.player.transform;
        gameManager = GameManager.instance;
        originalPos = enemy.transform.position;
        speed = enemy.isRight ? -properties.xSpeed : properties.xSpeed;
        barrelRight = enemy.isRight ? false : true;
        zMovementSpeed = properties.zMovementSpeed;
        destructionMargin = properties.destructionMargin;
        forwardDistance = properties.forwardDistance;
        backDistance = properties.backDistance;
        topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, originalPos.z + forwardDistance);
        moveForward = true;
        playerCl = register.player.sideBodyCollider;
        rotationDeadZone = properties.rotationDeadZone;
        rotationSpeed = properties.rotationSpeed;
        shooterTransformStartRotation = enemy.shooterTransform.rotation;
        shooterTransformInverseRotation = Quaternion.Inverse(shooterTransformStartRotation);
        xMin = register.xMin;
        xMax = register.xMax;

        fireRate = properties.fireRate;
        timer = 0;
        bulletPool = PoolManager.instance.pooledBulletClass["SphericalAimingBullet"];
    }

    public override void Move()
    {
        if (Vector3.Distance(enemyInstance.transform.position, topdownTarget) > targetPlayerDeltaDistance)
        {
            topdownTarget = new Vector3(enemyInstance.transform.position.x, enemyInstance.transform.position.y, topdownTarget.z);
        }
        else
        {
            moveForward = !moveForward;
            zMovementSpeed = -zMovementSpeed;
            topdownTarget = moveForward ? new Vector3(enemyInstance.transform.position.x, enemyInstance.transform.position.y, originalPos.z + forwardDistance) : new Vector3(enemyInstance.transform.position.x, enemyInstance.transform.position.y, originalPos.z - backDistance);
        }

        enemyInstance.transform.position = new Vector3(speed * Time.deltaTime + enemyInstance.transform.position.x, enemyInstance.transform.position.y, zMovementSpeed * Time.deltaTime + enemyInstance.transform.position.z);

        if (enemyInstance.isRight)
        {
            if (enemyInstance.transform.position.x <= xMin - destructionMargin)
            {
                enemyInstance.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemyInstance.transform.position.x >= xMax + destructionMargin)
            {
                Object.Destroy(enemyInstance.gameObject);
            }
        }

        if (gameManager.currentGameMode == GameMode.SIDESCROLL)
        {
            Vector3 playerTransform = new Vector3(playerCl.bounds.center.x - enemyInstance.transform.position.x, playerCl.bounds.center.y - enemyInstance.transform.position.y, 0);
            Vector3 barrelSpawnpointTransform = new Vector3(enemyInstance.bulletSpawnpoint.position.x - enemyInstance.transform.position.x, enemyInstance.bulletSpawnpoint.position.y - enemyInstance.transform.position.y, 0);
            float angle = Vector3.Angle(barrelSpawnpointTransform, playerTransform);
            Vector3 cross = Vector3.Cross(playerTransform, barrelSpawnpointTransform);

            if (angle > rotationDeadZone)
            {
                if (cross.z >= 0)
                {
                    enemyInstance.shooterTransform.RotateAround(enemyInstance.transform.position, Vector3.forward, -rotationSpeed);
                }
                else
                {
                    enemyInstance.shooterTransform.RotateAround(enemyInstance.transform.position, Vector3.forward, rotationSpeed);
                }
            }
        }
        else
        {
            if (enemyInstance.transform.position.x < playerTr.position.x)
            {
                if (barrelRight)
                {
                    Debug.Log("right");
                    enemyInstance.shooterTransform.rotation = enemyInstance.isRight ? shooterTransformInverseRotation : shooterTransformStartRotation;
                    barrelRight = false;
                }
            }
            else
            {
                if (!barrelRight)
                {
                    Debug.Log("LERFT");
                    enemyInstance.shooterTransform.rotation = enemyInstance.isRight ? shooterTransformStartRotation : shooterTransformInverseRotation;
                    barrelRight = true;
                }
            }
        }
    }

    public override void Shoot()
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = bulletPool.GetpooledBullet();
            bullet.transform.position = enemyInstance.bulletSpawnpoint.position;
            bullet.transform.rotation = enemyInstance.shooterTransform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

    //public override void ShootTopdown()
    //{
    //    base.ShootTopdown();
    //    if (timer < fireRate)
    //    {
    //        timer += Time.deltaTime;
    //    }
    //    else
    //    {
    //        GameObject bullet = bulletPool.GetpooledBullet();
    //        bullet.transform.position = enemyInstance.bulletSpawnpoint.position;
    //        bullet.transform.rotation = enemyInstance.shooterTransform.rotation;
    //        bullet.SetActive(true);
    //        timer = 0.0f;
    //    }
    //}
}
