using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSphericalAiming : EnemyShot
{

    private float fireRate;
    private float timer;
    private float rotationDeadZone;
    private float rotationSpeed;
    private Quaternion barrelStartRotation;
    private Quaternion barrelInverseRotation;
    private Transform playerTr;
    private Collider playerCl;
    //private GameObject prefab;
    private Register register;
    private PropertiesSphericalAiming properties;

    public override void Init()
    {
        base.Init();
        register = Register.instance;
        properties = Register.instance.propertiesSphericalAiming;
        rotationDeadZone = properties.rotationDeadZone;
        rotationSpeed = properties.rotationSpeed;
        fireRate = properties.fireRate;
        timer = 0;
        barrelStartRotation = Quaternion.identity;
        playerTr = register.player.transform;
        playerCl = register.player.sideBodyCollider;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletSphericalAimingPool, PoolManager.instance.sphericalAimingBulletAmount);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.shooterTransform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
        Vector3 playerTransform = new Vector3(playerCl.bounds.center.x - enemy.transform.position.x, playerCl.bounds.center.y - enemy.transform.position.y, 0);
        Vector3 barrelSpawnpointTransform = new Vector3(enemy.bulletSpawnpoint.position.x - enemy.transform.position.x, enemy.bulletSpawnpoint.position.y - enemy.transform.position.y, 0);
        float angle = Vector3.Angle(barrelSpawnpointTransform, playerTransform);
        Vector3 cross = Vector3.Cross(playerTransform, barrelSpawnpointTransform);

        if (angle > rotationDeadZone)
        {
            if (cross.z >= 0)
            {
                enemy.shooterTransform.RotateAround(enemy.transform.position, Vector3.forward, -rotationSpeed);
            }
            else
            {
                enemy.shooterTransform.RotateAround(enemy.transform.position, Vector3.forward, rotationSpeed);
            }
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
            GameObject bullet = PoolManager.instance.GetpooledBullet(ref PoolManager.instance.bulletSphericalAimingPool, PoolManager.instance.sphericalAimingBulletAmount);
            bullet.transform.position = enemy.bulletSpawnpoint.position;
            bullet.transform.rotation = enemy.shooterTransform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }

        if (barrelStartRotation != enemy.shooterTransform.rotation)
        {
            barrelStartRotation = enemy.shooterTransform.rotation;
            barrelInverseRotation = Quaternion.Inverse(barrelStartRotation);
        }

        if (enemy.rotateRight && enemy.shooterTransform.rotation != barrelStartRotation)
        {
            enemy.shooterTransform.rotation = barrelStartRotation;
        }
        else if (!enemy.rotateRight && enemy.shooterTransform.rotation != barrelInverseRotation)
        {
            enemy.shooterTransform.rotation = barrelInverseRotation;
        }

        if (playerTr.position.x >= enemy.transform.position.x)
        {
            if (enemy.rotateRight)
            {
                enemy.shooterTransform.rotation = barrelInverseRotation;
                enemy.rotateRight = false;
            }
        }
        else
        {
            if (!enemy.rotateRight)
            {
                enemy.shooterTransform.rotation = barrelStartRotation;
                enemy.rotateRight = true;
            }
        }
    }

}
