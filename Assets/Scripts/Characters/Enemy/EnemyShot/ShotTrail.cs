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
        //properties = Register.instance. propertiesTrail;
        //prefab = properties.trailPrefab;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (enemy.canShoot && !trail)
        {
            trail = PoolManager.instance.GetpooledBullet(PoolManager.instance.bulletTrailPool);
            trail.transform.position = enemy.bulletSpawnpoint.position;
            trail.transform.rotation = Quaternion.Inverse(enemy.transform.rotation);
            trail.SetActive(true);
            trail.transform.SetParent(enemy.bulletSpawnpoint);
            enemy.canShoot = false;
            PoolManager.instance.bulletTrailPool.index++;
        }
        if (PoolManager.instance.bulletTrailPool.index >= PoolManager.instance.pooledBulletAmount)
        {
            PoolManager.instance.bulletTrailPool.index = 0;
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        /*if (enemy.canShoot && !trail)
        {
            trail = //Object.Instantiate(prefab, enemy.bulletSpawnpoint.position, Quaternion.Inverse(enemy.transform.rotation));
            trail.transform.SetParent(enemy.bulletSpawnpoint);
            enemy.canShoot = false;
        }*/
    }

}
