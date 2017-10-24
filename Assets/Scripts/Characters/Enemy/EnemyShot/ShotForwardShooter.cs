using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotForwardShooter : EnemyShot
{

    private float fireRate;
    private float timer;
    private PropertiesForwardShooter properties;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesForwardShooter;
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletForwardShooterPool,ref PoolManager.instance.forwardShooterBulletAmount);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletForwardShooterPool, ref PoolManager.instance.forwardShooterBulletAmount);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }
}
