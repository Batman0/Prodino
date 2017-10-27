using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSphericalAiming : EnemyMovement
{

    private float topdownXSpeed;
    private float zMovementSpeed;
    private float destructionMargin;
    private float amplitude;
    private float length;
    private float height;
    private float time;
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
        zMovementSpeed = properties.zMovementSpeed;
        destructionMargin = properties.destructionMargin;
        amplitude = properties.amplitude;
        length = properties.waveLenght;
        height = enemy.transform.position.z;
        time = 0;
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
    }

    public override void MoveTopdown(Enemy enemy)
    {
        enemy.transform.position = new Vector3(topdownXSpeed * Time.deltaTime + enemy.transform.position.x, enemy.transform.position.y, 1 - (2 / Mathf.PI) * Mathf.Acos(Mathf.Cos(zMovementSpeed * time * Mathf.PI / 2)) * amplitude + height);
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
    }
}
