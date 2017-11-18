using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossEnemy : MonoBehaviour
{


    enum FortressPhase
    {
        Entry, Shooting, OpeningWeakSpot, Vulnerable, ClosingWeakSpot, OpeningCore, ProtectingCore, VulnerableCore, ClosingCore, ShootingPlus
    }

    [Header("Fortress Parameters")]
    public int healthPoints;

    [Header("Cannons Parameters")]
    public float[] fireRates;
    public float[] bulletSpeeds;
    [SerializeField]
    public CannonPatterns[] cannonPatterns;

    //Cannons
    private int currentBullet;
    private float fireTimer = 0;
    private FortressBullet[] bulletPool;
    private FortressCannon[] cannons = new FortressCannon[4];

    //Child indexes
    private const int CANNON_CHILD_INDEX = 0;

    //Phases and cycles
    private int currentCycle = 0;
    private FortressPhase currentPhase = FortressPhase.Entry;

    //Classes
    [System.Serializable]
    public class FortressCannon : MonoBehaviour
    {
        bool isShooting;
        public void Shoot(FortressBullet bullet, float speed)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.Speed = speed;
        }
    }

    [System.Serializable]
    public class CannonPatterns
    {
        private int currentBulletIndex;
        private int currentPattern = -1;
        [SerializeField]
        public Pattern [] patterns;

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
            if (currentBulletIndex < patterns[currentPattern].pattern.Length-1)
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
            bullet.gameObject.SetActive(false);
        }
        for(int i =0; i<cannons.Length; i++)
        {
            GameObject cannon = transform.GetChild(CANNON_CHILD_INDEX).GetChild(i).gameObject;
            cannon.AddComponent<FortressCannon>();
            cannons[i] = cannon.GetComponent<FortressCannon>();
        }
    }

    void FixedUpdate()
    {
        currentPhase = FortressPhase.Shooting;
        if (currentPhase == FortressPhase.Shooting)
        {
            TryShooting();
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
            EnterOpeningWeakSpotPhase();
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

    void EnterOpeningWeakSpotPhase()
    {
        currentPhase = FortressPhase.Entry;
    }
}
