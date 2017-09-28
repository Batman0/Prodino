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
}
