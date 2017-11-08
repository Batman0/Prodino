using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSphericalAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    private Transform playerTr;
    private Register register;
    private PropertiesSphericalAiming properties;
    private PoolManager.PoolBullet bulletPool;

    public override void Init()
    {
        base.Init();
        register = Register.instance;
        properties = Register.instance.propertiesSphericalAiming;
        fireRate = properties.fireRate;
        timer = 0;
        playerTr = register.player.transform;
        bulletPool = PoolManager.instance.pooledBulletClass["SphericalAimingBullet"];
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
            bullet.transform.rotation = enemy.shooterTransform.rotation;
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
            GameObject bullet = bulletPool.GetpooledBullet();
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.shooterTransform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

}
