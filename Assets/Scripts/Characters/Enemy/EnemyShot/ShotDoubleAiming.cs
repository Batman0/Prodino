using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDoubleAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    private GameObject sidescrollPrefab;
    private GameObject topdownPrefab;
    private PropertiesDoubleAiming properties;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesDoubleAiming;
        sidescrollPrefab = properties.sidescrollBulletPrefab;
        topdownPrefab = properties.topdownBulletPrefab;
        fireRate = properties.fireRate;
        timer = 0;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = Object.Instantiate(sidescrollPrefab, enemy.bulletSpawnpoint.position, enemy.transform.rotation);
            GameObject secondBullet = Object.Instantiate(sidescrollPrefab, enemy.bulletSpawnpointOther.position, Quaternion.Inverse(enemy.transform.rotation));
            timer = 0.0f;
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = Object.Instantiate(topdownPrefab, enemy.bulletSpawnpoint.position, enemy.transform.rotation);
            GameObject secondBullet = Object.Instantiate(topdownPrefab, enemy.bulletSpawnpointOther.position, enemy.transform.rotation);
            secondBullet.tag = "EnemyBulletInverse";
            timer = 0.0f;
        }
    }

}
