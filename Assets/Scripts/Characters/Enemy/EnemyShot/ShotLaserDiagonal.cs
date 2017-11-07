using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLaserDiagonal : EnemyShot
{
    private float waitingTime;
    private float loadingTime;
    private float shootingTime;
    private float width;
    private float height;
    private float timer;
    private GameObject laser;
    private PropertiesLaserDiagonal properties;
    private PoolManager.PoolBullet bulletPool;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesLaserDiagonal;
        bulletPool = PoolManager.instance.pooledBulletClass["LaserBullet"];
        waitingTime = properties.waitingTime;
        loadingTime = properties.loadingTime;
        shootingTime = properties.shootingTime;
        width = properties.laserWidth;
        height = properties.laserHeight;
        timer = 0;
        //prefab = properties.laserPrefab;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (timer < waitingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (timer < waitingTime + loadingTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (!laser)
                {
                    laser = bulletPool.GetpooledBullet();
                    laser.SetActive(true);
                    laser.transform.SetParent(enemy.bulletSpawnpoint.parent);
                    laser.transform.position = enemy.bulletSpawnpoint.position;
                    laser.transform.rotation = enemy.transform.rotation;
                    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
                    //enemy.canShoot = false;
                    enemy.isShooting = true;
                }
                if (timer < waitingTime + loadingTime + shootingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    laser.SetActive(false);
                    laser = null;
                    timer = 0.0f;
                    enemy.isShooting = false;
                    //canShoot = true;
                }
            }
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        if (timer < waitingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (timer < waitingTime + loadingTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (!laser)
                {
                    laser = bulletPool.GetpooledBullet();
                    laser.SetActive(true);
                    laser.transform.SetParent(enemy.bulletSpawnpoint.parent);
                    laser.transform.position = enemy.bulletSpawnpoint.position;
                    laser.transform.rotation = enemy.transform.rotation;
                    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
                    //enemy.canShoot = false;
                    enemy.isShooting = true;
                }
                if (timer < waitingTime + loadingTime + shootingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    laser.SetActive(false);
                    laser = null;
                    timer = 0.0f;
                    enemy.isShooting = false;
                    //canShoot = true;
                }
            }
        }
    }

}
