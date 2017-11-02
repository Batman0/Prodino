using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrail : EnemyShot
{

    //private GameObject prefab;
    private GameObject trail;
    private PropertiesTrail properties;
    private int indexOfBullet;

    public override void Init()
    {
        base.Init();
        properties = Register.instance. propertiesTrail;
        //prefab = properties.trailPrefab;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (enemy.canShoot && !trail)
        {
            trail = PoolManager.instance.pooledBulletClass["TrailBullet"].GetpooledBullet();
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
            trail = PoolManager.instance.pooledBulletClass["TrailBullet"].GetpooledBullet();
            trail.transform.position = enemy.bulletSpawnpoint.position;
            trail.transform.rotation = Quaternion.Inverse(enemy.transform.rotation);
            trail.SetActive(true);
            trail.transform.SetParent(enemy.bulletSpawnpoint);
            enemy.canShoot = false;
        }
    }

}
