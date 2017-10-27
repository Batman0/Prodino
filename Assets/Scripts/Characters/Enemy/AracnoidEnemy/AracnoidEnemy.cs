using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidEnemy : MonoBehaviour
{
    public enum LaserState
    {
        waiting = 0, loading = 1, shooting = 2, alwaysShooting = 3
    }

    public enum AracnoidState
    {
        shooting = 0, unloading = 1, reloading = 2, wounded = 3, recovery = 4
    }
    [System.Serializable]
    public class AracnoidLaser
    {

        private bool switchable;
        private float laserWidth;
        private float laserHeight;
        private float resetTime;
        public GameObject laserEmitter;
        [Header("Laser Movement Parameters")]
        public float fluctuationAmplitude;
        public float fluctuationSpeed;
        [Header("Laser Shooting Parameters")]
        public float waitingTime;
        public float loadingTime;
        public float shootingTime;
        public float offset;
        private float initialOffset;
        private Material laserEmitterMaterial;
        private bool movingUp;
        private Vector3 initialPosition;
        private Vector3 targetPosition;
        private float fracJourney;

        [HideInInspector]
        public LaserState state = LaserState.shooting;
        private GameObject laserObject;

        public void InitializeLaser()
        {
            laserObject = laserEmitter.transform.GetChild(0).gameObject;
            laserObject.SetActive(false);
            loadingTime += waitingTime;
            shootingTime += loadingTime;
            initialOffset = offset;
            laserEmitterMaterial = laserEmitter.GetComponent<MeshRenderer>().material;
            initialPosition = laserEmitter.transform.localPosition;
            targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + fluctuationAmplitude);
            if (fluctuationAmplitude > 0)
            {
                movingUp = true;
            }
        }
        public void Fluctuate(float deltaTime)
        {
            float stepLength = deltaTime * fluctuationSpeed;
            Vector3 previousPosition = laserEmitter.transform.localPosition;
            if (fluctuationSpeed != 0)
            {
                if (movingUp)
                {
                    if (laserEmitter.transform.localPosition.z >= initialPosition.z)
                    {
                        fracJourney += stepLength;
                        laserEmitter.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, fracJourney);
                        if (laserEmitter.transform.localPosition.z >= targetPosition.z)
                        {
                            movingUp = false;
                            laserEmitter.transform.localPosition = targetPosition;
                            fracJourney = 0;
                        }
                    }
                    else
                    {
                        fracJourney += stepLength;
                        laserEmitter.transform.localPosition = Vector3.Lerp(targetPosition, initialPosition, fracJourney);
                        if (laserEmitter.transform.localPosition.z >= initialPosition.z)
                        {
                            laserEmitter.transform.localPosition = initialPosition;
                            targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + Mathf.Abs(fluctuationAmplitude));
                            fracJourney = 0;
                        }
                    }
                }
                else
                {
                    if (laserEmitter.transform.localPosition.z <= initialPosition.z)
                    {
                        fracJourney += stepLength;
                        laserEmitter.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, fracJourney);
                        if (laserEmitter.transform.localPosition.z <= targetPosition.z)
                        {
                            movingUp = true;
                            laserEmitter.transform.localPosition = targetPosition;
                            fracJourney = 0;
                        }
                    }
                    else
                    {
                        fracJourney += stepLength;
                        laserEmitter.transform.localPosition = Vector3.Lerp(targetPosition, initialPosition, fracJourney);
                        if (laserEmitter.transform.localPosition.z <= initialPosition.z)
                        {
                            laserEmitter.transform.localPosition = initialPosition;
                            targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z - Mathf.Abs(fluctuationAmplitude));
                            fracJourney = 0;
                        }
                    }
                }
                laserEmitter.transform.localPosition = new Vector3(previousPosition.x, previousPosition.y, laserEmitter.transform.localPosition.z);
            }
        }
        public bool isDoneWaiting(float time)
        {
            if (time >= waitingTime + resetTime + initialOffset)
            {
                laserEmitterMaterial.color = Color.red;
                state = LaserState.loading;
                return true;
            }
            return false;
        }
        public bool isDoneLoading(float time)
        {
            if (time >= loadingTime + resetTime + initialOffset)
            {
                laserEmitterMaterial.color = Color.yellow;
                laserObject.SetActive(true);
                state = LaserState.shooting;
                return true;
            }
            return false;
        }
        public bool isDoneShooting(float time)
        {
            if (time >= shootingTime + resetTime + initialOffset)
            {
                laserObject.SetActive(false);
                state = LaserState.waiting;
                resetTime = resetTime + shootingTime + initialOffset;
                initialOffset = 0;

                return true;
            }
            return false;
        }
        public void EnableLaser(float time)
        {
            //laserEmitter.SetActive(true);
            state = LaserState.waiting;
            initialOffset = offset;
            resetTime = time;

        }
        public void DisableLaser()
        {
            //laserEmitter.SetActive(false);
            laserObject.SetActive(false);
            state = LaserState.waiting;
        }
        public void ForceLoading()
        {
            //laserObject.SetActive(true);
            state = LaserState.alwaysShooting;
            laserEmitterMaterial.color = Color.red;
        }
        public void ForceEnableLaser()
        {
            laserObject.SetActive(true);
            laserEmitterMaterial.color = Color.yellow;

            //state = LaserState.alwaysShooting;
        }
    }

    [Header("Lasers")]
    [SerializeField]
    AracnoidLaser[] lasers = new AracnoidLaser[4];
    [Header("Movement Parameters")]
    public float yFluctuationSpeed;
    public float yFluctuationAmplitude;
    [Header("Movement Parameters")]
    public float gunRotationSpeed;
    public float gunFireRate;
    [Header("Common Laser Shooting Parameters")]
    public float laserWidth;
    public float laserHeight;

    [Header("Health Points")]
    public float triggerHealthPoints;
    public float healthPoints;

    [Header("Enemy State Parameters")]
    public float maxCycles;
    public float maxDamageDuringWound;
    public float reloadingTime;
    public float unloadTime;
    public float maxWoundedTime;
    public float recoveryTime;
    [HideInInspector]
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
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float fracJourney;

    void Awake()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].InitializeLaser();
        }
        weakSpot = GetComponentInChildren<AracnoidWeakSpot>();
        initialPosition = transform.position;
        targetPosition = new Vector3(initialPosition.x, initialPosition.y + yFluctuationAmplitude, initialPosition.z );
        if (yFluctuationAmplitude > 0)
        {
            movingUp = true;
        }
    }
    void FixedUpdate()
    {
        if (state == AracnoidState.shooting)
        {
            bool cycleComplete = false;
            for (int i = 0; i < lasers.Length; i++)
            {
                if (lasers[i].state == LaserState.waiting)
                {
                    if (lasers[i].isDoneWaiting(Time.time))
                    {
                        LaserLoad(i);
                    };
                }
                else if (lasers[i].state == LaserState.loading)
                {
                    if (lasers[i].isDoneLoading(Time.time))
                    {
                        LaserShoot(i);
                    };
                }
                else if (lasers[i].state == LaserState.shooting)
                {
                    if (lasers[i].isDoneShooting(Time.time))
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
                }
            }
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].Fluctuate(Time.fixedDeltaTime);
            }
            Fluctuate();

        }
        else if (state == AracnoidState.unloading)
        {
            if (Time.time > currentStateEnterTime + unloadTime)
            {
                EnterReloadingState();
            }
            else
            {
                //Lerp laser positions out
            }
        }
        else if (state == AracnoidState.reloading)
        {
            if (Time.time > currentStateEnterTime + reloadingTime)
            {
                EnterShootingState();
            }
            else
            {
                //Lerp laser positions in
            }
        }
        else if (state == AracnoidState.wounded)
        {
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
        StartCoroutine(ForceEnableLasers());
    }
    IEnumerator ForceEnableLasers()
    {
        yield return new WaitForSeconds(lasers[0].loadingTime);
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].ForceEnableLaser();
        }
    }
    void EnterRecoveryState()
    {
        weakSpot.WeakSpotRecovery();
        //TEMP
        EnterShootingState();

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
    public bool CanBeWounded()
    {
        if (state == AracnoidState.unloading || state == AracnoidState.reloading)
        {
            return true;
        }
        return false;
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
                if ( transform.position.y >= initialPosition.y)
                {
                    fracJourney += stepLength;
                     transform.position = Vector3.Lerp(initialPosition, targetPosition, fracJourney);
                    if ( transform.position.y >= targetPosition.y)
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
                    if ( transform.position.y >= initialPosition.y)
                    {
                         transform.position = initialPosition;
                        targetPosition = new Vector3(initialPosition.x, initialPosition.y + Mathf.Abs(yFluctuationAmplitude), initialPosition.z);
                        fracJourney = 0;
                    }
                }
            }
            else
            {
                if ( transform.position.y <= initialPosition.y)
                {
                    fracJourney += stepLength;
                     transform.position = Vector3.Lerp(initialPosition, targetPosition, fracJourney);
                    if ( transform.position.y <= targetPosition.y)
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
                    if ( transform.position.y <= initialPosition.y)
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
