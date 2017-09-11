using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float speed;
	private float timerFire;
	public float fireRatio;
    /// <summary>
    /// How much far must the enemy be from the near clipping plane to be destroyed?
    /// </summary>
	public GameObject enemyBullet;
	public Transform enemyBulletSpawn;
	public bool canShoot = true;
    [HideInInspector]
    public bool isRight;
    [HideInInspector]
    public Vector3 originalPos;

    private const string enemyBulletTag = "EnemyBullet";

    void Awake()
    {
        Register.instance.numberOfEnemies++;
        originalPos = transform.position;
        switch (GameManager.instance.cameraState)
        {
            case State.SIDESCROLL:
                transform.position = new Vector3(transform.position.x, originalPos.y, 0);
                break;
            case State.TOPDOWN:
                transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
                break;
        }
    }

    void Start()
    {
        transform.rotation = isRight ? transform.rotation : Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }
    // Update is called once per frame
    void Update ()
    {
        Move();
        ChangePerspective();
        Shoot();
        DestroyGameobject();
    }

	protected virtual void Shoot()
    {
        //if (transform.position.x <= GameManager.instance.rightBound.x && transform.position.x >= GameManager.instance.leftBound.x)
        //{
            if (canShoot)
            {
                if (timerFire < fireRatio)
                {
                    timerFire += Time.deltaTime;
                }
                else
                {
                    GameObject bullet = Instantiate(enemyBullet, enemyBulletSpawn.position, transform.rotation) as GameObject;
                    bullet.tag = enemyBulletTag;
                    timerFire = 0.0f;
                }
            }
        //}
        //else
        //{
        //    if (timerFire != 0.0f)
        //    {
        //        timerFire = 0.0f;
        //    }
        //}
    }

    protected virtual void Move()
    {

    }

    protected virtual void ChangePerspective()
    {
        if (Register.instance.canStartEnemyTransition)
        {
            switch (GameManager.instance.cameraState)
            {
                case State.SIDESCROLL:
                    if (transform.position != new Vector3(transform.position.x, transform.position.y, originalPos.z))
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);
                    }
                    break;
                case State.TOPDOWN:
                    if (transform.position != new Vector3(transform.position.x, originalPos.y, transform.position.z))
                    {
                        transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
                    }
                    break;
            }
            Register.instance.translatedEnemies++;
            if (Register.instance.translatedEnemies == Register.instance.numberOfEnemies)
            {
                Register.instance.translatedEnemies = 0;
                Register.instance.canStartEnemyTransition = false;
            }
        }
        else if (Register.instance.canEndEnemyTransition)
        {
            switch (GameManager.instance.cameraState)
            {
                case State.TOPDOWN:
                    if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z))
                    {
                        transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
                    }
                    break;
                case State.SIDESCROLL:
                    if (transform.position != new Vector3(transform.position.x, originalPos.y, 0))
                    {
                        transform.position = new Vector3(transform.position.x, originalPos.y, 0);
                    }
                    break;
            }
            Register.instance.translatedEnemies++;
            if (Register.instance.translatedEnemies == Register.instance.numberOfEnemies)
            {
                Register.instance.translatedEnemies = 0;
                Register.instance.canEndEnemyTransition = false;
            }
        }
    }

    protected virtual void DestroyGameobject()
    {

    }
}
