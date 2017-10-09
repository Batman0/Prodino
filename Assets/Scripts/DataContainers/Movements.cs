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
            else
            {
                if (index < targets.Length - 1)
                {
                    index++;
                    waitingTimer = 0.0f;
                }
                else
                {
                    destroy = true;
                }
            }
        }
        return transform.position;
    }

    public static void MoveBackAndForth(Transform transform, float forthSpeed, float backSpeed, float rotationSpeed, float movementDuration, ref float doneRotation, ref float movementTimer, ref bool canShoot, ref bool destroy,Enemy enemy)
    {
        if (movementTimer < movementDuration && doneRotation == 0)
        {
            MoveForward(transform, forthSpeed);
            movementTimer += Time.deltaTime;
            
        }
        else if (movementTimer > 0.0f && doneRotation >= 180)
        {
            enemy.sideCollider.enabled = true;
            enemy.topCollider.enabled = false;
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
                enemy.sideCollider.enabled = false;
                enemy.topCollider.enabled = false;
                transform.Rotate(Vector3.up, rotationSpeed);
                doneRotation += rotationSpeed;
            }
        }
    }


    public static void Move(MovementType movementType, Transform transform, bool isRight, ref bool canShoot, Properties properties, Vector3 originalPos, ref int targetIndex, ref float lifeTime, ref float timer, ref float doneRotation, ref bool toDestroy, Enemy enemy)
    {
        switch (movementType)
        {
            case MovementType.FORWARDSHOOTER:
                MoveForward(transform, isRight, properties.fs_Speed, properties.fs_DestructionMargin, ref toDestroy);
                break;
            case MovementType.FORWARD:
                MoveForward(transform, isRight, properties.f_Speed, properties.f_DestructionMargin, ref toDestroy);
                break;
            case MovementType.LASERDIAGONAL:
                if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {
                    if (isRight)
                    {
                        MoveGeometric(ref targetIndex, properties.ld_YMovementSpeed, properties.ld_RightTargets, transform, ref toDestroy);
                    }
                    else
                    {
                        MoveGeometric(ref targetIndex, properties.ld_YMovementSpeed, properties.ld_LeftTargets, transform, ref toDestroy);
                    }
                }
                else
                {
                    MoveForward(transform, isRight, properties.ld_XMovementSpeed, properties.ld_DestructionMargin, ref toDestroy);
                }
                break;
            case MovementType.SPHERICALAIMING:
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {
                    MoveForward(transform, isRight, properties.sa_XMovementSpeed, properties.sa_DestructionMargin, ref toDestroy);
                }
                else
                {
                    if (isRight)
                    {
                        MoveGeometric(ref targetIndex, properties.sa_ZMovementSpeed, properties.sa_RightTargets, transform, ref toDestroy);
                    }
                    else
                    {
                        MoveGeometric(ref targetIndex, properties.sa_ZMovementSpeed, properties.sa_LeftTargets, transform, ref toDestroy);
                    }
                }
                break;
            case MovementType.BOMBDROP:
                MoveForward(transform, isRight, properties.bd_XMovementSpeed, properties.bd_DestructionMargin, ref toDestroy);
                break;
            case MovementType.TRAIL:
                MoveBackAndForth(transform, properties.t_XMovementSpeed, properties.t_XReturnSpeed, properties.t_RotationSpeed, properties.t_MovementDuration, ref doneRotation, ref timer, ref canShoot, ref toDestroy, enemy);
                break;
            case MovementType.DOUBLEAIMING:
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {
                    MoveForward(transform, isRight, properties.da_XMovementSpeed, properties.da_DestructionMargin, ref toDestroy);
                }
                else
                {
                    if (isRight)
                    {
                        MoveGeometric(ref targetIndex, properties.da_ZMovementSpeed, properties.da_RightTargets, transform, ref toDestroy);
                    }
                    else
                    {
                        MoveGeometric(ref targetIndex, properties.da_ZMovementSpeed, properties.da_LeftTargets, transform, ref toDestroy);
                    }
                }
                break;
            case MovementType.CIRCULAR:
                MoveCircular(transform, properties.c_Speed, isRight, properties.c_Radius, originalPos, ref lifeTime, ref toDestroy);
                break;
            case MovementType.SQUARE:
                if (isRight)
                {
                    MoveGeometric(ref targetIndex, properties.sq_Speed, properties.sq_WaitingTime, ref timer, properties.sq_RightTargets, transform, ref toDestroy);
                }
                else
                {
                    MoveGeometric(ref targetIndex, properties.sq_Speed, properties.sq_WaitingTime, ref timer, properties.sq_LeftTargets, transform, ref toDestroy);
                }
                break;
        }
    }
}
