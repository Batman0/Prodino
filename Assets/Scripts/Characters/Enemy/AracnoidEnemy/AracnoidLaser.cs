
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidLaser : MonoBehaviour
{

    public enum LaserState
    {
        waiting = 0, loading = 1, shooting = 2, alwaysShooting = 3, opening = 4, closing = 5
    }

    private bool switchable;
    private float laserWidth;
    private float laserHeight;
    private float resetTime;
    [Header("Laser Movement Parameters")]
    public float fluctuationAmplitude;
    public float fluctuationSpeed;
    [Header("Laser Shooting Parameters")]
    public float waitingTime;
    public float loadingTime;
    public float shootingTime;
    public float offset;
    [Header("Laser Positions in Phases")]
    //public Transform unloadingTransform;
    public Transform woundedInitialTransform;
    public Transform woundedFloatingTransform;
    [Header("Phases Parameters")]
    public float woundedFloatingSpeed;

    private float initialOffset;
    private AracnoidEnemy aracnoid;
    private Material laserEmitterMaterial;
    private bool movingUp;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 unloadingPosition;
    private Vector3 woundedInitialPosition;
    private Vector3 woundedFloatingPosition;
    private Vector3 lastPositionBeforeOpening;
    private Vector3 lastPositionBeforeClosing;
    private Transform shootingPosition;
    private float fluctuatingFracJourney;
    private float openingFracJourney;
    [HideInInspector]
    public LaserState state = LaserState.shooting;
    private GameObject laserObject;
    private GameObject laserEmitter;
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

    public void Awake()
    {
        laserEmitter = gameObject;
        laserObject = laserEmitter.transform.GetChild(0).gameObject;
        laserObject.SetActive(false);
        loadingTime += waitingTime;
        shootingTime += loadingTime;
        initialOffset = offset;
        laserEmitterMaterial = laserEmitter.GetComponent<MeshRenderer>().material;
        initialPosition = laserEmitter.transform.position;
        targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + fluctuationAmplitude);
        if (fluctuationAmplitude > 0)
        {
            movingUp = true;
        }
        Transform unloadingTransform = transform.GetChild(targetPositionChildIndex);
        unloadingPosition = new Vector3(unloadingTransform.position.x, unloadingTransform.position.y, unloadingTransform.position.z);
        //woundedFloatingPosition = new Vector3(woundedFloatingTransform.position.x, woundedFloatingTransform.position.y, woundedFloatingTransform.position.z);
        //woundedInitialPosition = new Vector3(woundedInitialTransform.position.x, woundedInitialTransform.position.y, woundedInitialTransform.position.z);
        aracnoid = GetComponentInParent<AracnoidEnemy>();
        laserCollider = laserObject.GetComponent<Collider>();
        laserCollider.enabled = false;
        loadingParticle = laserEmitter.transform.GetChild(particleChildIndex).GetChild(0).GetComponent<ParticleSystem>();
        loadingParticleMain = loadingParticle.main;
        laserParticle = laserEmitter.transform.GetChild(particleChildIndex).GetChild(1).GetComponent<ParticleSystem>();
        laserParticleMain = laserParticle.main;
        rayParticle = laserEmitter.transform.GetChild(particleChildIndex).GetChild(2).GetComponent<ParticleSystem>();
        rayParticleMain = rayParticle.main;
        SetLaserParticleTimes();
    }
    public void Fluctuate()
    {
        float stepLength = Time.fixedDeltaTime * fluctuationSpeed;
        Vector3 previousPosition = laserEmitter.transform.position;
        if (fluctuationSpeed != 0)
        {
            if (movingUp)
            {
                if (laserEmitter.transform.position.z >= initialPosition.z)
                {
                    fluctuatingFracJourney += stepLength;
                    laserEmitter.transform.position = Vector3.Lerp(initialPosition, targetPosition, fluctuatingFracJourney);
                    if (laserEmitter.transform.position.z >= targetPosition.z)
                    {
                        movingUp = false;
                        laserEmitter.transform.position = targetPosition;
                        fluctuatingFracJourney = 0;
                    }
                }
                else
                {
                    fluctuatingFracJourney += stepLength;
                    laserEmitter.transform.position = Vector3.Lerp(targetPosition, initialPosition, fluctuatingFracJourney);
                    if (laserEmitter.transform.position.z >= initialPosition.z)
                    {
                        laserEmitter.transform.position = initialPosition;
                        targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + Mathf.Abs(fluctuationAmplitude));
                        fluctuatingFracJourney = 0;
                    }
                }
            }
            else
            {
                if (laserEmitter.transform.position.z <= initialPosition.z)
                {
                    fluctuatingFracJourney += stepLength;
                    laserEmitter.transform.position = Vector3.Lerp(initialPosition, targetPosition, fluctuatingFracJourney);
                    if (laserEmitter.transform.position.z <= targetPosition.z)
                    {
                        movingUp = true;
                        laserEmitter.transform.position = targetPosition;
                        fluctuatingFracJourney = 0;
                    }
                }
                else
                {
                    fluctuatingFracJourney += stepLength;
                    laserEmitter.transform.position = Vector3.Lerp(targetPosition, initialPosition, fluctuatingFracJourney);
                    if (laserEmitter.transform.position.z <= initialPosition.z)
                    {
                        laserEmitter.transform.position = initialPosition;
                        targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z - Mathf.Abs(fluctuationAmplitude));
                        fluctuatingFracJourney = 0;
                    }
                }
            }
            laserEmitter.transform.position = new Vector3(previousPosition.x, previousPosition.y, laserEmitter.transform.position.z);
        }
    }
    public void OpenWeakSpot()
    {
        if (state != LaserState.opening)
        {
            state = LaserState.opening;
            openingFracJourney = 0;
            lastPositionBeforeOpening = transform.position;
        }
        openingFracJourney += Time.fixedDeltaTime / aracnoid.unloadTime;
        if (transform.position != unloadingPosition)
        {
            transform.position = Vector3.Lerp(lastPositionBeforeOpening, unloadingPosition, openingFracJourney);
        }

    }
    public void CloseWeakSpot()
    {
        if (state != LaserState.closing)
        {
            state = LaserState.closing;
            openingFracJourney = 0;
            lastPositionBeforeClosing = transform.position;
        }
        openingFracJourney += Time.fixedDeltaTime / aracnoid.reloadingTime;
        if (transform.position != initialPosition)
        {
            transform.position = Vector3.Lerp(lastPositionBeforeClosing, initialPosition, openingFracJourney);
        }
    }
    public bool isDoneWaiting()
    {
        if (Time.time >= waitingTime + resetTime + initialOffset)
        {
            StartLoading();
            return true;
        }
        return false;
    }
    public bool isDoneLoading()
    {
        if (Time.time >= loadingTime + resetTime + initialOffset)
        {
            StartShooting();
            return true;
        }
        return false;
    }
    public bool isDoneShooting()
    {
        if (Time.time >= shootingTime + resetTime + initialOffset)
        {
            StartWaiting();
            return true;
        }
        return false;
    }

    public void StartLoading()
    {
        laserEmitterMaterial.color = Color.red;
        state = LaserState.loading;
        laserObject.SetActive(true);
        loadingParticle.Play();
    }

    public void StartShooting()
    {
        laserEmitterMaterial.color = Color.yellow;
        laserCollider.enabled = true;
        state = LaserState.shooting;
        loadingParticle.Stop();
        laserParticle.Play();
        rayParticle.Play();
    }

    public void StartWaiting()
    {
        laserObject.SetActive(false);
        laserCollider.enabled = false;
        state = LaserState.waiting;
        resetTime = resetTime + shootingTime + initialOffset;
        initialOffset = 0;
        laserParticle.Stop();
        rayParticle.Stop();
        SetLaserParticleTimes();
    }

    void SetLaserParticleTimes()
    {
        //loadingParticleMain.simulationSpeed = loadingTime / baseLoadingParticleTime;
        laserParticleMain.loop = false;
        rayParticleMain.loop = false;
        loadingParticleMain.duration = loadingTime - waitingTime;
        laserParticleMain.duration = shootingTime - loadingTime;
        rayParticleMain.startLifetime = shootingTime - loadingTime;
        //laserParticleMain.startDelay = loadingTime - waitingTime;
        //rayParticleMain.startDelay = loadingTime - waitingTime;
    }

    void SetLaserParticleWoundedTimes()
    {
        laserParticleMain.loop = true;
        rayParticleMain.loop = true;
        rayParticleMain.startLifetime = aracnoid.maxWoundedTime;
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
        StartLoading();
        StartCoroutine(ForceEnableLaser());
    }
    IEnumerator ForceEnableLaser()
    {
        yield return new WaitForSeconds(loadingTime - waitingTime);
        StartShooting();
        SetLaserParticleWoundedTimes();
    }
    public  void ForceWaiting()
    {
        StartWaiting();
    }
    void FixedUpdate()
    {
        if (aracnoid.state == AracnoidEnemy.AracnoidState.shooting)
        {
            Fluctuate();
        }
        else if (aracnoid.state == AracnoidEnemy.AracnoidState.unloading)
        {
            OpenWeakSpot();
        }
        else if (aracnoid.state == AracnoidEnemy.AracnoidState.reloading)
        {
            CloseWeakSpot();
        }
    }
}

