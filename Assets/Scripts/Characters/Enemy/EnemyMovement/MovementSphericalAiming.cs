using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSphericalAiming : EnemyMovement
{

    private bool moveForward;
    private float topdownXSpeed;
    private float zMovementSpeed;
    private float destructionMargin;
    private float forwardDistance;
    private float backDistance;
    private float targetPlayerDeltaDistance = 0.1f;
    private Vector3 topdownTarget;
    //private float amplitude;
    //private float length;
    //private float height;
    //private float time;
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
        forwardDistance = properties.forwardDistance;
        backDistance = properties.backDistance;
        topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.originalPos.z + forwardDistance);
        moveForward = true;
        //zMovementSpeed = moveForward ? Mathf.Abs(zMovementSpeed) : -Mathf.Abs(zMovementSpeed);
        //amplitude = properties.amplitude;
        //length = properties.waveLenght;
        //height = enemy.transform.position.z;
        //time = 0;
    }

    public override void MoveSidescroll(Enemy enemy)
    {
        enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                enemy.gameObject.SetActive(false);
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
        if(Vector3.Distance(enemy.transform.position, topdownTarget) > targetPlayerDeltaDistance)
        {
            topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, topdownTarget.z);
        }
        else
        {
            moveForward = !moveForward;
            zMovementSpeed = -zMovementSpeed;
            topdownTarget = moveForward ? new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.originalPos.z + forwardDistance) : new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.originalPos.z - backDistance);
        }

        enemy.transform.position = new Vector3(topdownXSpeed * Time.deltaTime + enemy.transform.position.x, enemy.transform.position.y, zMovementSpeed * Time.deltaTime + enemy.transform.position.z);
        //time += Time.deltaTime;

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
