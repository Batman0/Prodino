using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSphericlaAiming : EnemyMovement
{

    private float topdownXSpeed;
    private float rotationSpeed;
    private float rotationDeadZone;
    private float destructionMargin;
    private float amplitude;
    private float length;
    private float height;
    private float time;
    private Quaternion barrelStartRotation;
    private Quaternion barrelInverseRotation;
    private Transform playerTr;
    private Register register;
    private PropertiesSphericalAiming properties;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        playerTr = register.player.transform;
        properties = register.propertiesSphericalAiming;
        speed = properties.xSpeed;
        topdownXSpeed = enemy.isRight ? -speed : speed;
        rotationSpeed = properties.rotationSpeed;
        rotationDeadZone = properties.rotationDeadZone;
        destructionMargin = properties.destructionMargin;
        amplitude = properties.amplitude;
        length = properties.waveLenght;
        height = enemy.transform.position.z;
        time = 0;
        barrelStartRotation = enemy.shooterTransform.rotation;
        barrelInverseRotation = Quaternion.Inverse(barrelStartRotation);
    }

    public override void MoveSidescroll(Enemy enemy)
    {
        enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                Object.Destroy(enemy.gameObject);
            }
        }
        else
        {
            if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                Object.Destroy(enemy.gameObject);
            }
        }

        Vector3 playerTransform = new Vector3(playerTr.position.x - enemy.transform.position.x, playerTr.position.y + 2 - enemy.transform.position.y, 0);
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

    public override void MoveTopdown(Enemy enemy)
    {
        enemy.transform.position = new Vector3(topdownXSpeed * Time.deltaTime + enemy.transform.position.x, enemy.transform.position.y, 1 - (2 / Mathf.PI) * Mathf.Acos(Mathf.Cos(length * time * Mathf.PI / 2)) * amplitude + height);
        time += Time.deltaTime;

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                Object.Destroy(enemy.gameObject);
            }
        }
        else
        {
            if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                Object.Destroy(enemy.gameObject);
            }
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
