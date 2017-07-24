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
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        switch(CameraController.instance.myState)
        {
            case CameraState.SIDESCROLL:
                transform.position = new Vector3(transform.position.x, transform.position.y,0);
                break;
            case CameraState.TOPDOWN:
                transform.position = new Vector3(transform.position.x, 0,transform.position.z);
                break;
        }
		transform.Translate (Vector3.up * speed * Time.deltaTime);

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

	void Shoot(){
		GameObject bullet = Instantiate (enemyBullet, enemyBulletSpawn.position, enemyBulletSpawn.rotation) as GameObject;
		bullet.tag = enemyBulletTag;
	}


}
