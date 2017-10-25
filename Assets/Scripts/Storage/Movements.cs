using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    ForwardShooter,
    Forward,
    LaserDiagonal,
    SphericalAiming,
    BombDrop,
    Trail,
    DoubleAiming,
    Circular,
    Square
}
public class Movements
{

    //public static void MoveForward(ref Enemy enemy)
    //{
    //    enemy.transform.Translate(Vector3.forward * enemy.speed * Time.deltaTime, Space.Self);

    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= Register.instance.xMin - enemy.destructionMargin)
    //        {
    //            enemy.Destroy(true);
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= Register.instance.xMax + enemy.destructionMargin)
    //        {
    //            enemy.Destroy(true);
    //        }
    //    }
    //}

    //private static void MoveForward(Transform transform, float speed)
    //{
    //    transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    //}

    //public static void MoveDiagonalY(ref Enemy enemy)
    //{
    //    enemy.transform.position = new Vector3(enemy.speed * Time.deltaTime + enemy.transform.position.x, 1 - (2 / Mathf.PI) * Mathf.Acos(Mathf.Cos(enemy.length * enemy.time * Mathf.PI / 2)) * enemy.amplitude + enemy.height, enemy.transform.position.z);
    //    enemy.time += Time.deltaTime;

    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= Register.instance.xMin - enemy.destructionMargin)
    //        {
    //            enemy.Destroy(true);
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= Register.instance.xMax + enemy.destructionMargin)
    //        {
    //            enemy.Destroy(true);
    //        }
    //    }
    //}

    //public static void MoveDiagonalZ(ref Enemy enemy)
    //{
    //    enemy.transform.position = new Vector3(enemy.speed * Time.deltaTime + enemy.transform.position.x, enemy.transform.position.y, 1 - (2 / Mathf.PI) * Mathf.Acos(Mathf.Cos(enemy.length * enemy.time * Mathf.PI / 2)) * enemy.amplitude + enemy.height);
    //    enemy.time += Time.deltaTime;

    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= Register.instance.xMin - enemy.destructionMargin)
    //        {
    //            enemy.Destroy(true);
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= Register.instance.xMax + enemy.destructionMargin)
    //        {
    //            enemy.Destroy(true);
    //        }
    //    }
    //}

    //public static void MoveCircular(ref Enemy enemy)
    //{
    //    if (enemy.isRight)
    //    {
    //        enemy.transform.position = new Vector3(enemy.radius * Mathf.Cos(Time.time * enemy.speed) + enemy.originalPos.x, enemy.radius * Mathf.Sin(Time.time * enemy.speed) + enemy.originalPos.y, enemy.radius * Mathf.Sin(Time.time * enemy.speed) + enemy.originalPos.z);
    //    }
    //    else
    //    {
    //        enemy.transform.position = new Vector3(-enemy.radius * Mathf.Cos(Time.time * enemy.speed) + enemy.originalPos.x, enemy.radius * Mathf.Sin(Time.time * enemy.speed) + enemy.originalPos.y, enemy.radius * Mathf.Sin(Time.time * enemy.speed) + enemy.originalPos.z);
    //    }

    //    if (enemy.lifeTime > 0.0f)
    //    {
    //        enemy.lifeTime -= Time.deltaTime;
    //    }
    //    else if (enemy.lifeTime <= 0.0f && !enemy.toDestroy)
    //    {
    //        enemy.toDestroy = true;
    //    }
    //}

    //public static void MoveGeometric(ref Enemy enemy)
    //{

    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= enemy.targets[enemy.movementTargetIndex].position.x)
    //        {
    //            enemy.movementTargetIndex++;
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= enemy.targets[enemy.movementTargetIndex].position.x)
    //        {
    //            enemy.movementTargetIndex++;
    //        }
    //    }

    //    if (enemy.transform.position != enemy.targets[enemy.movementTargetIndex].position)
    //    {
    //        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.targets[enemy.movementTargetIndex].position, enemy.speed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        if (enemy.movementTargetIndex < enemy.targets.Length - 1)
    //        {
    //            enemy.movementTargetIndex++;
    //        }
    //        else
    //        {
    //            enemy.Destroy(true);
    //        }      
    //    }
    //}

    //public static void MoveGeometricAndWait(ref Enemy enemy)
    //{
    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= enemy.targets[enemy.movementTargetIndex].position.x)
    //        {
    //            enemy.movementTargetIndex++;
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= enemy.targets[enemy.movementTargetIndex].position.x)
    //        {
    //            enemy.movementTargetIndex++;
    //        }
    //    }

    //    if (enemy.transform.position != enemy.targets[enemy.movementTargetIndex].position)
    //    {
    //        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.targets[enemy.movementTargetIndex].position, enemy.speed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        if (enemy.waitingTimer < enemy.waitingTime && enemy.movementTargetIndex < enemy.targets.Length - 1)
    //        {
    //            enemy.waitingTimer += Time.deltaTime;
    //        }

    //        if (enemy.waitingTimer >= enemy.waitingTime && enemy.movementTargetIndex < enemy.targets.Length - 1)
    //        {
    //            enemy.movementTargetIndex++;
    //            enemy.waitingTimer = 0.0f;
    //        }
    //        else if (enemy.movementTargetIndex >= enemy.targets.Length - 1)
    //        {
    //            enemy.Destroy(true);
    //        }
    //    }
    //    //return enemy.transform.position;
    //}

    //public static void MoveBackAndForth(ref Enemy enemy)
    //{
    //    if (enemy.waitingTimer < enemy.movementDuration && enemy.doneRotation == 0)
    //    {
    //        MoveForward(enemy.transform, enemy.speed);
    //        enemy.waitingTimer += Time.deltaTime;
    //    }
    //    else if (enemy.waitingTimer > 0.0f && enemy.doneRotation >= 180)
    //    {
    //        MoveForward(enemy.transform, enemy.backSpeed);
    //        enemy.waitingTimer -= Time.deltaTime;
    //    }
    //    else if (enemy.waitingTimer <= 0.0f && enemy.doneRotation >= 180)
    //    {
    //        enemy.Destroy(true);
    //    }
    //    else
    //    {
    //        if (enemy.doneRotation < 180)
    //        {
    //            if (enemy.sideCollider.enabled)
    //            {
    //                enemy.sideCollider.enabled = false;
    //                enemy.topCollider.enabled = true;
    //            }
    //            enemy.transform.Rotate(Vector3.up, enemy.rotationSpeed);
    //            enemy.doneRotation += enemy.rotationSpeed;
    //        }
    //        if (enemy.doneRotation >= 180 && !enemy.shoots)
    //        {
    //            enemy.shoots = true;
    //            if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && !enemy.sideCollider.enabled)
    //            {
    //                enemy.sideCollider.enabled = true;
    //                enemy.topCollider.enabled = false;
    //            }
    //        }
    //    }
    //}

    //public delegate void MyMovement(Enemy enemy);


    //public static void SetMovement(Enemy enemy)
    //{
    //    switch (enemy.movementType)
    //    {
    //        case MovementType.FORWARDSHOOTER:
    //            enemy.myMovementSidescroll += MoveForward;
    //            enemy.myMovementTopdown += MoveForward;
    //            break;
    //        case MovementType.FORWARD:
    //            enemy.myMovementSidescroll += MoveForward;
    //            enemy.myMovementTopdown += MoveForward;
    //            break;
    //        case MovementType.LASERDIAGONAL:
    //            //if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
    //            //{
    //            enemy.myMovementSidescroll += MoveDiagonalY;
    //            enemy.myMovementTopdown += MoveForward;
    //            //}
    //            //else
    //            //{
    //            //    enemy.myMovementSidescroll += MoveForward;
    //            //}
    //            break;
    //        case MovementType.SPHERICALAIMING:
    //            //if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
    //            //{
    //            enemy.myMovementSidescroll += MoveForward;
    //            enemy.myMovementTopdown += MoveDiagonalZ;
    //            //}
    //            //else
    //            //{
    //            //    enemy.myMovementSidescroll += MoveDiagonalZ;
    //            //}
    //            break;
    //        case MovementType.BOMBDROP:
    //            enemy.myMovementSidescroll += MoveForward;
    //            enemy.myMovementTopdown += MoveForward;
    //            break;
    //        case MovementType.TRAIL:
    //            enemy.myMovementSidescroll += MoveBackAndForth;
    //            enemy.myMovementTopdown += MoveBackAndForth;
    //            break;
    //        case MovementType.DOUBLEAIMING:
    //            //if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
    //            //{
    //            enemy.myMovementSidescroll += MoveForward;
    //            enemy.myMovementTopdown += MoveDiagonalZ;
    //            //}
    //            //else
    //            //{
    //            //    enemy.myMovementSidescroll += MoveDiagonalZ;
    //            //}
    //            break;
    //        case MovementType.CIRCULAR:
    //            enemy.myMovementSidescroll += MoveCircular;
    //            enemy.myMovementTopdown += MoveCircular;
    //            break;
    //        case MovementType.SQUARE:
    //            //if (enemy.isRight)
    //            //{
    //            enemy.myMovementSidescroll += MoveGeometricAndWait;
    //            enemy.myMovementTopdown += MoveGeometricAndWait;
    //            //}
    //            //else
    //            //{
    //            //    enemy.myMovementSidescroll += MoveGeometric;
    //            //}
    //            break;
    //    }
    //}
}
