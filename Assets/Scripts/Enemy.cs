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
    private Vector3 originalPos;

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
                transform.position = new Vector3(transform.position.x, 0, originalPos.z);
                break;
        }
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
        if (transform.position.x <= GameManager.instance.rightBound.x && transform.position.x > GameManager.instance.leftBound.x)
        {
            if (canShoot)
            {
                if (timerFire < fireRatio)
                {
                    timerFire += Time.deltaTime;
                }
                else
                {
                    GameObject bullet = Instantiate(enemyBullet, enemyBulletSpawn.position, enemyBulletSpawn.rotation) as GameObject;
                    bullet.tag = enemyBulletTag;
                    timerFire = 0.00f;
                }
            }
        }
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
                    if (transform.position != new Vector3(transform.position.x, 0, originalPos.z))
                    {
                        transform.position = new Vector3(transform.position.x, 0, originalPos.z);
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

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}
