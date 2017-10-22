using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLaserDiagonal : EnemyMovement
{

    private float sidescrollXSpeed;
    private float destructionMargin;
    private float amplitude;
    private float length;
    private float height;
    private float time;
    private PropertiesLaserDiagonal properties;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        properties = Register.instance.propertiesLaserDiagonal;
        speed = properties.xSpeed;
        sidescrollXSpeed = enemy.isRight ? -speed : speed;
        destructionMargin = properties.destructionMargin;
        amplitude = properties.amplitude;
        length = properties.waveLenght;
        height = enemy.transform.position.y;
        time = 0;
    }

    public override void MoveSidescroll(Enemy enemy)
    {
        enemy.transform.position = new Vector3(sidescrollXSpeed * Time.deltaTime + enemy.transform.position.x, 1 - (2 / Mathf.PI) * Mathf.Acos(Mathf.Cos(length * time * Mathf.PI / 2)) * amplitude + height, enemy.transform.position.z);
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

    public override void MoveTopdown(Enemy enemy)
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

}
