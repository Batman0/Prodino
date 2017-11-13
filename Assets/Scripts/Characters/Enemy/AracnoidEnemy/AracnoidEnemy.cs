using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidEnemy : MonoBehaviour
{

    public enum AracnoidState
    {
        shooting = 0, unloading = 1, vulnerable = 2, reloading = 3, wounded = 4, recovery = 5
    }


    [Header("Lasers")]
    public AracnoidLaser[] lasers = new AracnoidLaser[4];
    [Header("Movement Parameters")]
    public float yFluctuationSpeed;
    public float yFluctuationAmplitude;
    [Header("Movement Parameters")]
    public float gunRotationSpeed;
    public float gunFireRate;
    [Header("Common Laser Shooting Parameters")]
    public float externalLaserRotationSpeed;
    public float internalLaserRotationSpeed;

    [Header("Health Points")]
    public float triggerHealthPoints;
    public float healthPoints;

    [Header("Enemy State Parameters")]
    public float maxCycles;
    public float maxDamageDuringWound;
    public float unloadTime;
    public float vulnerableTime;
    public float reloadingTime;
    public float maxWoundedTime;
    public float recoveryTime;

    //[HideInInspector]
    public AracnoidState state = AracnoidState.reloading;

    //Boss Loop Parameters
    private int currentLaser;
    private float currentWaitingTime;
    private float currentLoadTime;
    private float currentShootingTime;
    private float currentCycle;
    private float currentStateEnterTime;
    private int currentWoundDamage;
    private AracnoidWeakSpot weakSpot;
    //Fluctuation Parameters
    private bool movingUp;
    private bool lastCycle;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float fracJourney;

    //Children Parameters
    private int headChildIndex=0;
    private int gunChildIndex=1;
    private int lasersChildIndex=2;
    private Transform internalLasers;
    private Transform externalLasers;


    void Awake()
    {
        weakSpot = GetComponentInChildren<AracnoidWeakSpot>();
        initialPosition = transform.position;
        targetPosition = new Vector3(initialPosition.x, initialPosition.y + yFluctuationAmplitude, initialPosition.z);
        if (yFluctuationAmplitude > 0)
        {
            movingUp = true;
        }
        externalLasers = transform.GetChild(lasersChildIndex).GetChild(1);
        internalLasers = transform.GetChild(lasersChildIndex).GetChild(0);
    }
    void FixedUpdate()
    {
        if (state == AracnoidState.shooting)
        {
            bool cycleComplete = false;
            for (int i = 0; i < lasers.Length; i++)
            {
                if (!lastCycle)
                {
                    if (lasers[i].state == AracnoidLaser.LaserState.waiting)
                    {
                        if (lasers[i].isDoneWaiting())
                        {
                            LaserLoad(i);
                            if (i == lasers.Length - 1 && currentCycle == maxCycles - 1)
                            {
                                lastCycle = true;
                            }
                        };
                    }
                }
                if (lasers[i].state == AracnoidLaser.LaserState.loading)
                {
                    if (lasers[i].isDoneLoading())
                    {
                        LaserShoot(i);
                    };
                }

                else if (lasers[i].state == AracnoidLaser.LaserState.shooting)
                {
                    if (lasers[i].isDoneShooting())
                    {
                        LaserWait(i);
                        if (i == lasers.Length - 1)
                        {
                            cycleComplete = true;
                        }
                    };
                }
            }
            if (cycleComplete)
            {
                if (currentCycle < maxCycles - 1)
                {
                    currentCycle++;
                }
                else
                {
                    EnterUnloadingState();
                    currentCycle = 0;
                    lastCycle = false;
                }
            }

            Fluctuate();

        }
        else if (state == AracnoidState.unloading)
        {
            if (Time.time > currentStateEnterTime + unloadTime)
            {
                EnterVulnerableState();
            }
        }
        else if (state == AracnoidState.vulnerable)
        {
            if (Time.time > currentStateEnterTime + unloadTime)
            {
                EnterReloadingState();
            }
        }
        else if (state == AracnoidState.reloading)
        {
            if (Time.time > currentStateEnterTime + reloadingTime)
            {
                EnterShootingState();
            }
        }
        else if (state == AracnoidState.wounded)
        {
            externalLasers.Rotate(0, externalLaserRotationSpeed*Time.fixedDeltaTime, 0);
            internalLasers.Rotate(0, internalLaserRotationSpeed*Time.fixedDeltaTime, 0);
            if (Time.time > currentStateEnterTime + maxWoundedTime)
            {
                EnterRecoveryState();
            }
        }
        else if (state == AracnoidState.recovery)
        {
            internalLasers.Rotate(internalLaserRotationSpeed * Time.fixedDeltaTime, 0, 0);
            if (Time.time > currentStateEnterTime + maxWoundedTime)
            {
                EnterRecoveryState();
            }
        }
    }
    void LaserLoad(int laserIndex)
    {
    }
    void LaserShoot(int laserIndex)
    {
    }
    void LaserWait(int laserIndex)
    {
    }
    void EnterUnloadingState()
    {
        state = AracnoidState.unloading;
        currentStateEnterTime = Time.time;
        DisableLasers();
        weakSpot.StartBlink();
    }
    void EnterVulnerableState()
    {
        state = AracnoidState.vulnerable;
        currentStateEnterTime = Time.time;
    }
    void EnterReloadingState()
    {
        state = AracnoidState.reloading;
        currentStateEnterTime = Time.time;
    }
    void EnterShootingState()
    {
        state = AracnoidState.shooting;
        currentStateEnterTime = Time.time;
        currentCycle = 0;
        EnableLasers();

    }
    void EnterWoundedState()
    {
        state = AracnoidState.wounded;
        currentStateEnterTime = Time.time;
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].ForceLoading();
        }
        //StartCoroutine(ForceEnableLasers());
    }
    //IEnumerator ForceEnableLasers()
    //{
    //    yield return new WaitForSeconds(lasers[0].loadingTime);
    //    for (int i = 0; i < lasers.Length; i++)
    //    {
    //        lasers[i].ForceEnableLaser();
    //    }
    //}
    void EnterRecoveryState()
    {
        weakSpot.WeakSpotRecovery();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].ForceWaiting();
        }
        //TEMP
        EnterReloadingState();

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
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].EnableLaser(Time.time);
        }
    }
    public void WeakSpotHit()
    {
        EnterWoundedState();
    }
    public void GetDamage()
    {
        healthPoints--;
        currentWoundDamage++;
        Debug.LogWarning("Aracnoid HP: " + healthPoints);
        if (healthPoints <= 0)
        {
            gameObject.SetActive(false);
        }
        else if (currentWoundDamage >= maxDamageDuringWound)
        {
            currentWoundDamage = 0;
            EnterRecoveryState();
        }
    }
    public void Fluctuate()
    {
        float stepLength = Time.fixedDeltaTime * yFluctuationSpeed;
        if (yFluctuationSpeed != 0)
        {
            if (movingUp)
            {
                if (transform.position.y >= initialPosition.y)
                {
                    fracJourney += stepLength;
                    transform.position = Vector3.Lerp(initialPosition, targetPosition, fracJourney);
                    if (transform.position.y >= targetPosition.y)
                    {
                        movingUp = false;
                        transform.position = targetPosition;
                        fracJourney = 0;
                    }
                }
                else
                {
                    fracJourney += stepLength;
                    transform.position = Vector3.Lerp(targetPosition, initialPosition, fracJourney);
                    if (transform.position.y >= initialPosition.y)
                    {
                        transform.position = initialPosition;
                        targetPosition = new Vector3(initialPosition.x, initialPosition.y + Mathf.Abs(yFluctuationAmplitude), initialPosition.z);
                        fracJourney = 0;
                    }
                }
            }
            else
            {
                if (transform.position.y <= initialPosition.y)
                {
                    fracJourney += stepLength;
                    transform.position = Vector3.Lerp(initialPosition, targetPosition, fracJourney);
                    if (transform.position.y <= targetPosition.y)
                    {
                        movingUp = true;
                        transform.position = targetPosition;
                        fracJourney = 0;
                    }
                }
                else
                {
                    fracJourney += stepLength;
                    transform.position = Vector3.Lerp(targetPosition, initialPosition, fracJourney);
                    if (transform.position.y <= initialPosition.y)
                    {
                        transform.position = initialPosition;
                        targetPosition = new Vector3(initialPosition.x, initialPosition.y - Mathf.Abs(yFluctuationAmplitude), initialPosition.z);
                        fracJourney = 0;
                    }
                }
            }
        }
    }

}
