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

    public static void bombShoot(Transform bulletSpawnpoint, GameObject bombPrefab, Transform enemyTransform)
    {
        GameObject bomb = Object.Instantiate(bombPrefab, bulletSpawnpoint.position, enemyTransform.rotation);
    }
}
