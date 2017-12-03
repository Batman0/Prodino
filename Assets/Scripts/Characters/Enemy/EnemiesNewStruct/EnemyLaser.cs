using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : Enemy
{
    
    //private new PropertiesLaserDiagonal property;

    private bool moveUp;
    [SerializeField]
    private float yMovementSpeed;
    [SerializeField]
    private float yMovementSpeedShooting;
    //[SerializeField]
    //private float destructionMargin;
    [SerializeField]
    private float amplitude;
    //private float downDistance;
    private float targetPlayerDeltaDistance;
    private Vector3 originalPos;
    private Vector3 sidescrollTarget;

    private bool isShooting;
    [SerializeField]
    private float waitingTime;
    [SerializeField]
    private float loadingTime;
    [SerializeField]
    private float shootingTime;
    [SerializeField]
    private float width;
    [SerializeField]
    private float height;
    private GameObject laser;

    public override void InitEnemy()
    {
        base.InitEnemy();
        //enemyLives = property.lives;
        originalPos = transform.position;
        //Debug.Log(originalPos);
        //Debug.Log(transform.position);
        xSpeed = isRight ? -xSpeed : xSpeed;
        moveUp = true;
        //yMovementSpeed = property.yMovementSpeed;
        //yMovementSpeedShooting = property.yMovementSpeedShooting;
        targetPlayerDeltaDistance = yMovementSpeed / 10;
        //destructionMargin = property.destructionMargin;
        //amplitude = property.amplitude;
        //downDistance = property.downDistance;
        sidescrollTarget = new Vector3(transform.position.x, originalPos.y + amplitude, transform.position.z);
        //waitingTime = property.waitingTime;
        //loadingTime = property.loadingTime;
        //shootingTime = property.shootingTime;
        //width = property.laserWidth;
        //height = property.laserHeight;
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
            //Debug.Log(transform.position);
        }
        else
        {
            //Debug.Log("ddddddd");
            yMovementSpeed = -yMovementSpeed;
            yMovementSpeedShooting = -yMovementSpeedShooting;
            amplitude = -amplitude;
            sidescrollTarget = new Vector3(transform.position.x, originalPos.y + amplitude, transform.position.z);
        }

        transform.position = new Vector3(xSpeed * Time.fixedDeltaTime + transform.position.x, ((!isShooting ? yMovementSpeed : yMovementSpeedShooting) * Time.fixedDeltaTime) + transform.position.y, transform.position.z);

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
        if (fireRateTimer < waitingTime)
        {
            fireRateTimer += Time.deltaTime;
        }
        else
        {
            if (fireRateTimer < waitingTime + loadingTime)
            {
                fireRateTimer += Time.deltaTime;
            }
            else
            {
                if (!laser)
                {
                    laser = PoolManager.instance.pooledBulletClass[enemyName].GetpooledBullet();
                    laser.SetActive(true);
                    laser.transform.SetParent(bulletSpawnpoint.parent);
                    laser.transform.position = bulletSpawnpoint.position;
                    laser.transform.rotation = transform.rotation;
                    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
                    isShooting = true;
                }
                if (fireRateTimer < waitingTime + loadingTime + shootingTime)
                {
                    fireRateTimer += Time.deltaTime;
                }
                else
                {
                    laser.SetActive(false);
                    laser = null;
                    fireRateTimer = 0.0f;
                    isShooting = false;
                }
            }
        }
    }

    //public override void SetProperty(ScriptableObject _property)
    //{
    //    property = (PropertiesLaserDiagonal) _property;
    //    InitEnemy();
    //}
}
