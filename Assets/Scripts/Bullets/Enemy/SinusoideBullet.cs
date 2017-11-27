using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : NormalBullet
{
    public PropertiesDoubleAiming property;
    private bool moveForward;
    private float xSpeed;
    private float timeToMakeSinusoide;
    private float time;
    private float forwardDistance;
    private float backDistance;
    private float transformTargetDeltaDistance;
    private float zOriginal;
    private Vector3 originalPos;
    private Vector3 target;
    private Transform playerTr;

    protected override void Awake()
    {
        base.Awake();
        speed = property.xBulletSpeed;
        timeToMakeSinusoide = property.timeToMakeSinusoide;
        destructionMargin = register.propertiesPlayer.bulletDestructionMargin;
        forwardDistance = property.bulletForwardDistance;
        backDistance = property.bulletBackDistance;
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
        zOriginal = transform.position.z;
        time = 0;
    }

    protected override void Move()
    {
        if (Vector3.Distance(transform.position, new Vector3(transform.position.x, transform.position.y, target.z)) < transformTargetDeltaDistance)
        {
            moveForward = !moveForward;
            target = moveForward ? new Vector3(transform.position.x, transform.position.y, originalPos.z + forwardDistance) : new Vector3(transform.position.x, transform.position.y, originalPos.z - backDistance);
            zOriginal = transform.position.z;
            time = 0;
        }
        transform.position = new Vector3(transform.position.x + xSpeed * Time.fixedDeltaTime, transform.position.y, Mathfx.Hermite(zOriginal, target.z, time));
        //Debug.Log(Mathfx.Hermite(zOriginal, target.z, zSpeedToIncrease));
        time += Time.fixedDeltaTime / timeToMakeSinusoide;
    }
}
