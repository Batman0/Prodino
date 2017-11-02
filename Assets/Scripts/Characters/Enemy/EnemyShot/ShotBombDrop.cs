using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBombDrop : EnemyShot
{

    private float loadingTime;
    private float timer;
    private GameObject prefab;
    private PropertiesBombDrop properties;
    private PoolManager.PoolBullet bulletPool;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesBombDrop;
        loadingTime = properties.loadingTime;
        timer = 0;
        bulletPool = PoolManager.instance.pooledBulletClass["BombDropBullet"];
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (timer < loadingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = bulletPool.GetpooledBullet();
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        if (timer < loadingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = bulletPool.GetpooledBullet();
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

}
