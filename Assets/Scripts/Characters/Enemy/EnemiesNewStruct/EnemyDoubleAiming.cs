using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoubleAiming : Enemy
{
    
    //private new PropertiesDoubleAiming property;

    //private bool moveForward;
    [SerializeField]
    private float zMovementSpeed;
    private float zMovementSpeedAdjustable;
    //[SerializeField]
    //private float destructionMargin;
    [SerializeField]
    private float amplitude;
    private float amplitudeAdjustable;
    private float targetPlayerDeltaDistance = 0.1f;
    private Vector3 originalPos;
    private Vector3 topdownTarget;
    [SerializeField]
    private float fireRate;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void ConstructEnemy()
    {
        base.ConstructEnemy();
        targetPlayerDeltaDistance = zMovementSpeed / 10;
    }

    public override void InitEnemy()
    {
        base.InitEnemy();
        originalPos = transform.position;
        xSpeedAdjustable = isRight ? -xSpeedAdjustable : xSpeedAdjustable;
        zMovementSpeedAdjustable = zMovementSpeed;
        amplitudeAdjustable = amplitude;
        topdownTarget = new Vector3(transform.position.x, transform.position.y, originalPos.z + amplitude);
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
            bullet.tag = "EnemyBullet";
            bullet.transform.position = bulletSpawnpoint.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            GameObject secondBullet = PoolManager.instance.pooledBulletClass[enemyName].GetpooledBullet();
            secondBullet.tag = "EnemyBulletInverse";
            secondBullet.transform.position = bulletSpawnpointSecond.position;
            secondBullet.transform.rotation = transform.rotation;
            secondBullet.SetActive(true);
            fireRateTimer = 0.0f;
        }
    }

    //public override void SetProperty(ScriptableObject _property)
    //{
    //    property = (PropertiesDoubleAiming)_property;
    //    InitEnemy();
    //}
}