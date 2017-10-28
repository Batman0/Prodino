using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLaserDiagonal : EnemyMovement
{

    private bool moveUp;
    private float sidescrollXSpeed;
    private float yMovementSpeed;
    private float yMovementSpeedShooting;
    private float destructionMargin;
    private float upDistance;
    private float downDistance;
    private float targetPlayerDeltaDistance;
    private Vector3 sidescrollTarget;
    //private float amplitude;
    //private float length;
    //private float height;
    //private float time;
    private PropertiesLaserDiagonal properties;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        properties = Register.instance.propertiesLaserDiagonal;
        speed = properties.xSpeed;
        sidescrollXSpeed = enemy.isRight ? -speed : speed;
        moveUp = true;
        yMovementSpeed = properties.yMovementSpeed;
        yMovementSpeedShooting = properties.yMovementSpeedShooting;
        targetPlayerDeltaDistance = Mathf.Max(yMovementSpeed, yMovementSpeedShooting) / 10;
        destructionMargin = properties.destructionMargin;
        upDistance = properties.upDistance;
        downDistance = properties.downDistance;
        sidescrollTarget = new Vector3(enemy.transform.position.x, enemy.originalPos.y + upDistance, enemy.transform.position.z);
        //amplitude = properties.amplitude;
        //length = properties.waveLenght;
        //height = enemy.transform.position.y;
        //time = 0;
    }

    public override void MoveSidescroll(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, sidescrollTarget) > targetPlayerDeltaDistance)
        {
            sidescrollTarget = new Vector3(enemy.transform.position.x, sidescrollTarget.y, enemy.transform.position.z);
        }
        else
        {
            moveUp = !moveUp;
            yMovementSpeed = moveUp ? Mathf.Abs(yMovementSpeed) : -Mathf.Abs(yMovementSpeed);
            yMovementSpeedShooting = moveUp ? Mathf.Abs(yMovementSpeedShooting) : -Mathf.Abs(yMovementSpeedShooting);
            sidescrollTarget = moveUp ? new Vector3(enemy.transform.position.x, enemy.originalPos.y + upDistance, enemy.transform.position.z) : new Vector3(enemy.transform.position.x, enemy.originalPos.y - downDistance, enemy.transform.position.z);
        }

        enemy.transform.position = new Vector3(sidescrollXSpeed * Time.deltaTime + enemy.transform.position.x, (!enemy.isShooting ? yMovementSpeed : yMovementSpeedShooting) * Time.deltaTime + enemy.transform.position.y, enemy.transform.position.z);

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
