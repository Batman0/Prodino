using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [Header("Objects")]
    protected GameManager gameManager;
    public ParticleSystemManager explosionParticleManager;
    public Transform bulletSpawnpoint;
    public Transform bulletSpawnpointSecond;
    public Transform shooterTransform;
    public Collider sideCollider;
    public Collider topCollider;
    protected Register register;
    protected float xMin;
    protected float xMax;
    public float timer = 0;
    protected ScriptableObject property;

    [Header("Statistics")]
    protected int enemyLives;
    protected float enemyDeactivationDelay = 0.5f;
    protected bool isDying = false;
    [HideInInspector]
    public bool isRight;

    virtual public void Awake()
    {
        register = Register.instance;
        gameManager = GameManager.instance;
        xMin = register.xMin;
        xMax = register.xMax;
    }
    virtual public void OnEnable()
    {

        if (gameManager.currentGameMode == GameMode.SIDESCROLL)
        {
            if (!sideCollider.enabled || topCollider.enabled)
            {
                topCollider.enabled = false;
                sideCollider.enabled = true;
            }
        }
        else
        {
            if (!topCollider.enabled || sideCollider.enabled)
            {
                sideCollider.enabled = false;
                topCollider.enabled = true;
            }
        }

    }
    
    public virtual void InitEnemy()
    {      
     
    }

    public virtual void Update()
    {
        ChangePerspective();

        if (CheckEnemyDead())
        {
            Deactivate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBullet")
        {
            enemyLives--;
            other.gameObject.SetActive(false);
        }
    }

    public virtual void Shoot()
    {
        if(!CheckCondition())
        {
            return;
        }
    }

    public virtual void Move()
    {
        if (!CheckCondition())
        {
            return;
        }
    }

    void ChangePerspective()
    {
        if (gameManager.transitionIsRunning)
        {
            if (gameManager.currentGameMode == GameMode.TOPDOWN)
            {
                if (!sideCollider.enabled)
                {
                    topCollider.enabled = false;
                    sideCollider.enabled = true;
                }
            }
            else
            {
                if (!topCollider.enabled)
                {
                    sideCollider.enabled = false;
                    topCollider.enabled = true;
                }
            }
        }
    }

    public void Deactivate()
    {
        if (explosionParticleManager != null)
        {
            explosionParticleManager.PlayAll();
        }
        isDying = true;
        StartCoroutine(DeactivateObject());
    }

    protected IEnumerator DeactivateObject()
    {
        yield return new WaitForSeconds(enemyDeactivationDelay);
        isDying = false;
        gameObject.SetActive(false);
    }

    public bool CheckEnemyDead()
    {
        return enemyLives <= 0;
    }

    public bool CheckCondition()
    {      
        return (!gameManager.transitionIsRunning && !isDying);
    }

    public abstract void SetProperty(ScriptableObject property);
}
