using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoubleAiming : Enemy
{
    
    private new PropertiesDoubleAiming property;

    [Header("Movement")]
    private float zMovementSpeed;
    private float destructionMargin;
    private float amplitude;
    private float targetPlayerDeltaDistance = 0.1f;
    private Vector3 originalPos;
    private Vector3 topdownTarget;
    private float speed;

    [Header("Shot")]
    private float fireRate;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void InitEnemy()
    {
        base.InitEnemy();
        enemyLives = property.lives;
        originalPos = transform.position;
        speed = isRight ? -property.xSpeed : property.xSpeed;
        zMovementSpeed = property.zMovementSpeed;
        destructionMargin = property.destructionMargin;
        amplitude = property.amplitude;
        topdownTarget = new Vector3(transform.position.x, transform.position.y, originalPos.z + amplitude);
        fireRate = property.fireRate;
    }

    void FixedUpdate()
    {
        Move();
    }

    public override void Update()
    {
        base.Update();
        Shoot();
    }

    public override void Move()
    {
        base.Move();
        if (Vector3.Distance(transform.position, topdownTarget) > targetPlayerDeltaDistance)
        {
            topdownTarget = new Vector3(transform.position.x,transform.position.y, topdownTarget.z);
        }
        else
        {
            zMovementSpeed = -zMovementSpeed;
            amplitude = -amplitude;
            topdownTarget = new Vector3(transform.position.x, transform.position.y, originalPos.z + amplitude);
        }

        transform.position = new Vector3(speed * Time.fixedDeltaTime + transform.position.x, transform.position.y, zMovementSpeed * Time.fixedDeltaTime + transform.position.z);

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
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.pooledBulletClass[property.enemyName].GetpooledBullet();
            bullet.tag = "EnemyBullet";
            bullet.transform.position = bulletSpawnpoint.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            GameObject secondBullet = PoolManager.instance.pooledBulletClass[property.enemyName].GetpooledBullet();
            secondBullet.tag = "EnemyBulletInverse";
            secondBullet.transform.position = bulletSpawnpointSecond.position;
            secondBullet.transform.rotation = transform.rotation;
            secondBullet.SetActive(true);
            timer = 0.0f;
        }
    }

    public override void SetProperty(ScriptableObject _property)
    {
        property = (PropertiesDoubleAiming)_property;
        InitEnemy();
    }
}