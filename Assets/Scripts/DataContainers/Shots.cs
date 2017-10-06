using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    FORWARDSHOOTER,
    FORWARD,
    LASERDIAGONAL,
    SPHERICALAIMING,
    BOMBDROP,
    TRAIL,
    DOUBLEAIMING
    
}
public static class Shots
{
    public static void ShootForward(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    {
        GameObject bullet = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation) as GameObject;
        //bullet.layer = layer;
    }

    public static GameObject ShootLaser(GameObject prefab, Transform bulletSpawnpoint, Transform rotTransform, float width, float height)
    {
        GameObject bullet = Object.Instantiate(prefab, bulletSpawnpoint.position, rotTransform.rotation) as GameObject;
        bullet.transform.localScale = new Vector3(width, height, bullet.transform.localScale.z);
        return bullet;
    }

    public static void ShootTrail()
    {

    }

    public static void ShootBomb(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    {
        GameObject bomb = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation);
    }

    public static void Shoot(ShotType shotType, Properties properties, ref float timer, ref bool canShoot, Transform spawnPoint, Transform rotTransform)
    {
        switch (shotType)
        {
            case ShotType.FORWARDSHOOTER:
                if (timer < properties.fs_FireRate)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    ShootForward(properties.enemyBulletPrefab, spawnPoint, rotTransform);
                    timer = 0.0f;
                }
                break;
            case ShotType.FORWARD:
                break;
            case ShotType.LASERDIAGONAL:
                if (timer < properties.l_WaitingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    if (timer < properties.l_WaitingTime + properties.l_LoadingTime)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        if (!canShoot)
                        {
                            canShoot = true;
                        }
                        if (canShoot)
                        {
                            GameObject laser = ShootLaser(properties.enemyLaserPrefab, spawnPoint, rotTransform, properties.l_LaserDepth, properties.l_LaserHeight);
                            laser.transform.SetParent(spawnPoint.parent);
                            canShoot = false;
                        }
                        if (timer < properties.l_WaitingTime + properties.l_LoadingTime + properties.l_ShootingTime)
                        {
                            timer += Time.deltaTime;
                        }
                        else
                        {
                            timer = 0.0f;
                            canShoot = true;
                        }
                    }
                }
                break;
            case ShotType.SPHERICALAIMING:
                break;
            case ShotType.BOMBDROP:
                if (timer < properties.bd_LoadingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    ShootBomb(properties.bombPrefab, spawnPoint, rotTransform);
                    timer = 0.0f;
                }
                break;
            case ShotType.TRAIL:
                break;
            case ShotType.DOUBLEAIMING:
                break;
        }
    }
}
