using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootType
{
    DEFAULT,
    LASER,
    TRAIL,
    BOMB
}
public static class Shoots
{
    public static void straightShoot(Transform bulletSpawnpoint, GameObject bulletPrefab,Transform enemyTransform)
    {
        GameObject bullet = Object.Instantiate(bulletPrefab, bulletSpawnpoint.position, enemyTransform.rotation) as GameObject;
        //bullet.layer = layer;
    }

    public static void laserShoot()
    {

    }

    public static void trailShoot()
    {

    }
	
    public static void bombShoot()
    {

    }
}
