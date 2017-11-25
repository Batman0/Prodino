using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : Enemy
{
    
    private new PropertiesLaserDiagonal property;

    [Header("Movement")]
    private bool moveUp;
    private float yMovementSpeed;
    private float yMovementSpeedShooting;
    private float destructionMargin;
    private float upDistance;
    private float downDistance;
    private float targetPlayerDeltaDistance;
    private Vector3 originalPos;
    private Vector3 sidescrollTarget;
    private float speed;

    [Header("Shot")]
    private bool isShooting;
    private float waitingTime;
    private float loadingTime;
    private float shootingTime;
    private float width;
    private float height;
    private GameObject laser;

    public override void InitEnemy()
    {
        base.InitEnemy();
        enemyLives = property.lives;
        originalPos = transform.position;
        speed = isRight ? -property.xSpeed : property.xSpeed;
        moveUp = true;
        yMovementSpeed = property.yMovementSpeed;
        yMovementSpeedShooting = property.yMovementSpeedShooting;
        targetPlayerDeltaDistance = Mathf.Max(yMovementSpeed, yMovementSpeedShooting) / 10;
        destructionMargin = property.destructionMargin;
        upDistance = property.upDistance;
        downDistance = property.downDistance;
        sidescrollTarget = new Vector3(transform.position.x, originalPos.y + upDistance, transform.position.z);
        waitingTime = property.waitingTime;
        loadingTime = property.loadingTime;
        shootingTime = property.shootingTime;
        width = property.laserWidth;
        height = property.laserHeight;
    }

   void FixedUpdate()
    {
        Move();
    }
	
	public override void Update ()
    {
        base.Update();
        Shoot();
    }

    public override void Move()
    {
        base.Move();
        if (Vector3.Distance(transform.position, sidescrollTarget) > targetPlayerDeltaDistance)
        {
            sidescrollTarget = new Vector3(transform.position.x, sidescrollTarget.y, transform.position.z);
        }
        else
        {
            moveUp = !moveUp;
            yMovementSpeed = moveUp ? Mathf.Abs(yMovementSpeed) : -Mathf.Abs(yMovementSpeed);
            yMovementSpeedShooting = moveUp ? Mathf.Abs(yMovementSpeedShooting) : -Mathf.Abs(yMovementSpeedShooting);
            sidescrollTarget = moveUp ? new Vector3(transform.position.x, originalPos.y + upDistance, transform.position.z) : new Vector3(transform.position.x, originalPos.y - downDistance, transform.position.z);
        }

        transform.position = new Vector3(speed * Time.fixedDeltaTime + transform.position.x, (!isShooting ? yMovementSpeed : yMovementSpeedShooting) * Time.fixedDeltaTime + transform.position.y, transform.position.z);

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
    }

    public override void Shoot()
    {
        base.Shoot();
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
                    laser = PoolManager.instance.pooledBulletClass[property.enemyName].GetpooledBullet();
                    laser.SetActive(true);
                    laser.transform.SetParent(bulletSpawnpoint.parent);
                    laser.transform.position = bulletSpawnpoint.position;
                    laser.transform.rotation = transform.rotation;
                    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
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
                }
            }
        }
    }

    public override void SetProperty(EnemyProperties _property)
    {
        property = (PropertiesLaserDiagonal) _property;
    }
}
