using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : NormalBullet
{
    //public PropertiesDoubleAiming property;
    private bool moveForward;
    private float xSpeed;
    [SerializeField]
    private float sinusoideDuration;
    [SerializeField]
    private float forwardDistance;
    [SerializeField]
    private float backDistance;
    private float transformTargetDeltaDistance;
    private float zOriginal;
    private float moment;
    /// <summary>
    /// Makes the movement more or less smooth. It doesn't have to be less than 0.935f (0.94f just to be sure).
    /// </summary>
    [SerializeField]
    [Range(0.94f, 10f)]
    private float easingValue;
    private Vector3 originalPos;
    private Vector3 target;
    private Transform playerTr;

    protected override void Awake()
    {
        base.Awake();
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
        transform.position = new Vector3(transform.position.x + xSpeed * Time.fixedDeltaTime, transform.position.y, Mathfx.Hermite(zOriginal, target.z, moment, easingValue));
        moment += Time.fixedDeltaTime / sinusoideDuration;
    }
}
