using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : EnemyForward
{
    private new PropertiesBombDrop property;
    

    [Header("Shot")]
    private float loadingTime;


    public override void InitEnemy()
    {
        base.InitEnemy();
        enemyLives = property.lives;
        speed = property.xSpeed;
        destructionMargin = property.destructionMargin;
        loadingTime = property.loadingTime;
    }

    // Update is called once per frame
    public override void Update ()
    {
		base.Update ();
        Shoot();
	}

    public override void Shoot()
    {
        base.Shoot();


        if (timer < loadingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.pooledBulletClass[property.enemyName].GetpooledBullet();
            bullet.transform.position = bulletSpawnpoint.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

    public override void SetProperty(ScriptableObject _property)
    {
        property = (PropertiesBombDrop)_property;
        InitEnemy();
    }
}
