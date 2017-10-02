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

    public static void laserShoot(Transform bulletSpawnpoint, float width, RaycastHit hit, float timeVisibleLine)
    {
        Debug.DrawRay(bulletSpawnpoint.position, bulletSpawnpoint.right, Color.red, timeVisibleLine);

        if (Physics.Raycast(bulletSpawnpoint.position, bulletSpawnpoint.right, out hit, width))
        {
            Object.Destroy(hit.collider.gameObject);
        }
    }

    public static void trailShoot()
    {

    }

    public static void bombShoot(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    {
        GameObject bomb = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation);
    }

    public static void Shoot(ShootType shootType, EnemyProperties enemyProperties, ref float timer, Transform spawnPoint, Transform rotTransform)
    {
        switch (shootType)
        {
            case ShootType.DEFAULT:
                if (timer < enemyProperties.d_RatioOfFire)
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
                //Debug.Log("Laser");
                break;
            case ShootType.TRAIL:
                break;
            case ShootType.BOMB:
                if (timer < enemyProperties.b_SpawnTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    bombShoot(enemyProperties.b_Bullet, spawnPoint, rotTransform);
                    timer = 0.0f;
                }
                break;
            case ShootType.NOFIRE:
                break;
        }
    }
}
