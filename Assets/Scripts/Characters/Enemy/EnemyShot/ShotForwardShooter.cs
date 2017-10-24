using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotForwardShooter : EnemyShot
{

    private float fireRate;
    private float timer;
    //private GameObject prefab;
    private PropertiesForwardShooter properties;
    private int indexOfBullet;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesForwardShooter;
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletForwardShooterPool);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            PoolManager.instance.bulletForwardShooterPool.index++;
            timer = 0.0f;
        }
        if (PoolManager.instance.bulletForwardShooterPool.index >= PoolManager.instance.pooledBulletAmount)
        {
            PoolManager.instance.bulletForwardShooterPool.index = 0;
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletForwardShooterPool);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            PoolManager.instance.bulletForwardShooterPool.index++;
            timer = 0.0f;
        }
        if (PoolManager.instance.bulletForwardShooterPool.index >= PoolManager.instance.pooledBulletAmount)
        {
            PoolManager.instance.bulletForwardShooterPool.index = 0;
        }
    }
}
