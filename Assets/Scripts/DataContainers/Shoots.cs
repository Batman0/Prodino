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
    public static GameObject straightShoot(GameObject bulletPrefab, Transform bulletSpawnpoint, Transform rotTransform)
    {
        GameObject bullet = Object.Instantiate(bulletPrefab, bulletSpawnpoint.position, rotTransform.rotation) as GameObject;
        return bullet;
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
