using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : NormalBullet
{

    private bool moveForward;
    private float xSpeed;
    private float zSpeed;
    private float forwardDistance;
    private float backDistance;
    private float transformTargetDeltaDistance;
    private Vector3 originalPos;
    private Vector3 target;
    private Transform playerTr;

    protected override void Awake()
    {
        base.Awake();
        speed = Register.instance.propertiesDoubleAiming.xBulletSpeed;
        zSpeed = Register.instance.propertiesDoubleAiming.zBulletSpeed;
        destructionMargin = Register.instance.propertiesPlayer.bulletDestructionMargin;
        forwardDistance = Register.instance.propertiesDoubleAiming.bulletForwardDistance;
        backDistance = Register.instance.propertiesDoubleAiming.bulletBackDistance;
        playerTr = Register.instance.player.transform;
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
    }

    protected override void Move()
    {
        if (Vector3.Distance(transform.position, new Vector3(transform.position.x, transform.position.y, target.z)) < transformTargetDeltaDistance)
        {
            moveForward = !moveForward;
            target = moveForward ? new Vector3(transform.position.x, transform.position.y, originalPos.z + forwardDistance) : new Vector3(transform.position.x, transform.position.y, originalPos.z - backDistance);
        }
        transform.position = new Vector3(transform.position.x + xSpeed * Time.deltaTime, transform.position.y, Mathfx.Hermite(transform.position.z, target.z, zSpeed * Time.deltaTime));
    }
}
