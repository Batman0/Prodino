using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : EnemyForward
{
    //private new PropertiesBombDrop property;
    

    [SerializeField]
    private float loadingTime;


    public override void InitEnemy()
    {
    //    base.InitEnemy();
    //    enemyLives = property.lives;
    //    speed = property.xSpeed;
    //    destructionMargin = property.destructionMargin;
    //    loadingTime = property.loadingTime;
    }

    // Update is called once per frame
    public override void Update ()
    {
		base.Update();
        Shoot();
	}

    public override void Shoot()
    {
        base.Shoot();


        if (fireRateTimer < loadingTime)
        {
            fireRateTimer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.pooledBulletClass[enemyName].GetpooledBullet();
            bullet.transform.position = bulletSpawnpoint.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            fireRateTimer = 0.0f;
        }
    }

    //public override void SetProperty(ScriptableObject _property)
    //{
    //    property = (PropertiesBombDrop)_property;
    //    InitEnemy();
    //}
}
