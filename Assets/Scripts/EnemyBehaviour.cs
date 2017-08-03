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
    private Vector3 sideScrollPos;
    private Vector3? TopDownPos;

    private const string enemyBulletTag = "EnemyBullet";

    void Start()
    {
        sideScrollPos = transform.position;
    }
	// Update is called once per frame
	void Update ()
    {
        Move();
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
        switch (GameManager.instance.cameraState)
        {
            case State.SIDESCROLL:
                transform.position = new Vector3(transform.position.x, sideScrollPos.y, 0);
                break;
            case State.TOPDOWN:
                if (TopDownPos == null)
                {
                    TopDownPos = transform.position;
                }
                transform.position = new Vector3(transform.position.x, 0, TopDownPos.Value.z);
                break;
        }
        transform.Translate(Vector3.right* -speed * Time.deltaTime,Space.World);
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
