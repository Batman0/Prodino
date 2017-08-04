using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
	public float speed;
	public float timerFire;
	public float fireRatio;
    /// <summary>
    /// How much far must the enemy be from the near clipping plane to be destroyed?
    /// </summary>
    public float destructionMargin;
	public GameObject enemyBullet;
	public Transform enemyBulletSpawn;
	public bool canShoot = true;
    public PlayerController player;
    private Vector3? originalPos;

    private const string enemyBulletTag = "EnemyBullet";

    void Start()
    {
        originalPos = new Vector3(transform.position.x, transform.position.y, 0);
        switch (GameManager.instance.cameraState)
        {
            case State.SIDESCROLL:
                transform.position = new Vector3(transform.position.x, originalPos.Value.y, 0);
                break;
            case State.TOPDOWN:
                transform.position = new Vector3(transform.position.x, 0, originalPos.Value.z);
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
        transform.Translate(Vector3.right* -speed * Time.deltaTime,Space.World);
    }

    protected virtual void ChangePerspective()
    {
        if (GameManager.instance.canChangeState)
        {
            switch (GameManager.instance.cameraState)
            {
                case State.SIDESCROLL:
                    if (transform.position != new Vector3(transform.position.x, 0, originalPos.Value.z))
                    {
                        transform.position = new Vector3(transform.position.x, 0, originalPos.Value.z);
                    }
                    break;
                case State.TOPDOWN:
                    if (transform.position != new Vector3(transform.position.x, originalPos.Value.y, 0))
                    {
                        transform.position = new Vector3(transform.position.x, originalPos.Value.y, 0);
                    }
                    break;
            }
        }
    }

    protected virtual void DestroyGameobject()
    {
        if (transform.position.x <= GameManager.instance.leftBound.x - destructionMargin)
        {
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}
