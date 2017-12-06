using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : MonoBehaviour
{
    private bool blownUp;
    [SerializeField]
    private float bombFallSpeed;
    [SerializeField]
    private float bombLifeTime;
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
    }

    private void OnEnable()
    {
        EventManager.changeState += ChangePerspective;
        blownUp = false;
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
            other.gameObject.SetActive(false);
            blownUp = false;
        }

        StartCoroutine("Deactivate", bombLifeTime);
    }

    void OnDisable()
    {
        explosionSidescrollCollider.enabled = false;
        explosionTopdownCollider.enabled = false;
    }

    void Move()
    {
        rb.AddForce(Vector3.down * bombFallSpeed, ForceMode.Acceleration);
    }

    void ChangePerspective(GameMode currentState)
    {
        if (blownUp)
        {
            if (currentState == GameMode.SIDESCROLL)
            {

                explosionSidescrollCollider.enabled = true;
                explosionTopdownCollider.enabled = false;

            }
            else
            {
                //if (!explosionTopdownCollider.enabled)
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
