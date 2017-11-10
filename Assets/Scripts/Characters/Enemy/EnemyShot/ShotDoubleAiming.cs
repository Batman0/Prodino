using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDoubleAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    private PropertiesDoubleAiming properties;
    private PoolManager.PoolBullet bulletPool,bulletPoolSin;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesDoubleAiming;
        bulletPool = PoolManager.instance.pooledBulletClass["DoubleAimingBullet"];
        bulletPoolSin = PoolManager.instance.pooledBulletClass["DoubleAimingSinusoideBullet"];
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
            GameObject bullet = bulletPool.GetpooledBullet();
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            GameObject secondBullet =bulletPool.GetpooledBullet();
            secondBullet.transform.position = enemy.bulletSpawnpointOther.position;
            secondBullet.transform.rotation = Quaternion.Inverse(enemy.transform.rotation);
            secondBullet.SetActive(true);
            timer = 0.0f;
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
            GameObject bullet = bulletPoolSin.GetpooledBullet();
            bullet.tag = "EnemyBullet";
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            GameObject secondBullet = bulletPoolSin.GetpooledBullet();
            secondBullet.tag = "EnemyBulletInverse";
            secondBullet.transform.position = enemy.bulletSpawnpointOther.position;
            secondBullet.transform.rotation = enemy.transform.rotation;
            secondBullet.SetActive(true);
            timer = 0.0f;
        }
    }
}
