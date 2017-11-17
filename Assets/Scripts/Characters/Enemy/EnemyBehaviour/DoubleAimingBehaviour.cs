using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAimingBehaviour : EnemyBehaviour
{
    [Header("Common")]
    private PropertiesDoubleAiming properties;

    [Header("Movement")]
    private float zMovementSpeed;
    private float destructionMargin;
    private float amplitude;
    private float targetPlayerDeltaDistance = 0.1f;
    private float xMin;
    private float xMax;
    private Vector3 originalPos;
    private Vector3 topdownTarget;
    private Register register;

    [Header("Shot")]
    private float fireRate;
    private float timer;
    private PoolManager.PoolBullet /*bulletPool, */bulletPoolSin;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        originalPos = enemy.transform.position;
        properties = register.propertiesDoubleAiming;
        speed = enemy.isRight ? -properties.xSpeed : properties.xSpeed;
        zMovementSpeed = properties.zMovementSpeed;
        destructionMargin = properties.destructionMargin;
        amplitude = properties.amplitude;
        topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, originalPos.z + amplitude);
        xMin = register.xMin;
        xMax = register.xMax;

        //bulletPool = PoolManager.instance.pooledBulletClass["DoubleAimingBullet"];
        bulletPoolSin = PoolManager.instance.pooledBulletClass["DoubleAimingSinusoideBullet"];
        fireRate = properties.fireRate;
        timer = 0;
    }

    public override void Move()
    {
        if (Vector3.Distance(enemyInstance.transform.position, topdownTarget) > targetPlayerDeltaDistance)
        {
            topdownTarget = new Vector3(enemyInstance.transform.position.x, enemyInstance.transform.position.y, topdownTarget.z);
        }
        else
        {
            zMovementSpeed = -zMovementSpeed;
            amplitude = -amplitude;
            topdownTarget = new Vector3(enemyInstance.transform.position.x, enemyInstance.transform.position.y, originalPos.z + amplitude);
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
                enemyInstance.gameObject.SetActive(false);
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
            GameObject bullet = bulletPoolSin.GetpooledBullet();
            bullet.tag = "EnemyBullet";
            bullet.transform.position = enemyInstance.bulletSpawnpoint.position;
            bullet.transform.rotation = enemyInstance.transform.rotation;
            bullet.SetActive(true);
            GameObject secondBullet = bulletPoolSin.GetpooledBullet();
            secondBullet.tag = "EnemyBulletInverse";
            secondBullet.transform.position = enemyInstance.bulletSpawnpointOther.position;
            secondBullet.transform.rotation = enemyInstance.transform.rotation;
            secondBullet.SetActive(true);
            timer = 0.0f;
        }
    }

    //public override void ShootTopdown()
    //{
    //    if (!isShootSecond)
    //    {
    //        isShootSecond = true;
    //        return;
    //    }
    //    if (timer < fireRate)
    //    {
    //        timer += Time.deltaTime;
    //    }
    //    else
    //    {
    //        GameObject bullet = bulletPool.GetpooledBullet();
    //        bullet.transform.position = enemyInstance.bulletSpawnpoint.position;
    //        bullet.transform.rotation = enemyInstance.transform.rotation;
    //        bullet.SetActive(true);
    //        GameObject secondBullet = bulletPool.GetpooledBullet();
    //        secondBullet.transform.position = enemyInstance.bulletSpawnpointOther.position;
    //        secondBullet.transform.rotation = Quaternion.Inverse(enemyInstance.transform.rotation);
    //        secondBullet.SetActive(true);
    //        timer = 0.0f;
    //    }
    //}
}
