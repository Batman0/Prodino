using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShooter : EnemyForward
{
    //private new PropertiesForwardShooter property;

    [SerializeField]
    private float fireRate;


    public override void InitEnemy()
    {
    //    base.InitEnemy();
    //    enemyLives = property.lives;
    //    speed = property.xSpeed;
    //    enemyLives = property.lives;
    //    destructionMargin = property.destructionMargin;
    //    fireRate = property.fireRate;
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
            if (fireRateTimer < fireRate)
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
    }
    //public override void SetProperty(ScriptableObject _property)
    //{
    //    property = (PropertiesForwardShooter) _property;
    //    InitEnemy();
    //}
}
