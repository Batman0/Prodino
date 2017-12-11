using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidEnemy : MonoBehaviour
{

    public enum AracnoidState
    {
        Shooting, RecoveringPosition, Unloading, Vulnerable, Reloading, Wounded, Recovery
    }


    [Header("Components")]
    private AracnoidWeakSpot weakSpot;
    public Transform internalLasers;
    public Transform externalLasers;


    [Header("Lasers")]
    public AracnoidLaser[] lasers = new AracnoidLaser[4];
    [SerializeField]
    public AracnoidLaserCycle[] laserCycles;
    public float loadingTime;
    public float woundedLoadingTime;
    [Header("Movement Parameters")]
    public float fluctuationSpeed;
    public Vector3 fluctuationAmplitudes;
    private bool fluctuationDirection = true;
    private Vector3 startPosition;
    private Vector3 origin;
    private Vector3 targetPosition;
    private float journeyPercentage;

    [Header("Fire Parameters")]
    public float gunRotationSpeed;
    public float gunFireRate;

    [Header("Common Laser Shooting Parameters")]
    public int externalRotationsAmount = 3;
    public int internalRotationsAmount = 6;
    private float externalFracRotation;
    private float internalFracRotation;

    [Header("Health Points")]
    public float triggerHealthPoints;
    public float healthPoints;



    [Header("Enemy State Parameters")]
    private float currentStateEnterTime;
    private int currentCycle;
    private int lasersDoneShooting = 0;
    public int maxDamageDuringWound;
    private int currentWoundDamage;
    public float unloadTime;
    public float vulnerableTime;
    public float reloadingTime;
    public float maxWoundedTime;
    public float recoveryTime;
    [Header("Enemy Phase Parameters")]
    public int[] phaseHPLimits;
    private int currentPhase;
    private int currentLaserCyle;


    [Header("Particles")]
    public Transform explosionContainer;
    private List<ParticleSystemManager> explosions = new List<ParticleSystemManager>();

    private AracnoidState state = AracnoidState.Reloading;

    public AracnoidState State
    {
        get { return state; }
    }

    [System.Serializable]
    public class AracnoidLaserCycle
    {
        [SerializeField]
        public AracnoidLasersPatternContainer[] patterns;
        public int cycles;
        public bool random;
        private int currentPattern;
        [System.Serializable]
        public class AracnoidLasersPatternContainer
        {
            public AracnoidLaserParameters leftExternalLaser;
            public AracnoidLaserParameters rightExternalLaser;
            public AracnoidLaserParameters leftInternalLaser;
            public AracnoidLaserParameters rightInternalLaser;
            private AracnoidLaserParameters[] lasersBehaviour = new AracnoidLaserParameters[4];

            public AracnoidLaserParameters[] LaserBehaviour
            {
                get
                {
                    lasersBehaviour[0] = leftExternalLaser;
                    lasersBehaviour[1] = rightExternalLaser;
                    lasersBehaviour[2] = leftInternalLaser;
                    lasersBehaviour[3] = rightInternalLaser;
                    return lasersBehaviour;
                }
            }
        }
        [System.Serializable]

        public struct AracnoidLaserParameters
        {
            [Header("Laser Movement Parameters")]
            public float fluctuationAmplitude;
            public float fluctuationSpeed;
            public float offset;
            [Header("Laser Shooting Parameters")]
            public float waitingTime;
            public float shootingTime;
        }

        public AracnoidLaserParameters[] GetPattern()
        {
            if(random)
            {
                currentPattern = Random.Range(0, patterns.Length);
            }
            else
            {
                if (currentPattern < patterns.Length - 1)
                {
                    currentPattern++;
                }
                else
                {
                    currentPattern = 0;
                }
            }
            return patterns[currentPattern].LaserBehaviour;
        }
    }
    void Awake()
    {
        weakSpot = GetComponentInChildren<AracnoidWeakSpot>();
        startPosition = transform.localPosition;
        origin = transform.localPosition;
        targetPosition = new Vector3(startPosition.x + fluctuationAmplitudes.x, startPosition.y + fluctuationAmplitudes.y, startPosition.z + fluctuationAmplitudes.z);
        for (int i = 0; i < explosionContainer.childCount; i++)
        {
            explosions.Add(explosionContainer.GetChild(i).GetComponent<ParticleSystemManager>());
        }
    }
    void FixedUpdate()
    {
        if (state == AracnoidState.Shooting)
        {
            if (lasersDoneShooting > 3)
            {
                if (currentCycle < laserCycles[currentPhase].cycles - 1)
                {
                    EnableLasers();
                    lasersDoneShooting = 0;
                    currentCycle++;
                }
                else
                {
                    currentLaserCyle++;
                    EnterRecoveringPositionState();
                    currentCycle = 0;
                }
            }
            Fluctuate();
        }
        else if (state == AracnoidState.RecoveringPosition)
        {
            bool targetReached;
            transform.localPosition = MovementFunctions.Lerp(fluctuationSpeed / 100, transform, ref journeyPercentage, origin, startPosition, out targetReached);
            foreach (AracnoidLaser laser in lasers)
            {
                if (laser.State == AracnoidLaser.LaserState.RecoveringPosition)
                {
                    targetReached = false;
                }
            }
            if (targetReached)
            {
                EnterUnloadingState();
            }
        }

        else if (state == AracnoidState.Unloading)
        {
            if (Time.time > currentStateEnterTime + unloadTime)
            {
                EnterVulnerableState();
            }
        }
        else if (state == AracnoidState.Vulnerable)
        {
            if (Time.time > currentStateEnterTime + unloadTime)
            {
                EnterReloadingState();
            }
        }
        else if (state == AracnoidState.Reloading)
        {
            if (Time.time > currentStateEnterTime + reloadingTime)
            {
                EnterShootingState();
            }
        }
        else if (state == AracnoidState.Wounded)
        {
            bool rotationComplete;
            externalLasers.localRotation = MovementFunctions.Rotate(externalLasers, ref externalFracRotation, maxWoundedTime, externalRotationsAmount, false, true, false, out rotationComplete);
            internalLasers.localRotation = MovementFunctions.Rotate(internalLasers, ref internalFracRotation, maxWoundedTime, internalRotationsAmount, false, true, false, out rotationComplete);

            if ((Time.time > currentStateEnterTime + maxWoundedTime) && rotationComplete)
            {
                EnterRecoveryState();
            }
        }
        else if (state == AracnoidState.Recovery)
        {
            //internalLasers.Rotate(internalLaserRotationSpeed * Time.fixedDeltaTime, 0, 0);
            if (Time.time > currentStateEnterTime + recoveryTime)
            {
                EnterReloadingState();
            }
        }
    }
    void EnterRecoveringPositionState()
    {
        EnterNewState(AracnoidState.RecoveringPosition);
        ResetFluctuationParameters();
        DisableLasers();
    }
    void EnterUnloadingState()
    {
        EnterNewState(AracnoidState.Unloading);
        weakSpot.StartBlink();
    }
    void EnterVulnerableState()
    {
        EnterNewState(AracnoidState.Vulnerable);
    }
    void EnterReloadingState()
    {
        EnterNewState(AracnoidState.Reloading);
    }
    void EnterShootingState()
    {
        EnterNewState(AracnoidState.Shooting);
        lasersDoneShooting = 0;
        ResetFluctuationParameters();
        currentCycle = 0;
        EnableLasers();

    }
    void EnterWoundedState()
    {
        EnterNewState(AracnoidState.Wounded);
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].ForceLoading();
        }
        externalFracRotation = 0;
        internalFracRotation = 0;
        //StartCoroutine(ForceEnableLasers());
    }

    void EnterRecoveryState()
    {
        EnterNewState(AracnoidState.Recovery);
        currentWoundDamage = 0;
        weakSpot.WeakSpotRecovery();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].ForceWaiting();
        }
    }
    void DisableLasers()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].DisableLaser();
        }
    }
    void EnableLasers()
    {
        AracnoidLaserCycle.AracnoidLaserParameters[] pattern = laserCycles[currentPhase].GetPattern();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].EnableLaser();
            lasers[i].SetLaserParameters(pattern[i]);

        }
    }
    public void WeakSpotHit()
    {
        EnterWoundedState();
    }
    public void GetDamage()
    {
        if (currentWoundDamage >= maxDamageDuringWound)
        {
            return;
        }
        healthPoints--;
        currentWoundDamage++;
        if (currentPhase < phaseHPLimits.Length && healthPoints < phaseHPLimits[currentPhase])
        {
            currentPhase++;
        }
        Debug.LogWarning("Aracnoid HP: " + healthPoints);
        if (healthPoints <= 0)
        {
            Death();
        }
    }
    public void Fluctuate()
    {
        transform.localPosition = MovementFunctions.Fluctuate(fluctuationSpeed, fluctuationAmplitudes, origin, ref fluctuationDirection, transform, ref journeyPercentage, ref targetPosition, ref startPosition);
    }
    void Death()
    {
        StartCoroutine(Explode(0));
        float timer = 0.0f;
        if (timer < GameManager.instance.delayToRespawnEnemy)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameManager.instance.isBossAlive = false;
        }
    }
    public void LaserDoneShooting()
    {
        lasersDoneShooting++;
    }

    void EnterNewState(AracnoidState _state)
    {
        state = _state;
        currentStateEnterTime = Time.time;
    }

    void ResetFluctuationParameters()
    {
        startPosition = transform.localPosition;
        journeyPercentage = 0;
    }

    IEnumerator Explode(int index)
    {
        yield return new WaitForSeconds(0.5f);
        if (index < explosions.Count)
        {

            explosions[index].PlayAll();
            if (index == 0)
            {
                foreach (AracnoidLaser l in lasers)
                {
                    l.gameObject.SetActive(false);
                }
                StartCoroutine(Explode(++index));

            }
            else if (index == explosions.Count - 1)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
                StartCoroutine(Explode(++index));
        }
    }


}
