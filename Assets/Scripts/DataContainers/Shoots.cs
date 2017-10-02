using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootType
{
    DEFAULT,
    LASER,
    TRAIL,
    BOMB,
    NOFIRE
}
public static class Shoots
{
    public static void straightShoot(GameObject prefab, Transform sSpawnpoint, Transform rotTransform)
    {
        GameObject bullet = Object.Instantiate(prefab, sSpawnpoint.position, rotTransform.rotation) as GameObject;
        //bullet.layer = layer;
    }

    public static GameObject laserShoot(Transform bulletSpawnpoint, Transform rotTransform, float width, float height)
    {
        GameObject bullet = Object.Instantiate(Register.instance.enemyLaser, bulletSpawnpoint.position, rotTransform.rotation) as GameObject;
        bullet.transform.localScale = new Vector3(width, height, bullet.transform.localScale.z);
        return bullet;
    }

    public static void trailShoot()
    {

    }

    public static void bombShoot(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    {
        GameObject bomb = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation);
    }

    public static void Shoot(ShootType shootType, Properties properties, ref float timer, ref bool canShoot, Transform spawnPoint, Transform rotTransform)
    {
        switch (shootType)
        {
            case ShootType.DEFAULT:
                if (timer < properties.d_RatioOfFire)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    straightShoot(Register.instance.enemyBullet, spawnPoint, rotTransform);
                    timer = 0.0f;
                }
                break;
            case ShootType.LASER:
                if (timer < properties.l_RatioOfFire)
                {
                    timer += Time.deltaTime;
                    if (!canShoot)
                    {
                        canShoot = true;
                    }
                }
                else
                {
                    if (canShoot)
                    {
                        GameObject laser = laserShoot(spawnPoint, rotTransform, properties.l_Width, properties.l_Height);
                        laser.transform.SetParent(spawnPoint.parent);
                        canShoot = false;
                    }
                    if (timer < properties.l_RatioOfFire + properties.l_Lifetime)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        timer = 0.0f;
                        canShoot = true;
                    }
                }
                break;
            case ShootType.TRAIL:
                break;
            case ShootType.BOMB:
                if (timer < properties.b_SpawnTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    bombShoot(properties.b_Bullet, spawnPoint, rotTransform);
                    timer = 0.0f;
                }
                break;
            case ShootType.NOFIRE:
                break;
        }
    }
}
