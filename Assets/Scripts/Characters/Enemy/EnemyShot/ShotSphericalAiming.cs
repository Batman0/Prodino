using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSphericalAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    //private GameObject prefab;
    private PropertiesSphericalAiming properties;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesSphericalAiming;
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletSphericalAimingPool, ref PoolManager.instance.sphericalAimingBulletAmount);
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletSphericalAimingPool, ref PoolManager.instance.sphericalAimingBulletAmount);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.shooterTransform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

}
