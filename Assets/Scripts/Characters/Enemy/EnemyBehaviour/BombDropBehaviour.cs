using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropBehaviour : EnemyBehaviour
{

    [Header("Common")]
    private PropertiesBombDrop properties;

    [Header("Movement")]
    private float destructionMargin;
    private float xMin;
    private float xMax;
    private Register register;

    [Header("Shot")]
    private float loadingTime;
    private float timer;
    private GameObject prefab;
    private PoolManager.PoolBullet bulletPool;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        properties = register.propertiesBombDrop;
        speed = properties.xSpeed;
        destructionMargin = properties.destructionMargin;
        xMin = register.xMin;
        xMax = register.xMax;

        loadingTime = properties.loadingTime;
        timer = 0;
        bulletPool = PoolManager.instance.pooledBulletClass["BombDropBullet"];
    }

    public override void Move()
    {
        enemyInstance.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemyInstance.isRight)
        {
            if (enemyInstance.transform.position.x <= xMin - destructionMargin)
            {
                enemyInstance.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemyInstance.transform.position.x >= xMax + destructionMargin)
            {
                enemyInstance.gameObject.SetActive(false);
            }
        }
    }

    public override void Shoot()
    {
        if (timer < loadingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = bulletPool.GetpooledBullet();
            bullet.transform.position = enemyInstance.bulletSpawnpoint.position;
            bullet.transform.rotation = enemyInstance.transform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

    //public override void ShootTopdown()
    //{
    //    base.ShootTopdown();
    //    if (timer < loadingTime)
    //    {
    //        timer += Time.deltaTime;
    //    }
    //    else
    //    {
    //        GameObject bullet = bulletPool.GetpooledBullet();
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
