using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTrail : EnemyMovement
{
    private float backSpeed;
    private float rotationSpeed;
    private float movementDuration;
    private float waitingTimer;
    private float doneRotation;
    private Transform enemyTransform;
    private PropertiesTrail properties;

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
        enemyTransform = enemy.meshTransform;
    }

    public override void MoveSidescroll(Enemy enemy)
    {
        if (waitingTimer < movementDuration && doneRotation == 0)
        {
            MoveForward(enemy.transform, speed);
            waitingTimer += Time.deltaTime;
        }
        else if (waitingTimer > 0.0f && doneRotation >= 180)
        {
            MoveForward(enemy.transform, -backSpeed);
            waitingTimer -= Time.deltaTime;
        }
        else if (waitingTimer <= 0.0f && doneRotation >= 180)
        {
            enemy.gameObject.SetActive(false);
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
            if (doneRotation >= 180 && !enemy.canShoot)
            {
                enemy.canShoot = true;
                //if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && !enemy.sideCollider.enabled)
                //{
                //    enemy.sideCollider.enabled = true;
                //    enemy.topCollider.enabled = false;
                //}
            }
        }
    }

    public override void MoveTopdown(Enemy enemy)
    {
        if (waitingTimer < movementDuration && doneRotation == 0)
        {
            MoveForward(enemy.transform, speed);
            waitingTimer += Time.deltaTime;
        }
        else if (waitingTimer > 0.0f && doneRotation >= 180)
        {
            MoveForward(enemy.transform, -backSpeed);
            waitingTimer -= Time.deltaTime;
        }
        else if (waitingTimer <= 0.0f && doneRotation >= 180)
        {
            enemy.gameObject.SetActive(false);
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
            if (doneRotation >= 180 && !enemy.canShoot)
            {
                enemy.canShoot = true;
                //if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && !enemy.sideCollider.enabled)
                //{
                //    enemy.sideCollider.enabled = true;
                //    enemy.topCollider.enabled = false;
                //}
            }
        }
    }

    private static void MoveForward(Transform transform, float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

}
