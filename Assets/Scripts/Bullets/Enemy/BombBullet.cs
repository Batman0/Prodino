using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : MonoBehaviour
{
    public PropertiesBombDrop property;
    private bool blownUp;
    private float speed;
    private float lifeTime;
    public CapsuleCollider explosionSidescrollCollider;
    public CapsuleCollider explosionTopdownCollider;
    public Rigidbody rb;
    private Register register;
    private GameManager gameManager;
    private GameMode currentGameMode;

    private void Awake()
    {
        register = Register.instance;
        gameManager = GameManager.instance;
        speed = property.bombFallSpeed;
        lifeTime = property.bombLifeTime;
    }

    private void OnEnable()
    {
        currentGameMode = gameManager.currentGameMode;
        blownUp = false;
    }

    void Update()
    {
        ChangePerspective();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Env")
        {
            blownUp = true;
        }

        if(other.gameObject.tag =="Player")
        {
            gameObject.SetActive(false);
            Destroy(other.gameObject);
            blownUp = false;
            //explosionSidescrollCollider.enabled = false;
            //explosionTopdownCollider.enabled = false;
        }

        StartCoroutine("Deactivate", lifeTime);
    }

    void OnDisable()
    {
        explosionSidescrollCollider.enabled = false;
        explosionTopdownCollider.enabled = false;
    }

    void Move()
    {
        rb.AddForce(Vector3.down * speed, ForceMode.Acceleration);
    }

    void ChangePerspective()
    {
        if (blownUp)
        {
            if (gameManager.currentGameMode == GameMode.SIDESCROLL)
            {
                if (!explosionSidescrollCollider.enabled)
                {
                    explosionSidescrollCollider.enabled = true;
                    explosionTopdownCollider.enabled = false;
                }
            }
            else
            {
                if (!explosionTopdownCollider.enabled)
                {
                    explosionSidescrollCollider.enabled = false;
                    explosionTopdownCollider.enabled = true;
                }
            }
        }
    }

    IEnumerator Deactivate(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
