using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSphericalAiming : EnemyMovement
{

    private bool moveForward;
    //private float topdownXSpeed;
    private float zMovementSpeed;
    private float destructionMargin;
    private float forwardDistance;
    private float backDistance;
    private float targetPlayerDeltaDistance = 0.1f;
    private float rotationDeadZone;
    private float rotationSpeed;
    private float xMin;
    private float xMax;
    private Vector3 topdownTarget;
    private Quaternion shooterTransformStartRotation;
    private Quaternion shooterTransformInverseRotation;
    private Collider playerCl;
    //private float amplitude;
    //private float length;
    //private float height;
    //private float time;
    private Transform playerTr;
    private Register register;
    private GameManager gameManager;
    private PropertiesSphericalAiming properties;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        gameManager = GameManager.instance;
        playerTr = register.player.transform;
        properties = register.propertiesSphericalAiming;
        speed = enemy.isRight ? -properties.xSpeed : properties.xSpeed;
        //topdownXSpeed = enemy.isRight ? -speed : speed;
        zMovementSpeed = properties.zMovementSpeed;
        destructionMargin = properties.destructionMargin;
        forwardDistance = properties.forwardDistance;
        backDistance = properties.backDistance;
        topdownTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.originalPos.z + forwardDistance);
        moveForward = true;
        playerCl = register.player.sideBodyCollider;
        rotationDeadZone = properties.rotationDeadZone;
        rotationSpeed = properties.rotationSpeed;
        shooterTransformStartRotation = enemy.shooterTransform.rotation;
        shooterTransformInverseRotation = Quaternion.Inverse(shooterTransformStartRotation);
        xMin = register.xMin;
        xMax = register.xMax;
        //zMovementSpeed = moveForward ? Mathf.Abs(zMovementSpeed) : -Mathf.Abs(zMovementSpeed);
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
            moveForward = !moveForward;
            zMovementSpeed = -zMovementSpeed;
            topdownTarget = moveForward ? new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.originalPos.z + forwardDistance) : new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.originalPos.z - backDistance);
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
                Object.Destroy(enemy.gameObject);
            }
        }

        if (gameManager.currentGameMode == GameMode.SIDESCROLL)
        {
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
        else
        {
            //if (enemy.rotateRight && enemy.shooterTransform.rotation != barrelStartRotation)
            //{
            //    enemy.shooterTransform.rotation = barrelStartRotation;
            //}
            //else if (!enemy.rotateRight && enemy.shooterTransform.rotation != barrelInverseRotation)
            //{
            //    enemy.shooterTransform.rotation = barrelInverseRotation;
            //}

            if (enemy.transform.position.x < playerTr.position.x)
            {
                if (enemy.barrelRight)
                {
                    Debug.Log("right");
                    enemy.shooterTransform.rotation = enemy.isRight ? shooterTransformInverseRotation : shooterTransformStartRotation;
                    enemy.barrelRight = false;
                }
            }
            else
            {
                if (!enemy.barrelRight)
                {
                    Debug.Log("LERFT");
                    enemy.shooterTransform.rotation = enemy.isRight ? shooterTransformStartRotation : shooterTransformInverseRotation;
                    enemy.barrelRight = true;
                }
            }
        }
    }

    //public override void MoveTopdown(Enemy enemy)
    //{

    //    if (barrelStartRotation != enemy.shooterTransform.rotation)
    //    {
    //        barrelStartRotation = enemy.shooterTransform.rotation;
    //        barrelInverseRotation = Quaternion.Inverse(barrelStartRotation);
    //    }


    //}
}
