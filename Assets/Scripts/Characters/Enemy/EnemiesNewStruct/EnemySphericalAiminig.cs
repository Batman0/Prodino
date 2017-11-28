using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySphericalAiminig : Enemy
{
    
    private new PropertiesSphericalAiming property;

    [Header("Common")]
    private Transform playerTr;

    [Header("Movement")]
    private bool moveForward;
    private bool barrelRight;
    private float zMovementSpeed;
    private float destructionMargin;
    private float amplitude;
    private float targetPlayerDeltaDistance = 0.1f;
    private float rotationDeadZone;
    private float rotationSpeed;
    private Vector3 originalPos;
    private Vector3 topdownTarget;
    private Quaternion shooterTransformStartRotation;
    private Quaternion shooterTransformInverseRotation;
    private Collider playerCl;
    private float speed;

    [Header("Shot")]
    private float fireRate;

    public override void InitEnemy()
    {
        base.InitEnemy();
        enemyLives = property.lives;
        originalPos = transform.position;
        speed = isRight ? -property.xSpeed : property.xSpeed;
        barrelRight = isRight ? false : true;
        zMovementSpeed = property.zMovementSpeed;
        destructionMargin = property.destructionMargin;
        amplitude = property.amplitude;
        topdownTarget = new Vector3(transform.position.x, transform.position.y, originalPos.z + amplitude);
        moveForward = true;
        playerCl = register.player.sideBodyCollider;
        rotationDeadZone = property.rotationDeadZone;
        rotationSpeed = property.rotationSpeed;
        targetPlayerDeltaDistance = zMovementSpeed / 10;
        shooterTransformStartRotation = shooterTransform.rotation;
        shooterTransformInverseRotation = Quaternion.Inverse(shooterTransformStartRotation);
    }

    void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();
        Shoot();

	}

    public override void Move()
    {
        base.Move();
        if (Vector3.Distance(transform.position, topdownTarget) > targetPlayerDeltaDistance)
        {
            topdownTarget = new Vector3(transform.position.x, transform.position.y, topdownTarget.z);
            Debug.Log("NO");
        }
        else
        {
            Debug.Log("YES");
            //moveForward = !moveForward;
            zMovementSpeed = -zMovementSpeed;
            amplitude = -amplitude;
            topdownTarget = new Vector3(transform.position.x, transform.position.y, originalPos.z + amplitude);
        }

        transform.position = new Vector3(speed * Time.fixedDeltaTime + transform.position.x, transform.position.y, zMovementSpeed * Time.fixedDeltaTime + transform.position.z);

        if (isRight)
        {
            if (transform.position.x <= xMin - destructionMargin)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.x >= xMax + destructionMargin)
            {
                gameObject.SetActive(false);
            }
        }

        if (gameManager.currentGameMode == GameMode.SIDESCROLL)
        {
            Vector3 playerTransform = new Vector3(playerCl.bounds.center.x - transform.position.x, playerCl.bounds.center.y - transform.position.y, 0);
            Vector3 barrelSpawnpointTransform = new Vector3(bulletSpawnpoint.position.x - transform.position.x, bulletSpawnpoint.position.y - transform.position.y, 0);
            float angle = Vector3.Angle(barrelSpawnpointTransform, playerTransform);
            Vector3 cross = Vector3.Cross(playerTransform, barrelSpawnpointTransform);

            if (angle > rotationDeadZone)
            {
                if (cross.z >= 0)
                {
                    shooterTransform.RotateAround(transform.position, Vector3.forward, -rotationSpeed);
                }
                else
                {
                    shooterTransform.RotateAround(transform.position, Vector3.forward, rotationSpeed);
                }
            }
        }
        else
        {
            if (transform.position.x < playerTr.position.x)
            {
                if (barrelRight)
                {
                    
                    shooterTransform.rotation = isRight ? shooterTransformInverseRotation : shooterTransformStartRotation;
                    barrelRight = false;
                }
            }
            else
            {
                if (!barrelRight)
                {
                   
                    shooterTransform.rotation = isRight ? shooterTransformStartRotation : shooterTransformInverseRotation;
                    barrelRight = true;
                }
            }
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        if (timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bullet = PoolManager.instance.pooledBulletClass[property.enemyName].GetpooledBullet();
            bullet.transform.position = bulletSpawnpoint.position;
            bullet.transform.rotation = shooterTransform.rotation;
            bullet.SetActive(true);
            timer = 0.0f;
        }
    }

    public override void SetProperty(ScriptableObject _property)
    {
        property = (PropertiesSphericalAiming)_property;
        InitEnemy();
    }
}
