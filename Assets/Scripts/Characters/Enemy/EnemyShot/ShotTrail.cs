using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrail : EnemyShot
{

    private PoolManager.PoolBullet bulletPool;
    private GameObject trail;
    private PropertiesTrail properties;
    private int indexOfBullet;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesTrail;
        bulletPool = PoolManager.instance.pooledBulletClass["TrailBullet"];
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (enemy.canShoot && !trail)
        {
            trail = bulletPool.GetpooledBullet();
            trail.transform.position = enemy.bulletSpawnpoint.position;
            trail.transform.rotation = Quaternion.Inverse(enemy.transform.rotation);
            trail.SetActive(true);
            trail.transform.SetParent(enemy.bulletSpawnpoint);
            enemy.canShoot = false;
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        if (enemy.canShoot && !trail)
        {
            trail = bulletPool.GetpooledBullet();
            trail.transform.position = enemy.bulletSpawnpoint.position;
            trail.transform.rotation = Quaternion.Inverse(enemy.transform.rotation);
            trail.SetActive(true);
            trail.transform.SetParent(enemy.bulletSpawnpoint);
            enemy.canShoot = false;
        }
    }

}
