using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : NormalBullet
{

    private bool moveForward;
    private float xSpeed;
    //public float value;
    private float sinusoideDuration;
    private float forwardDistance;
    private float backDistance;
    private float transformTargetDeltaDistance;
    private float zOriginal;
    private float moment;
    /// <summary>
    /// Makes the movement more or less smooth. It doesn't have to be less than 0.935f (0.94f just to be sure).
    /// </summary>
    private float easingValue;
    private Vector3 originalPos;
    private Vector3 target;
    private Transform playerTr;

    protected override void Awake()
    {
        base.Awake();
        speed = register.propertiesDoubleAiming.xBulletSpeed;
        sinusoideDuration = register.propertiesDoubleAiming.sinusoideDuration;
        destructionMargin = register.propertiesPlayer.bulletDestructionMargin;
        forwardDistance = register.propertiesDoubleAiming.bulletForwardDistance;
        backDistance = register.propertiesDoubleAiming.bulletBackDistance;
        easingValue = register.propertiesDoubleAiming.easingValue;
        playerTr = register.player.transform;
        transformTargetDeltaDistance = 0.5f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        originalPos = transform.position;
        if (transform.tag == "EnemyBulletInverse")
        {
            moveForward = false;
        }
        else
        {
            moveForward = true;
        }
        target = moveForward ? new Vector3(transform.position.x, transform.position.y, originalPos.z + forwardDistance) : new Vector3(transform.position.x, transform.position.y, originalPos.z - backDistance);
        xSpeed = transform.position.x >= playerTr.position.x ? -speed : speed;
        zOriginal = !moveForward ? originalPos.z + forwardDistance : originalPos.z - backDistance;
        moment = 0.5f;
    }

    protected override void Move()
    {
        if (Vector3.Distance(transform.position, new Vector3(transform.position.x, transform.position.y, target.z)) < transformTargetDeltaDistance)
        {
            moveForward = !moveForward;
            target = moveForward ? new Vector3(transform.position.x, transform.position.y, originalPos.z + forwardDistance) : new Vector3(transform.position.x, transform.position.y, originalPos.z - backDistance);
            zOriginal = transform.position.z;
            moment = 0;
        }
        transform.position = new Vector3(transform.position.x + xSpeed * Time.fixedDeltaTime, transform.position.y, Hermite(zOriginal, target.z, moment, easingValue));
        moment += Time.fixedDeltaTime / sinusoideDuration;
    }

    public static float Hermite(float start, float end, float value1, float value2)
    {
        return Mathf.Lerp(start, end, value1 * (value1 * value2) * (3.0f - 2.0f * value1));
    }

    public static Vector2 Hermite(Vector2 start, Vector2 end, float value1, float value2)
    {
        return new Vector2(Hermite(start.x, end.x, value1, value2), Hermite(start.y, end.y, value1, value2));
    }

    public static Vector3 Hermite(Vector3 start, Vector3 end, float value1, float value2)
    {
        return new Vector3(Hermite(start.x, end.x, value1, value2), Hermite(start.y, end.y, value1, value2), Hermite(start.z, end.z, value1, value2));
    }
}
