using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShooterBehaviour : EnemyBehaviour
{

    [Header("Common")]
    private PropertiesForwardShooter properties;

    [Header("Movement")]
    private float destructionMargin;

    [Header("Shot")]
    private float fireRate;
    private float timer;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        properties = Register.instance.propertiesForwardShooter;
        speed = properties.xSpeed;
        destructionMargin = properties.destructionMargin;

        fireRate = properties.fireRate;
        timer = properties.fireRate;
    }

    public override void Move()
    {
        enemyInstance.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemyInstance.isRight)
        {
            if (enemyInstance.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                enemyInstance.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemyInstance.transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                enemyInstance.gameObject.SetActive(false);
            }
        }
    }

    public override void Shoot()
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.pooledBulletClass["ForwardShooterBullet"].GetpooledBullet();
            bullet.transform.position = enemyInstance.bulletSpawnpoint.position;
            bullet.transform.rotation = enemyInstance.transform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

    //public override void ShootTopdown()
    //{
    //    base.ShootTopdown();
    //    if (timer < fireRate)
    //    {
    //        timer += Time.deltaTime;
    //    }
    //    else
    //    {
    //        GameObject bullet = PoolManager.instance.pooledBulletClass["ForwardShooterBullet"].GetpooledBullet();
    //        bullet.transform.position = enemyInstance.bulletSpawnpoint.position;
    //        bullet.transform.rotation = enemyInstance.transform.rotation;
    //        bullet.SetActive(true);
    //        timer = 0.0f;
    //    }
    //}

    //public override void MoveTopdown(Enemy enemy)
    //{
    //    enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
    //        {
    //            enemy.gameObject.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
    //        {
    //            enemy.gameObject.SetActive(false);
    //        }
    //    }
    //}

}
