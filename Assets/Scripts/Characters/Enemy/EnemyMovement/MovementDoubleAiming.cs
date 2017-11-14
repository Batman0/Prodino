using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDoubleAiming : EnemyMovement
{

    //private bool moveForward;
    private float topdownXSpeed;
    private float zMovementSpeed;
    private float destructionMargin;
    private float amplitude;
    //private float backDistance;
    private float targetPlayerDeltaDistance = 0.1f;
    private float xMin;
    private float xMax;
    private Vector3 originalPos;
    private Vector3 topdownTarget;
    //private float amplitude;
    //private float length;
    //private float height;
    //private float time;
    private PropertiesDoubleAiming properties;
    private Register register;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        originalPos = enemy.transform.position;
        properties = register.propertiesDoubleAiming;
        speed = enemy.isRight ? -properties.xSpeed : properties.xSpeed;
        //topdownXSpeed = enemy.isRight ? -speed : speed;
        zMovementSpeed = properties.zMovementSpeed;
        destructionMargin = properties.destructionMargin;
        amplitude = properties.amplitude;
        //backDistance = properties.backDistance;
        topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, originalPos.z + amplitude);
        //moveForward = true;
        xMin = register.xMin;
        xMax = register.xMax;
        //amplitude = properties.amplitude;
        //length = properties.waveLenght;
        //height = enemy.transform.position.z;
        //time = 0;
    }

    public override void Movement(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, topdownTarget) > targetPlayerDeltaDistance)
        {
            topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, topdownTarget.z);
        }
        else
        {
            //moveForward = !moveForward;
            zMovementSpeed = -zMovementSpeed;
            amplitude = -amplitude;
            topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, originalPos.z + amplitude);
        }

        enemy.transform.position = new Vector3(speed * Time.deltaTime + enemy.transform.position.x, enemy.transform.position.y, zMovementSpeed * Time.deltaTime + enemy.transform.position.z);

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
