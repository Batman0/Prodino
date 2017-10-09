using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    FORWARDSHOOTER,
    FORWARD,
    CIRCULAR,
    SQUARE,
    LASERDIAGONAL,
    SPHERICALAIMING,
    BOMBDROP,
    TRAIL,
    DOUBLEAIMING
}
public static class Movements
{
    public static void MoveForward(Transform transform, bool isRight, float speed, float destructionMargin, ref bool destroy)
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (isRight)
        {
            if (transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                destroy = true;
            }
        }
        else
        {
            if (transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                destroy = true;
            }
        }
    }

    public static void MoveForward(Transform transform, float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    public static void MoveCircular(Transform transform, float speed, bool isRight, float radius, Vector3 originalPos, ref float lifeTime, ref bool destroy)
    {
        if (isRight)
        {
            transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
        }
        else
        {
            transform.position = new Vector3(-radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
        }

        if (lifeTime > 0.0f)
        {
            lifeTime -= Time.deltaTime;
        }
        else if (lifeTime <= 0.0f && !destroy)
        {
            destroy = true;
        }
    }

    public static void MoveGeometric(ref int index, float speed, Transform[] targets, Transform transform, ref bool destroy)
    {

        if (transform.position != targets[index].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[index].position, speed * Time.deltaTime);
        }
        else
        {
            if (index < targets.Length - 1)
            {
                index++;
            }
            else
            {
                destroy = true;
            }      
        }
    }

    public static Vector3 MoveGeometric(ref int index, float speed, float waitingTime, ref float waitingTimer, Transform[] targets, Transform transform, ref bool destroy)
    {

        if (transform.position != targets[index].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[index].position, speed * Time.deltaTime);
        }
        else
        {
            if (waitingTimer < waitingTime && index < targets.Length - 1)
            {
                waitingTimer += Time.deltaTime;
            }

            if (waitingTimer >= waitingTime && index < targets.Length - 1)
            {
                index++;
                waitingTimer = 0.0f;
            }
            else if (index >= targets.Length - 1)
            {
                destroy = true;
            }
        }
        return transform.position;
    }

    public static void MoveBackAndForth(Transform transform, float forthSpeed, float backSpeed, float rotationSpeed, float movementDuration, ref float doneRotation, ref float movementTimer, ref bool canShoot, ref bool destroy)
    {
        if (movementTimer < movementDuration && doneRotation == 0)
        {
            MoveForward(transform, forthSpeed);
            movementTimer += Time.deltaTime;
        }
        else if (movementTimer > 0.0f && doneRotation >= 180)
        {
            MoveForward(transform, forthSpeed);
            movementTimer -= Time.deltaTime;
        }
        else if (movementTimer <= 0.0f && doneRotation >= 180)
        {
            destroy = true;
        }
        else
        {
            if (doneRotation == 0 && !canShoot)
            {
                canShoot = true;
            }
            if (doneRotation < 180)
            {
                transform.Rotate(Vector3.up, rotationSpeed);
                doneRotation += rotationSpeed;
            }
        }
    }


    public static void Move(MovementType movementType, Transform transform, bool isRight, ref bool canShoot, Vector3 originalPos, ref int targetIndex, ref float lifeTime, ref float timer, ref float doneRotation, ref bool toDestroy)
    {
        switch (movementType)
        {
            case MovementType.FORWARDSHOOTER:
                MoveForward(transform, isRight, Register.instance.propertiesForwardShooter.speed, Register.instance.propertiesForwardShooter.bulletDestructionMargin, ref toDestroy);
                break;
            case MovementType.FORWARD:

                MoveForward(transform, isRight, Register.instance.propertiesForward.speed, Register.instance.propertiesForward.destructionMargin, ref toDestroy);
                break;
            case MovementType.LASERDIAGONAL:
                if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {
                    if (isRight)
                    {
                        MoveGeometric(ref targetIndex, Register.instance.propertiesLaserDiagonal.yMovementSpeed, Register.instance.propertiesLaserDiagonal.rightTargets, transform, ref toDestroy);
                    }
                    else
                    {
                        MoveGeometric(ref targetIndex, Register.instance.propertiesLaserDiagonal.yMovementSpeed, Register.instance.propertiesLaserDiagonal.leftTargets, transform, ref toDestroy);
                    }
                }
                else
                {
                    MoveForward(transform, isRight, Register.instance.propertiesLaserDiagonal.xMovementSpeed, Register.instance.propertiesLaserDiagonal.destructionMargin, ref toDestroy);
                }
                break;
            case MovementType.SPHERICALAIMING:
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {
                    MoveForward(transform, isRight, Register.instance.propertiesSphericalAiming.xMovementSpeed, Register.instance.propertiesSphericalAiming.destructionMargin, ref toDestroy);
                }
                else
                {
                    if (isRight)
                    {
                        MoveGeometric(ref targetIndex, Register.instance.propertiesSphericalAiming.zMovementSpeed, Register.instance.propertiesSphericalAiming.rightTargets, transform, ref toDestroy);
                    }
                    else
                    {
                        MoveGeometric(ref targetIndex, Register.instance.propertiesSphericalAiming.zMovementSpeed, Register.instance.propertiesSphericalAiming.leftTargets, transform, ref toDestroy);
                    }
                }
                break;
            case MovementType.BOMBDROP:
                MoveForward(transform, isRight, Register.instance.propertiesBombDrop.xMovementSpeed, Register.instance.propertiesBombDrop.destructionMargin, ref toDestroy);
                break;
            case MovementType.TRAIL:
                MoveBackAndForth(transform, Register.instance.propertiesTrail.xMovementSpeed, Register.instance.propertiesTrail.xReturnSpeed, Register.instance.propertiesTrail.rotationSpeed, Register.instance.propertiesTrail.movementDuration, ref doneRotation, ref timer, ref canShoot, ref toDestroy);
                break;
            case MovementType.DOUBLEAIMING:
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {
                    MoveForward(transform, isRight, Register.instance.propertiesDoubleAiming.xMovementSpeed, Register.instance.propertiesDoubleAiming.destructionMargin, ref toDestroy);
                }
                else
                {
                    if (isRight)
                    {
                        MoveGeometric(ref targetIndex, Register.instance.propertiesDoubleAiming.zMovementSpeed, Register.instance.propertiesDoubleAiming.rightTargets, transform, ref toDestroy);
                    }
                    else
                    {
                        MoveGeometric(ref targetIndex, Register.instance.propertiesDoubleAiming.zMovementSpeed, Register.instance.propertiesDoubleAiming.leftTargets, transform, ref toDestroy);
                    }
                }
                break;
            case MovementType.CIRCULAR:
                MoveCircular(transform, Register.instance.propertiesCircular.speed, isRight, Register.instance.propertiesCircular.radius, originalPos, ref lifeTime, ref toDestroy);
                break;
            case MovementType.SQUARE:
                if (isRight)
                {
                    MoveGeometric(ref targetIndex, Register.instance.propertiesSquare.speed, Register.instance.propertiesSquare.waitingTime, ref timer, Register.instance.propertiesSquare.rightTargets, transform, ref toDestroy);
                }
                else
                {
                    MoveGeometric(ref targetIndex, Register.instance.propertiesSquare.speed, Register.instance.propertiesSquare.waitingTime, ref timer, Register.instance.propertiesSquare.leftTargets, transform, ref toDestroy);
                }
                break;
        }
    }
}
