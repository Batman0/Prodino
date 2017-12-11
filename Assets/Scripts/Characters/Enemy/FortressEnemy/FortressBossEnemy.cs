using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossEnemy : MonoBehaviour
{


    public enum FortressState
    {
        Entry, Shooting, OpeningWeakSpot, Vulnerable, ClosingWeakSpot, OpeningCore, ProtectingCore, VulnerableCore, ClosingCore, ShootingPlus, Death
    }

    [Header("Fortress Parameters")]
    public int healthPoints;
    public int maxDamagePerCycle;
    private int cycleCurrentDamage = 0;

    [Header("State durations")]
    public float openingWeakSpotDuration = 1f;
    public float vulnerableDuration = 1f;
    public float closingWeakSpotDuration = 1f;
    public float openingCoreDuration;
    public float vulnerableCoreDuration;
    public float closingCoreDuration;
    public float attackPlusDuration;


    [Header("Cannons Parameters")]
    public float[] fireRates;
    public float[] bulletSpeeds;
    [SerializeField]
    public CannonPatterns[] cannonPatterns;

    [Header("Object components")]
    public Transform body;
    public Transform bodyFinalTransform;
    private Vector3 bodyInitialPosition;

    //Cannons
    private int currentBullet;
    private float fireTimer = 0;
    private FortressBullet[] bulletPool;
    private FortressBossCannon[] cannons = new FortressBossCannon[4];

    //States and cycles
    private int currentCycle = 0;
    private FortressState state = FortressState.Entry;
    private int hitSpots = 0;
    private float currentStateEnterTime = 0;

    public FortressState State
    {
        get
        {
            return state;
        }
    }

    [System.Serializable]
    public class CannonPatterns
    {
        private int currentBulletIndex;
        private int currentPattern = -1;
        [SerializeField]
        public Pattern[] patterns;
        [System.Serializable]
        public struct Pattern
        {
            public int[] pattern;
        }
        public void GetCurrentCannon(ref int cannonIndex, ref bool isLast)
        {
            if (currentPattern == -1)
            { SetPattern(); }
            cannonIndex = patterns[currentPattern].pattern[currentBulletIndex];
            if (currentBulletIndex < patterns[currentPattern].pattern.Length - 1)
            {
                currentBulletIndex++;
            }
            else
            {
                isLast = true;
                currentBulletIndex = 0;
                currentPattern = -1;
            }
        }
        private void SetPattern()
        {
            currentPattern = 0;/* Random.Range(0, patterns.Length);*/
        }
    }

    void Awake()
    {
        bulletPool = GetComponentsInChildren<FortressBullet>();
        foreach (FortressBullet bullet in bulletPool)
        {
            bullet.transform.parent = null;
            bullet.gameObject.SetActive(false);
        }
        cannons = GetComponentsInChildren<FortressBossCannon>();
        bodyInitialPosition = body.position;
    }

    void Start()
    {
        EnterNewState(FortressState.Shooting);
    }

    void FixedUpdate()
    {
        Debug.Log(state);
        switch (state)
        {
            case FortressState.Shooting:
                TryShooting();
                break;
            case FortressState.OpeningCore:
                OpenCoreAnimation();
                break;
            case FortressState.ClosingCore:
                CloseCoreAnimation();
                break;
            default: break;
        }
    }

    void TryShooting()
    {
        fireTimer += Time.fixedDeltaTime;
        if (fireTimer >= fireRates[currentCycle])
        {
            fireTimer = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        int currentCannon = 0;
        bool isLast = false;
        cannonPatterns[currentCycle].GetCurrentCannon(ref currentCannon, ref isLast);
        cannons[currentCannon].Shoot(bulletPool[currentBullet], bulletSpeeds[currentCycle]);
        NextBullet();
        if (isLast)
        {
            EnterOpeningWeakSpotState();
        }
    }

    void NextBullet()
    {
        currentBullet++;
        if (currentBullet >= bulletPool.Length)
        {
            currentBullet = 0;
        }
    }

    void EnterShootingState()
    {
        EnterNewState(FortressState.Shooting);
    }

    void EnterOpeningWeakSpotState()
    {
        EnterNewState(FortressState.OpeningWeakSpot);
        //Opening animation
        Invoke("EnterVulnerableState", openingWeakSpotDuration);
    }

    void EnterVulnerableState()
    {
        EnterNewState(FortressState.Vulnerable);
        Invoke("EnterClosingWeakSpotState", vulnerableDuration);
    }

    void EnterClosingWeakSpotState()
    {
        EnterNewState(FortressState.ClosingWeakSpot);
        Invoke("EnterShootingState", closingWeakSpotDuration);
    }

    void EnterOpeningCoreState()
    {
        CancelInvoke("EnterClosingWeakSpotState");
        EnterNewState(FortressState.OpeningCore);
        Invoke("EnterOpeningProtectingCore", openingCoreDuration);
    }

    void EnterOpeningProtectingCore()
    {
        EnterNewState(FortressState.ProtectingCore);
        Invoke("EnterClosingCoreState", openingCoreDuration);
    }

    void EnterVulnerableCoreState()
    {
        CancelInvoke("EnterClosingCoreState");
        EnterNewState(FortressState.VulnerableCore);
        Invoke("EnterClosingCoreState", openingCoreDuration);
    }

    void EnterClosingCoreState()
    {
        CancelInvoke("EnterClosingCoreState");
        EnterNewState(FortressState.ClosingCore);
        Invoke("EnterShootingState", closingCoreDuration);
    }

    void EnterAttackPlusState()
    {
        CancelInvoke("EnterShootingState");
        EnterNewState(FortressState.ShootingPlus);
        Invoke("EnterShootingState", attackPlusDuration);
    }

    void EnterDeathState()
    {
        EnterNewState(FortressState.Death);
    }

    public void WeakSpotHit()
    {
        hitSpots++;
        if (hitSpots == 4)
        {
            hitSpots = 0;
            EnterOpeningCoreState();
        }
    }

    public void DealDamage()
    {
        healthPoints--;
        cycleCurrentDamage++;
        Debug.Log(healthPoints);
        if (healthPoints <= 0)
        {
            EnterDeathState();
        }
        if (cycleCurrentDamage > maxDamagePerCycle)
        {
            EnterClosingCoreState();
            Invoke("EnterShootingPlusState", closingCoreDuration);
        }
    }

    public void BreakBarrier()
    {
        EnterVulnerableCoreState();
    }

    void OpenCoreAnimation()
    {
        body.position = bodyFinalTransform.position;
    }

    void CloseCoreAnimation()
    {
        body.position = bodyInitialPosition;
    }

    void EnterNewState(FortressState _state)
    {
        state = _state;
        currentStateEnterTime = Time.time;
    }
}

