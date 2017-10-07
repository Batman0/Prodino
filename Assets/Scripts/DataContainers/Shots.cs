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

    public static void ShootLaser(GameObject prefab, Transform bulletSpawnpoint, Transform rotTransform, float width, float height)
    {
        GameObject laser = Object.Instantiate(prefab, bulletSpawnpoint.position, rotTransform.rotation) as GameObject;
        laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
        laser.transform.SetParent(bulletSpawnpoint.parent);
    }

    /*public static void ShootAiming(GameObject prefab,Transform bulletSpawnpoint, Transform rotTransform)
    {
       
    }*/

    public static void ShootTrail()
    {

    }

    public static void ShootBomb(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    {
        GameObject bomb = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation);
    }

    public static void Shoot(ShotType shotType, Properties properties, ref float timer, ref bool canShoot, ref bool rotateRight, Transform spawnPoint, Transform rotTransform, Transform transform)
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
                            ShootLaser(properties.enemyLaserPrefab, spawnPoint, rotTransform, properties.l_LaserDepth, properties.l_LaserHeight);
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
                if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {    
                    rotTransform.LookAt(Register.instance.player.transform);
                    
                    
                    if (timer < properties.sa_FireRate)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    { 
                        ShootForward(properties.enemyBulletPrefab, spawnPoint, rotTransform);
                        timer = 0.0f;
                    }
                }
                else
                {
                    if(Register.instance.player.transform.position.x >= transform.position.x)
                    {
                        if(rotateRight)
                        {
                            rotTransform.RotateAround(transform.position, Vector3.forward, 180);
                            rotateRight = false;
                        }
                    }
                    else
                    {
                        if(!rotateRight)
                        {
                            rotTransform.RotateAround(transform.position, Vector3.forward, 180);
                            rotateRight = true;
                        }
                    }
                }             
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
