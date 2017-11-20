using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDiagonalBehaviour : EnemyBehaviour
{

    [Header("Movement/Shot")]
    private PropertiesLaserDiagonal properties;

    [Header("Movement")]
    private bool moveUp;
    private float yMovementSpeed;
    private float yMovementSpeedShooting;
    private float destructionMargin;
    private float upDistance;
    private float downDistance;
    private float targetPlayerDeltaDistance;
    private float xMin;
    private float xMax;
    private Vector3 originalPos;
    private Vector3 sidescrollTarget;
    private Register register;

    [Header("Shot")]
    private bool isShooting;
    private float waitingTime;
    private float loadingTime;
    private float shootingTime;
    private float width;
    private float height;
    private float timer;
    private GameObject laser;
    private PoolManager.PoolBullet bulletPool;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        properties = register.propertiesLaserDiagonal;
        originalPos = enemy.transform.position;
        speed = enemy.isRight ? -properties.xSpeed : properties.xSpeed;
        moveUp = true;
        yMovementSpeed = properties.yMovementSpeed;
        yMovementSpeedShooting = properties.yMovementSpeedShooting;
        targetPlayerDeltaDistance = Mathf.Max(yMovementSpeed, yMovementSpeedShooting) / 10;
        destructionMargin = properties.destructionMargin;
        upDistance = properties.upDistance;
        downDistance = properties.downDistance;
        sidescrollTarget = new Vector3(enemy.transform.position.x, originalPos.y + upDistance, enemy.transform.position.z);
        xMin = register.xMin;
        xMax = register.xMax;

        bulletPool = PoolManager.instance.pooledBulletClass["LaserBullet"];
        waitingTime = properties.waitingTime;
        loadingTime = properties.loadingTime;
        shootingTime = properties.shootingTime;
        width = properties.laserWidth;
        height = properties.laserHeight;
        timer = 0;
    }

    public override void Move()
    {
        if (Vector3.Distance(enemyInstance.transform.position, sidescrollTarget) > targetPlayerDeltaDistance)
        {
            sidescrollTarget = new Vector3(enemyInstance.transform.position.x, sidescrollTarget.y, enemyInstance.transform.position.z);
        }
        else
        {
            moveUp = !moveUp;
            yMovementSpeed = moveUp ? Mathf.Abs(yMovementSpeed) : -Mathf.Abs(yMovementSpeed);
            yMovementSpeedShooting = moveUp ? Mathf.Abs(yMovementSpeedShooting) : -Mathf.Abs(yMovementSpeedShooting);
            sidescrollTarget = moveUp ? new Vector3(enemyInstance.transform.position.x, originalPos.y + upDistance, enemyInstance.transform.position.z) : new Vector3(enemyInstance.transform.position.x, originalPos.y - downDistance, enemyInstance.transform.position.z);
        }

        enemyInstance.transform.position = new Vector3(speed * Time.deltaTime + enemyInstance.transform.position.x, (!isShooting ? yMovementSpeed : yMovementSpeedShooting) * Time.deltaTime + enemyInstance.transform.position.y, enemyInstance.transform.position.z);

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
                enemyInstance.gameObject.SetActive(false);
            }
        }
    }

    public override void Shoot()
    {
        if (timer < waitingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (timer < waitingTime + loadingTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (!laser)
                {
                    laser = bulletPool.GetpooledBullet();
                    laser.SetActive(true);
                    laser.transform.SetParent(enemyInstance.bulletSpawnpoint.parent);
                    laser.transform.position = enemyInstance.bulletSpawnpoint.position;
                    laser.transform.rotation = enemyInstance.transform.rotation;
                    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
                    //enemy.canShoot = false;
                    isShooting = true;
                }
                if (timer < waitingTime + loadingTime + shootingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    laser.SetActive(false);
                    laser = null;
                    timer = 0.0f;
                    isShooting = false;
                    //canShoot = true;
                }
            }
        }
    }
}
