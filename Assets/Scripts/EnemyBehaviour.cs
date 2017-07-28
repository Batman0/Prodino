using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
	public float speed;
	public float timerFire;
	public float fireRatio;
	public GameObject enemyBullet;
	public Transform enemyBulletSpawn;
	public bool canShoot = true;
    public PlayerController player;


    private const string enemyBulletTag = "EnemyBullet";

	// Update is called once per frame
	void Update () {

        Move();

		if (canShoot)
		{
			if (timerFire < fireRatio)
			{
				timerFire += Time.deltaTime;
			}
			else
			{
				Shoot();
				timerFire = 0.00f;
			}
		}
	}

	protected virtual void Shoot(){
		GameObject bullet = Instantiate (enemyBullet, enemyBulletSpawn.position, enemyBulletSpawn.rotation) as GameObject;
		bullet.tag = enemyBulletTag;
    }

    protected virtual void Move()
    {
        switch (GameManager.instance.cameraState)
        {
            case State.SIDESCROLL:
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                break;
            case State.TOPDOWN:
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                break;
        }
        transform.Translate(Vector3.right* speed * Time.deltaTime);
    }

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}
