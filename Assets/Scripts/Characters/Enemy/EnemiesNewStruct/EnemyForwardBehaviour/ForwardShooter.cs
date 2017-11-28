using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShooter : EnemyForward
{
    private new PropertiesForwardShooter property;

    [Header("Shot")]
    private float fireRate;


    public override void InitEnemy()
    {
        base.InitEnemy();
        enemyLives = property.lives;
        speed = property.xSpeed;
        enemyLives = property.lives;
        destructionMargin = property.destructionMargin;
        fireRate = property.fireRate;
    }

    public override void Update()
    {
		base.Update ();
        Shoot();
    }

    public override void Shoot()
    {
        base.Shoot();
        if(CheckCondition())
        {
            if (timer < fireRate)
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
    }
    public override void SetProperty(ScriptableObject _property)
    {
        property = (PropertiesForwardShooter) _property;
        InitEnemy();
    }
}
