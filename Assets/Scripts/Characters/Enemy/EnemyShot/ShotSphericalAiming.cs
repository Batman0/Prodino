using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSphericalAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    //private GameObject prefab;
    private PropertiesSphericalAiming properties;
    private int indexOfBullet;

    public override void Init()
    {
        base.Init();
        //properties = Register.instance.propertiesSphericalAiming;
        fireRate = properties.fireRate;
        timer = 0;
        //prefab = properties.bulletPrefab;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletSphericalAimingPool);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.shooterTransform.rotation;
            bullet.SetActive(true);
            PoolManager.instance.bulletSphericalAimingPool.index++;
            timer = 0.0f;
        }

        if (PoolManager.instance.bulletSphericalAimingPool.index >= PoolManager.instance.pooledBulletAmount)
        {
            PoolManager.instance.bulletSphericalAimingPool.index = 0;
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletSphericalAimingPool);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.shooterTransform.rotation;
            bullet.SetActive(true);
            PoolManager.instance.bulletSphericalAimingPool.index++;
            timer = 0.0f;
        }

        if (PoolManager.instance.bulletSphericalAimingPool.index >= PoolManager.instance.pooledBulletAmount)
        {
            PoolManager.instance.bulletSphericalAimingPool.index = 0;
        }
    }

}
