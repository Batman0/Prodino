using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLaserDiagonal : EnemyMovement
{

    private bool moveUp;
    //private float sidescrollXSpeed;
    private float yMovementSpeed;
    private float yMovementSpeedShooting;
    private float destructionMargin;
    private float upDistance;
    private float downDistance;
    private float targetPlayerDeltaDistance;
    private float xMin;
    private float xMax;
    private Vector3 sidescrollTarget;
    //private float amplitude;
    //private float length;
    //private float height;
    //private float time;
    private PropertiesLaserDiagonal properties;
    private Register register;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        properties = register.propertiesLaserDiagonal;
        speed = enemy.isRight ? -properties.xSpeed : properties.xSpeed;
        //sidescrollXSpeed = enemy.isRight ? -speed : speed;
        moveUp = true;
        yMovementSpeed = properties.yMovementSpeed;
        yMovementSpeedShooting = properties.yMovementSpeedShooting;
        targetPlayerDeltaDistance = Mathf.Max(yMovementSpeed, yMovementSpeedShooting) / 10;
        destructionMargin = properties.destructionMargin;
        upDistance = properties.upDistance;
        downDistance = properties.downDistance;
        sidescrollTarget = new Vector3(enemy.transform.position.x, enemy.originalPos.y + upDistance, enemy.transform.position.z);
        xMin = register.xMin;
        xMax = register.xMax;
    }

    public override void Movement(Enemy enemy)
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

        enemy.transform.position = new Vector3(speed * Time.deltaTime + enemy.transform.position.x, (!enemy.isShooting ? yMovementSpeed : yMovementSpeedShooting) * Time.deltaTime + enemy.transform.position.y, enemy.transform.position.z);

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= xMin - destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemy.transform.position.x >= xMax + destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    //public override void MoveTopdown(Enemy enemy)
    //{
    //    enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
    //        {
    //            enemy.gameObject.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
    //        {
    //            enemy.gameObject.SetActive(false);
    //        }
    //    }
    //}

}
