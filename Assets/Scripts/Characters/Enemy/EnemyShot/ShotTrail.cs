using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrail : EnemyShot
{

    private GameObject prefab;
    private GameObject trail;
    private PropertiesTrail properties;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesTrail;
        prefab = properties.trailPrefab;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (enemy.canShoot && !trail)
        {
            trail = Object.Instantiate(prefab, enemy.bulletSpawnpoint.position, Quaternion.Inverse(enemy.transform.rotation));
            trail.transform.SetParent(enemy.bulletSpawnpoint);
            enemy.canShoot = false;
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        if (enemy.canShoot && !trail)
        {
            trail = Object.Instantiate(prefab, enemy.bulletSpawnpoint.position, Quaternion.Inverse(enemy.transform.rotation));
            trail.transform.SetParent(enemy.bulletSpawnpoint);
            enemy.canShoot = false;
        }
    }

}
