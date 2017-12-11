
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidLaser : MonoBehaviour
{

    public enum LaserState
    {
        WarmUp, Waiting, Loading, Shooting, WoundedLoading, WoundedShooting, RecoveringPosition, Opening, Closing, Suspended
    }


    [Header("Laser Movement Parameters")]
    private float fluctuationAmplitude;
    private float fluctuationSpeed;
    private float offset;
    [Header("Laser Shooting Parameters")]
    private float waitingTime;
    private float shootingTime;
    private float laserFadeTIme = 1f;
    [Header("Laser Phases")]
    private float currentStateEnterTime;
    private LaserState state = LaserState.Suspended;

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
    private ParticleSystem.ColorOverLifetimeModule rayParticleColorOverLifetime;
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
        rayParticleColorOverLifetime = rayParticle.colorOverLifetime;
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
        if (state != LaserState.Opening)
        {
            state = LaserState.Opening;
            openingFracJourney = 0;
            startPosition = transform.localPosition;
        }
        bool targetReached = false;
        MovementFunctions.Lerp(1 / aracnoid.unloadTime, transform, ref openingFracJourney, unloadingPosition, startPosition, out targetReached);
    }
    public void CloseWeakSpot()
    {
        if (state != LaserState.Closing)
        {
            state = LaserState.Closing;
            openingFracJourney = 0;
            startPosition = transform.localPosition;
        }
        bool targetReached = false;
        MovementFunctions.Lerp(1 / aracnoid.reloadingTime, transform, ref openingFracJourney, origin, startPosition, out targetReached);
    }

    bool isDoneWarmingUp()
    {
        return Time.time >= offset + currentStateEnterTime;
    }
    bool isDoneWaiting()
    {
        return Time.time >= waitingTime + currentStateEnterTime;
    }
    bool isDoneLoading()
    {
        return Time.time >= aracnoid.loadingTime + currentStateEnterTime;
    }
    bool isDoneWoundedLoading()
    {
        return Time.time >= aracnoid.woundedLoadingTime + currentStateEnterTime;
    }
    bool isDoneShooting()
    {
        return Time.time >= shootingTime + currentStateEnterTime;
    }

    public void StartWaiting()
    {
        EnterNewState(LaserState.Waiting);
        startPosition = origin;
        fluctuatingFracJourney = 0;
    }

    public void StartLoading()
    {
        EnterNewState(LaserState.Loading);
        laserObject.SetActive(true);
        loadingParticle.Play();
    }

    public void StartShooting()
    {
        EnterNewState(LaserState.Shooting);
        laserCollider.enabled = true;
        loadingParticle.Stop();
        laserParticle.Play();
        rayParticle.Play();
    }
    public void StartWoundedShooting()
    {
        EnterNewState(LaserState.WoundedShooting);
        laserCollider.enabled = true;
        loadingParticle.Stop();
        laserParticle.Play();
        rayParticle.Play();
    }

    public void StopShooting()
    {
        EnterNewState(LaserState.RecoveringPosition);
        laserObject.SetActive(false);
        laserCollider.enabled = false;
        laserParticle.Stop();
        rayParticle.Stop();
    }
    public void Suspend()
    {
        EnterNewState(LaserState.Suspended);
        laserObject.SetActive(false);
        laserCollider.enabled = false;
        laserParticle.Stop();
        rayParticle.Stop();
    }

    void SetLaserParticleTimes()
    {
        laserParticleMain.loop = false;
        rayParticleMain.loop = false;
        loadingParticleMain.duration = aracnoid.loadingTime - loadingParticleMain.startLifetime.constant / 10;
        laserParticleMain.duration = shootingTime - laserParticleMain.startLifetime.constant;
        rayParticleMain.startLifetime = shootingTime;
        SetRayGradient(1 - (laserFadeTIme / shootingTime));
    }

    void SetLaserParticleWoundedTimes()
    {
        loadingParticleMain.duration = aracnoid.woundedLoadingTime;
        laserParticleMain.loop = true;
        rayParticleMain.loop = true;
        rayParticleMain.startLifetime = aracnoid.maxWoundedTime;
        SetRayGradient(1 - (laserFadeTIme / aracnoid.maxWoundedTime));
    }

    void SetRayGradient(float fadingPoint)
    {
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(1.0f, fadingPoint), new GradientAlphaKey(0, 1) });
        rayParticleColorOverLifetime.color = grad;
    }

    public void EnableLaser()
    {
        //laserEmitter.SetActive(true);
        EnterNewState(LaserState.WarmUp);
        //SetLaserParticleTimes();
    }
    public void DisableLaser()
    {
        laserObject.SetActive(false);
        startPosition = transform.localPosition;
        fluctuatingFracJourney = 0;
        //state = LaserState.RecoveringPosition;
    }
    public void ForceLoading()
    {
        EnterNewState(LaserState.WoundedLoading);
        SetLaserParticleWoundedTimes();
        StartCoroutine(ForceEnableLaser());
    }
    IEnumerator ForceEnableLaser()
    {
        yield return new WaitForSeconds(aracnoid.woundedLoadingTime);
        StartWoundedShooting();
    }
    public void ForceWaiting()
    {
        Suspend();
    }
    void FixedUpdate()
    {
        if (aracnoid.State == AracnoidEnemy.AracnoidState.Shooting && state != LaserState.WarmUp && state!=LaserState.Suspended)
        {
            Fluctuate();
            if (state == LaserState.RecoveringPosition)
            {
                if (transform.localPosition == origin)
                {
                    Suspend();
                }
            }
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
            case LaserState.WarmUp:
                if (isDoneWarmingUp())
                {

                    StartWaiting();
                }
                break;
            case LaserState.Waiting:
                if (isDoneWaiting())
                {
                    StartLoading();
                }
                break;
            case LaserState.Loading:
                if (isDoneLoading())
                {
                    StartShooting();
                }
                break;
            case LaserState.WoundedLoading:
                if (isDoneLoading())
                {
                    StartShooting();
                }
                break;
            case LaserState.Shooting:
                if (isDoneShooting())
                {
                    StopShooting();
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

    public void SetLaserParameters(AracnoidEnemy.AracnoidLaserCycle.AracnoidLaserParameters cycle)
    {
        fluctuationAmplitude = cycle.fluctuationAmplitude;
        fluctuationSpeed = cycle.fluctuationSpeed;
        offset = cycle.offset;
        waitingTime = cycle.waitingTime;
        shootingTime = cycle.shootingTime;
        SetLaserParticleTimes();
        if (fluctuationAmplitude > 0)
        {
            movingUp = true;
        }
    }
}

