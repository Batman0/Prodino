using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDoubleAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    //private GameObject sidescrollPrefab;
    //private GameObject topdownPrefab;
    private PropertiesDoubleAiming properties;
    private int indexOfBullet;
    private int indexOfSinusoideBullet;

    public override void Init()
    {
        base.Init();
        //properties = Register.instance.propertiesDoubleAiming;
        //sidescrollPrefab = properties.sidescrollBulletPrefab;
        //topdownPrefab = properties.topdownBulletPrefab;
        fireRate = properties.fireRate;
        timer = 0;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletDoubleAimingPool);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            indexOfBullet++;
            GameObject secondBullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletDoubleAimingPool); 
            secondBullet.transform.position = enemy.bulletSpawnpointOther.position;
            secondBullet.transform.rotation = Quaternion.Inverse(enemy.transform.rotation);
            secondBullet.SetActive(true);
            PoolManager.instance.bulletDoubleAimingPool.index++;
            timer = 0.0f;
        }
        if (PoolManager.instance.bulletDoubleAimingPool.index >= PoolManager.instance.pooledBulletAmount)
        {
            PoolManager.instance.bulletDoubleAimingPool.index = 0;
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletDoubleAimingSinusoidePool);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            indexOfSinusoideBullet++;
            GameObject secondBullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletDoubleAimingSinusoidePool);
            secondBullet.tag = "EnemyBulletInverse";
            secondBullet.transform.position = enemy.bulletSpawnpointOther.position;
            secondBullet.transform.rotation = enemy.transform.rotation;
            secondBullet.SetActive(true);
            PoolManager.instance.bulletDoubleAimingSinusoidePool.index++;
            timer = 0.0f;
        }

        if (PoolManager.instance.bulletDoubleAimingSinusoidePool.index >= PoolManager.instance.pooledBulletAmount)
        {
            PoolManager.instance.bulletDoubleAimingSinusoidePool.index = 0;
        }
    }
}
