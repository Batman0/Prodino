using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDoubleAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    private PropertiesDoubleAiming properties;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesDoubleAiming;
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
            GameObject bullet = PoolManager.instance.pooledBulletClass["DoubleAimingBullet"].GetpooledBullet();
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            GameObject secondBullet = PoolManager.instance.pooledBulletClass["DoubleAimingBullet"].GetpooledBullet();
            secondBullet.transform.position = enemy.bulletSpawnpointOther.position;
            secondBullet.transform.rotation = Quaternion.Inverse(enemy.transform.rotation);
            secondBullet.SetActive(true);
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
            GameObject bullet = PoolManager.instance.pooledBulletClass["DoubleAimingSinusoideBullet"].GetpooledBullet();
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            GameObject secondBullet = PoolManager.instance.pooledBulletClass["DoubleAimingSinusoideBullet"].GetpooledBullet();
            secondBullet.tag = "EnemyBulletInverse";
            secondBullet.transform.position = enemy.bulletSpawnpointOther.position;
            secondBullet.transform.rotation = enemy.transform.rotation;
            secondBullet.SetActive(true);
            timer = 0.0f;
        }
    }
}
