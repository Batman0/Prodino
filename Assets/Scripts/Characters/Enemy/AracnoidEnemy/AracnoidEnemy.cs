using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidEnemy : MonoBehaviour
{

    public enum AracnoidState
    {
        Shooting, RecoveringPosition, Unloading, Vulnerable, Reloading, Wounded, Recovery
    }


    [Header("Lasers")]
    public AracnoidLaser[] lasers = new AracnoidLaser[4];
    [Header("Movement Parameters")]
    public float fluctuationSpeed;
    public Vector3 fluctuationAmplitudes;
    [Header("Movement Parameters")]
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
    public float maxCycles;
    public float maxDamageDuringWound;
    public float unloadTime;
    public float vulnerableTime;
    public float reloadingTime;
    public float maxWoundedTime;
    public float recoveryTime;


    [Header("Children")]
    public Transform explosion;
    //[HideInInspector]
    private AracnoidState state = AracnoidState.Reloading;

    //Boss Loop Parameters
    private int currentLaser;
    private int lasersDoneShooting = 0;
    private float currentWaitingTime;
    private float currentLoadTime;
    private float currentShootingTime;
    private float currentCycle;
    private float currentStateEnterTime;
    private int currentWoundDamage;
    private AracnoidWeakSpot weakSpot;
    //Fluctuation Parameters
    private bool fluctuationDirection = true;
    private Vector3 startPosition;
    private Vector3 origin;
    private Vector3 targetPosition;
    private float fracJourney;

    //Children Parameters
    private int headChildIndex = 0;
    private int gunChildIndex = 1;
    private int lasersChildIndex = 2;
    private Transform internalLasers;
    private Transform externalLasers;

    public AracnoidState State
    {
        get { return state; }
    }


    void Awake()
    {
        weakSpot = GetComponentInChildren<AracnoidWeakSpot>();
        startPosition = transform.localPosition;
        origin = transform.localPosition;
        targetPosition = new Vector3(startPosition.x + fluctuationAmplitudes.x, startPosition.y + fluctuationAmplitudes.y, startPosition.z + fluctuationAmplitudes.z);
        externalLasers = transform.GetChild(lasersChildIndex).GetChild(1);
        internalLasers = transform.GetChild(lasersChildIndex).GetChild(0);
    }
    void FixedUpdate()
    {
        if (state == AracnoidState.Shooting)
        {
            if (lasersDoneShooting > 3)
            {
                if (currentCycle < maxCycles - 1)
                {
                    EnableLasers();
                    currentCycle++;
                }
                else
                {
                    EnterRecoveringPositionState();
                    currentCycle = 0;
                }
                lasersDoneShooting = 0;
            }
            Fluctuate();
        }
        else if (state == AracnoidState.RecoveringPosition)
        {
            bool targetReached;
            transform.localPosition = MovementFunctions.Lerp(fluctuationSpeed / 100, transform, ref fracJourney, origin, startPosition, out targetReached);
            foreach (AracnoidLaser laser in lasers)
            {
                if (laser.State == AracnoidLaser.LaserState.recoveringPosition)
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
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].EnableLaser();
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
        Debug.LogWarning("Aracnoid HP: " + healthPoints);
        if (healthPoints <= 0)
        {
            Death();
        }
    }
    public void Fluctuate()
    {
        transform.localPosition = MovementFunctions.Fluctuate(fluctuationSpeed, fluctuationAmplitudes, origin, ref fluctuationDirection, transform, ref fracJourney, ref targetPosition, ref startPosition);
    }
    void Death()
    {
        //gameObject.SetActive(false);
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
        fracJourney = 0;
    }

    IEnumerator Explode(int index)
    {
        yield return new WaitForSeconds(0.5f);
        if (index < explosion.childCount)
        {

            explosion.GetChild(index).gameObject.SetActive(true);
            if (index == 0)
            {
                foreach (AracnoidLaser l in lasers)
                {
                    l.gameObject.SetActive(false);
                }
                StartCoroutine(Explode(++index));

            }
            else if (index == explosion.childCount - 1)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
                StartCoroutine(Explode(++index));
        }
    }
}
