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
    //public GameObject gameObjectPrefab;
    public GameObject bulletPrefab;
    public string enemyName;
    protected Register register;
    protected float xMin;
    protected float xMax;
    //protected ScriptableObject property;

    [Header("Statistics")]
    [SerializeField]
    protected int enemyLives;
    protected float enemyDeactivationDelay = 0.5f;
    [SerializeField]
    private float xSpeed;
    protected float xSpeedAdjustable;
    [SerializeField]
    protected float destructionMargin;
    protected bool isDying = false;
    [HideInInspector]
    public bool isRight;
    public float fireRateTimer = 0;

    virtual public void Awake()
    {
        ConstructEnemy();
    }

    virtual public void OnEnable()
    {
        InitEnemy();
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
        }
    }

    public virtual void ConstructEnemy()
    {
        register = Register.instance;
        gameManager = GameManager.instance;
        xMin = register.xMin;
        xMax = register.xMax;
    }

    public virtual void InitEnemy()
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

        isRight = transform.position.x >= register.player.transform.position.x ? true : false;
        CheckRotation();
        xSpeedAdjustable = xSpeed;
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

    private void CheckRotation()
    {
        if (!isRight)
        {
            transform.Rotate(Vector3.up, 180, Space.World);
        }
    }

    //public abstract void SetProperty(ScriptableObject property);
}
