using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehaviour : EnemyBehaviour
{

    [Header("Movement/Shot")]
    private PropertiesTrail properties;
    private bool canShoot;

    [Header("Movement")]
    private float backSpeed;
    private float rotationSpeed;
    private float movementDuration;
    private float waitingTimer;
    private float doneRotation;
    private Transform enemyTransform;

    [Header("Shot")]
    private PoolManager.PoolBullet bulletPool;
    private GameObject trail;
    private int indexOfBullet;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        properties = Register.instance.propertiesTrail;
        speed = properties.xSpeed;
        backSpeed = properties.xReturnSpeed;
        rotationSpeed = properties.rotationSpeed;
        movementDuration = properties.movementDuration;
        waitingTimer = 0;
        doneRotation = 0;
        enemyTransform = enemy.transform.GetChild(0);

        bulletPool = PoolManager.instance.pooledBulletClass["TrailBullet"];
    }

    public override void Move()
    {
        if (waitingTimer < movementDuration && doneRotation == 0)
        {
            MoveForward(enemyInstance.transform, speed);
            waitingTimer += Time.deltaTime;
        }
        else if (waitingTimer > 0.0f && doneRotation >= 180)
        {
            MoveForward(enemyInstance.transform, -backSpeed);
            waitingTimer -= Time.deltaTime;
        }
        else if (waitingTimer <= 0.0f && doneRotation >= 180)
        {
            enemyInstance.gameObject.SetActive(false);
        }
        else
        {
            if (doneRotation < 180)
            {
                //if (enemy.sideCollider.enabled)
                //{
                //    enemy.sideCollider.enabled = false;
                //    enemy.topCollider.enabled = true;
                //}
                enemyTransform.Rotate(Vector3.up, rotationSpeed);
                doneRotation += rotationSpeed;
            }
            if (doneRotation >= 180 && !canShoot)
            {
                canShoot = true;
                //if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && !enemy.sideCollider.enabled)
                //{
                //    enemy.sideCollider.enabled = true;
                //    enemy.topCollider.enabled = false;
                //}
            }
        }
    }

    private void MoveForward(Transform transform, float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    public override void Shoot()
    {
        if (canShoot && !trail)
        {
            trail = bulletPool.GetpooledBullet();
            trail.transform.position = enemyInstance.bulletSpawnpoint.position;
            trail.transform.rotation = Quaternion.Inverse(enemyInstance.transform.rotation);
            trail.SetActive(true);
            trail.transform.SetParent(enemyInstance.bulletSpawnpoint);
            canShoot = false;
        }
    }
}
