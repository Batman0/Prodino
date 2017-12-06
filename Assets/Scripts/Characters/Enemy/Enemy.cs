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
        EventManager.changeState += ChangeGameState;
        InitEnemy();
    }

    public virtual void Update()
    {
        if (CheckEnemyDead())
        {
            Deactivate();
        }
    }

    virtual public void OnDisable()
    {
        EventManager.changeState += ChangeGameState;
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
            topCollider.enabled = false;
            sideCollider.enabled = true;
        }
        else
        {
            sideCollider.enabled = false;
            topCollider.enabled = true;
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

    void ChangeGameState(GameMode currentGameMode)
    {
        if (currentGameMode == GameMode.TOPDOWN)
        {
            topCollider.enabled = true;
            sideCollider.enabled = false;
        }
        else
        {
            sideCollider.enabled = true;
            topCollider.enabled = false;
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
}
