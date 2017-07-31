using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private bool isRight;
    private bool isLeft;
    private bool isCenter;
    public float bulletSpeed = 10.0f;
    public float lifeTime = 4.0f;

    private void Awake()
    {
        AssignDirection();
    }

    void Start()
    {
        DestroyGameObject();
    }

    void FixedUpdate()
    {
        Move();
    }

	void OnTriggerEnter(Collider other)
    {
		if (gameObject.tag == "EnemyBullet" && other.gameObject.tag == "Player")
        {
			Destroy (other.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}

		if (gameObject.tag == "PlayerBullet" && other.gameObject.tag == "Enemy")
        {
			Destroy (other.gameObject);
		}
	}

    void DestroyGameObject()
    {
        Destroy(gameObject, lifeTime);
    }

    void AssignDirection()
    {
        if (transform.position.x > GameManager.instance.playerPosition.x)
        {
            isRight = true;
            isLeft = false;
            isCenter = false;
        }
        else if (transform.position.x < GameManager.instance.playerPosition.x)
        {
            isRight = false;
            isLeft = true;
            isCenter = false;
        }
        else if (transform.position.x == GameManager.instance.playerPosition.x)
        {
            isRight = false;
            isLeft = false;
            isCenter = true;
        }
    }

    void Move()
    {
        switch (gameObject.tag)
        {
            case "PlayerBullet":
                if (GameManager.instance.cameraState == State.SIDESCROLL)
                {
                    if (isRight)
                    {
                        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime, Space.World);
                    }
                    else if (isLeft)
                    {
                        transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime, Space.World);
                    }
                    else if (isCenter)
                    {
                        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.World);
                    }
                }
                else if (GameManager.instance.cameraState == State.TOPDOWN)
                {
                    transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self);
                }
                break;
            case "EnemyBullet":
                transform.Translate(-Vector3.forward * bulletSpeed * Time.deltaTime, Space.World);
                break;
        }
    }
}
