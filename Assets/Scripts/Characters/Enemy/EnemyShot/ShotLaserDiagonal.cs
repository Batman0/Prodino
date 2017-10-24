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

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesLaserDiagonal;
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
                    laser = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletLaserPool, ref PoolManager.instance.laserBulletAmount);                   
                    laser.SetActive(true);
                    laser.transform.position = enemy.bulletSpawnpoint.position;
                    laser.transform.rotation = enemy.bulletSpawnpoint.parent.rotation;
                    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
                    laser.transform.SetParent(enemy.bulletSpawnpoint.parent);
                    //enemy.canShoot = false;
                }
                if (timer < waitingTime + loadingTime + shootingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    laser.SetActive(false);
                    timer = 0.0f;
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
                if (!enemy.canShoot && enemy.shotType != ShotType.TRAIL)
                {
                    enemy.canShoot = true;
                }
                if (enemy.canShoot)
                {
                    GameObject laser = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletLaserPool, ref PoolManager.instance.laserBulletAmount);
                    laser.SetActive(true);
                    PoolManager.instance.bulletLaserPool.index++;
                    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
                    laser.transform.SetParent(enemy.bulletSpawnpoint.parent);
                    enemy.canShoot = false;
                }

                if (timer < waitingTime + loadingTime + shootingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    laser.SetActive(false);
                    timer = 0.0f;
                    //canShoot = true;
                }
            }
        }
    }

}
