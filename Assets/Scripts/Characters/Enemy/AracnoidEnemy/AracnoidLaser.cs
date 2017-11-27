
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidLaser : MonoBehaviour
{

    public enum LaserState
    {
        waiting, loading, shooting, woundedLoading, woundedShooting, recoveringPosition, opening, closing, suspended
    }

    private bool switchable;
    private float laserWidth;
    private float laserHeight;
    private float currentStateEnterTime;
    [Header("Laser Movement Parameters")]
    public float fluctuationAmplitude;
    public float fluctuationSpeed;
    [Header("Laser Shooting Parameters")]
    public float waitingTime;
    public float loadingTime;
    public float shootingTime;
    public float woundedLoadingTime;
    [Header("Laser Phases")]
    //public Transform unloadingTransform;
    public Transform woundedInitialTransform;
    public Transform woundedFloatingTransform;
    private LaserState state = LaserState.suspended;
    [Header("Phases Parameters")]
    public float woundedFloatingSpeed;

    public LaserState State
    {
        get
        {
            return state;
        }
    }

    private AracnoidEnemy aracnoid;
    private bool fluctuating;
    private bool movingUp;
    private Vector3 startPosition;
    private Vector3 origin;
    private Vector3 targetPosition;
    private Vector3 unloadingPosition;
    private Vector3 lastPositionBeforeClosing;
    private Transform shootingPosition;
    private float fluctuatingFracJourney;
    private float openingFracJourney;
    private GameObject laserObject;
    private Collider laserCollider;
    private ParticleSystem loadingParticle;
    private ParticleSystem laserParticle;
    private ParticleSystem rayParticle;
    private ParticleSystem.MainModule loadingParticleMain;
    private ParticleSystem.MainModule laserParticleMain;
    private ParticleSystem.MainModule rayParticleMain;
    private bool isMovingLasers;
    private int particleChildIndex = 1;
    private int targetPositionChildIndex = 2;
    private float baseLoadingParticleTime = 2.5f;
    private Quaternion initialRotation;
    public void Awake()
    {
        InitializeComponents();
        InitializePositions();
        InitializeParticles();
    }
    void InitializeComponents()
    {
        aracnoid = GetComponentInParent<AracnoidEnemy>();
        laserObject = transform.GetChild(0).gameObject;
        laserObject.SetActive(false);
        laserCollider = laserObject.GetComponent<Collider>();
        laserCollider.enabled = false;
    }

    void InitializePositions()
    {
        if (fluctuationAmplitude > 0)
        {
            movingUp = true;
        }
        startPosition = transform.localPosition;
        origin = transform.localPosition;
        targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + fluctuationAmplitude);
        initialRotation = transform.rotation;
        Transform unloadingTransform = transform.GetChild(targetPositionChildIndex);
        unloadingTransform.parent = transform.parent;
        unloadingPosition = new Vector3(unloadingTransform.localPosition.x, unloadingTransform.localPosition.y, unloadingTransform.localPosition.z);
    }

    void InitializeParticles()
    {
        loadingParticle = transform.GetChild(particleChildIndex).GetChild(0).GetComponent<ParticleSystem>();
        loadingParticleMain = loadingParticle.main;
        laserParticle = transform.GetChild(particleChildIndex).GetChild(1).GetComponent<ParticleSystem>();
        laserParticleMain = laserParticle.main;
        rayParticle = transform.GetChild(particleChildIndex).GetChild(2).GetComponent<ParticleSystem>();
        rayParticleMain = rayParticle.main;
        SetLaserParticleTimes();
    }
    public void Fluctuate()
    {
        if (fluctuationSpeed != 0)
        {
            transform.localPosition = MovementFunctions.Fluctuate(fluctuationSpeed, new Vector3(0, 0, fluctuationAmplitude), origin, ref movingUp, transform, ref fluctuatingFracJourney, ref targetPosition, ref startPosition);
        }
    }
    public void OpenWeakSpot()
    {
        if (state != LaserState.opening)
        {
            state = LaserState.opening;
            openingFracJourney = 0;
            startPosition = transform.localPosition;
        }
        bool targetReached = false;
        MovementFunctions.Lerp(1 / aracnoid.unloadTime, transform, ref openingFracJourney, unloadingPosition, startPosition, out targetReached);
    }
    public void CloseWeakSpot()
    {
        if (state != LaserState.closing)
        {
            state = LaserState.closing;
            openingFracJourney = 0;
            startPosition = transform.localPosition;
        }
        bool targetReached = false;
        MovementFunctions.Lerp(1 / aracnoid.reloadingTime, transform, ref openingFracJourney, origin, startPosition, out targetReached);
    }
    bool isDoneWaiting()
    {
        return Time.time >= waitingTime + currentStateEnterTime;
    }
    bool isDoneLoading()
    {
        return Time.time >= loadingTime + currentStateEnterTime;
    }
    bool isDoneWoundedLoading()
    {
        return Time.time >= woundedLoadingTime + currentStateEnterTime;
    }
    bool isDoneShooting()
    {
        return Time.time >= shootingTime + currentStateEnterTime;
    }

    public void StartLoading()
    {
        EnterNewState(LaserState.loading);
        laserObject.SetActive(true);
        loadingParticle.Play();
    }

    public void StartShooting()
    {
        EnterNewState(LaserState.shooting);
        laserCollider.enabled = true;
        loadingParticle.Stop();
        laserParticle.Play();
        rayParticle.Play();
    }
    public void StartWoundedShooting()
    {
        EnterNewState(LaserState.woundedShooting);
        laserCollider.enabled = true;
        loadingParticle.Stop();
        laserParticle.Play();
        rayParticle.Play();
    }

    public void Suspend()
    {
        EnterNewState(LaserState.suspended);
        laserObject.SetActive(false);
        laserCollider.enabled = false;
        laserParticle.Stop();
        rayParticle.Stop();
    }

    void SetLaserParticleTimes()
    {
        laserParticleMain.loop = false;
        rayParticleMain.loop = false;
        loadingParticleMain.duration = loadingTime - loadingParticleMain.startLifetime.constant / 10;
        laserParticleMain.duration = shootingTime - laserParticleMain.startLifetime.constant;
        rayParticleMain.startLifetime = shootingTime;
    }

    void SetLaserParticleWoundedTimes()
    {
        loadingParticleMain.duration = woundedLoadingTime;
        laserParticleMain.loop = true;
        rayParticleMain.loop = true;
        rayParticleMain.startLifetime = aracnoid.maxWoundedTime;
    }

    public void EnableLaser()
    {
        //laserEmitter.SetActive(true);
        EnterNewState(LaserState.waiting);
        startPosition = origin;
        fluctuatingFracJourney = 0;
        //SetLaserParticleTimes();
    }
    public void DisableLaser()
    {
        laserObject.SetActive(false);
        startPosition = transform.localPosition;
        fluctuatingFracJourney = 0;
        state = LaserState.recoveringPosition;
    }
    public void ForceLoading()
    {
        EnterNewState(LaserState.woundedLoading);
        SetLaserParticleWoundedTimes();
        StartCoroutine(ForceEnableLaser());
    }
    IEnumerator ForceEnableLaser()
    {
        yield return new WaitForSeconds(woundedLoadingTime);
        StartWoundedShooting();
    }
    public void ForceWaiting()
    {
        SetLaserParticleTimes();
        Suspend();
    }
    void FixedUpdate()
    {
        if (aracnoid.State == AracnoidEnemy.AracnoidState.Shooting)
        {
            Fluctuate();
        }
        //else if (aracnoid.State == AracnoidEnemy.AracnoidState.RecoveringPosition)
        //{
        //    if (state == LaserState.recoveringPosition)
        //    {
        //        bool targetReached;
        //        transform.localPosition = MovementFunctions.Lerp(fluctuationSpeed * 10, transform, ref fluctuatingFracJourney, origin, startPosition, out targetReached);
        //        if (targetReached)
        //        {
        //            state = LaserState.suspended;
        //        }
        //    }

        //}
        else if (aracnoid.State == AracnoidEnemy.AracnoidState.Unloading || aracnoid.State == AracnoidEnemy.AracnoidState.RecoveringPosition)
        {
            OpenWeakSpot();
        }
        else if (aracnoid.State == AracnoidEnemy.AracnoidState.Reloading)
        {
            CloseWeakSpot();
        }
    }

    void Update()
    {
        switch (state)
        {
            case LaserState.waiting:
                if (isDoneWaiting())
                {
                    StartLoading();
                }
                break;
            case LaserState.loading:
                if (isDoneLoading())
                {
                    StartShooting();
                }
                break;
            case LaserState.woundedLoading:
                if (isDoneLoading())
                {
                    StartShooting();
                }
                break;
            case LaserState.shooting:
                if (isDoneShooting())
                {
                    Suspend();
                    aracnoid.LaserDoneShooting();
                }
                break;
            default: break;
        }
    }
    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }

    void EnterNewState(LaserState _state)
    {
        currentStateEnterTime = Time.time;
        state = _state;
    }
}

