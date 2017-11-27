using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrail : Enemy
{
    private new PropertiesTrail property;

    private bool canShoot;
    private float destructionMargin;

    [Header("Movement")]
    private float speed;
    private float backSpeed;
    private float rotationSpeed;
    private float movementDuration;
    private float waitingTimer;
    private float doneRotation;
    private Transform enemyTransform;

    [Header("Shot")]
    private GameObject trail;

    public override void InitEnemy()
    {
        base.InitEnemy();
        enemyLives = property.lives;
        speed = property.xSpeed;
        backSpeed = property.xReturnSpeed;
        rotationSpeed = property.rotationSpeed;
        movementDuration = property.movementDuration;
        waitingTimer = 0;
        doneRotation = 0;
        enemyTransform = transform.GetChild(0);
        trail = property.bulletPrefab;
    }

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

    public override void Move()
    {
        base.Move();
        
        
            if (waitingTimer < movementDuration && doneRotation == 0)
            {
                transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime, Space.Self);
                waitingTimer += Time.deltaTime;
            }
            else if (waitingTimer > 0.0f && doneRotation >= 180)
            {
                transform.Translate(Vector3.forward * -backSpeed * Time.fixedDeltaTime, Space.Self);
                waitingTimer -= Time.deltaTime;
            }
            else if (waitingTimer <= 0.0f && doneRotation >= 180)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (doneRotation < 180)
                {
                    enemyTransform.Rotate(Vector3.up, rotationSpeed);
                    doneRotation += rotationSpeed;
                }
                if (doneRotation >= 180 && !canShoot)
                {
                    canShoot = true;
                }
            }
    }

    public override void Shoot()
    {
        base.Shoot();

        if(canShoot && !trail)
        {
            trail = PoolManager.instance.pooledBulletClass[property.enemyName].GetpooledBullet();
            trail.transform.position = bulletSpawnpoint.position;
            trail.transform.rotation = Quaternion.Inverse(transform.rotation);
            trail.SetActive(true);
            trail.transform.SetParent(bulletSpawnpoint);
            canShoot = false;
        }
    }

    public override void SetProperty(ScriptableObject _property)
    {
        property = (PropertiesTrail)_property;
        InitEnemy();
    }
}