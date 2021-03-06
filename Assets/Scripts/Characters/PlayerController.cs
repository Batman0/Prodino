﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState { Moving, Attacking, Dead }

    private PlayerState currentPlayerState;
    public PlayerState CurrentPlayerState
    {
        get { return currentPlayerState; }
    }
    [HideInInspector]
    //LUCA usata per settare la posizione di partenza e anche per farlo rimanere su una y costante. 
    //da cambiare se da design ci fosse bisogno di far cadere o spostare in basso trevor.
    public Vector3 startPosition;
    //[SerializeField]
    //private PropertiesPlayer properties;

    private RaycastHit hit;
    private Rigidbody rb;
    public LayerMask groundMask;
    public Collider sideBodyCollider;
    public Collider topBodyCollider;
    public Collider topTailCollider;
    [SerializeField]
    private int lives;
    private int currentLives;

    [Header("Respawn")]
    public GameObject playerModel;
    private bool isInvincible;
    [SerializeField]
    public float invincibleTime;
    [SerializeField]
    public float RespawnTimer;

    [Header("Input")]
    private Player player;
    private const int playerId = 0;

    [Header("Movement")]
    private bool canJump = true;
    private bool jumpFrame = false;
    private bool thereIsGround;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    private float jumpTimer;
    public float timeToJump = 0.2f;
    [SerializeField]
    private float upRotationAngle;
    [SerializeField]
    private float downRotationAngle;
    private const float jumpCheckRayLength = 3.0f;
    private const float groundCheckRayLength = 25.0f;
    private float controllerDeadZone = 0.1f;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float drag;
    [SerializeField]
    private float glideSpeed;
    [SerializeField]
    private float topdownPlayerHeight;
    private float horizontalAxis;
    private float verticalAxis;
    private Quaternion transformStartRotation;
    private Quaternion armsAimStartRotation;
    private Quaternion sideBodyColliderStartRot;
    private Quaternion topBodyColliderStartRot;
    private Quaternion topTailBodyColliderStartRot;

    [Header("Shooting")]
    [SerializeField]
    private float fireRatio;
    private float fireTimer;
    public Transform[] bulletSpawnPoints;
    private const string playerBulletTag = "PlayerBullet";
    public GameObject bulletPrefab;

    [Header("BulletPool")]
    private PoolManager.PoolBullet bulletPool;

    [Header("Guns")]
    public GameObject armRx;
    public Transform gunRx;
    public GameObject armLx;
    public Transform gunLx;
    [SerializeField]
    private float maxArmsRotation;
    private int gunIndex;

    [Header("Aim")]
    private float intersectionPoint;
    private Vector3 aimVector;
    private Plane? sidescrollPlane;
    private Plane? topDownPlane;
    private Ray aimRay;
    public GameObject aimTransformPrefab;
    private GameObject aimTransform;
    public GameObject armsAim;
    public GameObject gunsAimR;
    public GameObject gunsAimL;
    public GameObject shoulderAimR;
    public GameObject shoulderAimL;
    private Quaternion gunLStartRotation;
    private Quaternion gunRStartRotation;
    private Quaternion shoulderRStartRotation;
    private Quaternion shoulderLStartRotation;

    [Header("Boundaries")]
    private float sideXMin, sideXMax, sideYMin, sideYMax;
    private float topXMin, topXMax, topZMin, topZMax;

    [Header("Animations")]
    public bool isSidescroll;
    public bool anim_isRunning;
    public bool anim_isFlying;
    public bool anim_isMovingBackwards;
    private bool anim_isGliding;
    private bool anim_isJumping;
    private Vector3 inverseDirection;
    private Vector3 playerForward;
    private float anglePlayerDirection;
    public Animator animator;
    private float playerBackwardsAnimationLimit = 25;
    private const string sidescrollAnimation = "sidescroll";
    private const string isFlyingAnim = "isFlying";
    private const string isRunningAnim = "isRunning";
    private const string isJumpingAnim = "isJumping";
    private const string isGlidingAnim = "isGliding";
    private const string movingBackwardsAnim = "isMovingBackwards";

    [Header("Particles")]
    public ParticleSystemManager jetpackStrongerParticle;
    public ParticleSystemManager jetpackRegularParticle;

    [Header("TailMelee")]
    [SerializeField]
    private float tailMeleeSpeed;
    private float angleTailAttack;

    [Header("BiteMelee")]
    [SerializeField]
    private float biteHeight;
    [SerializeField]
    private float biteCoolDown;

    void Awake()
    {
        Register.instance.player = this;
        rb = GetComponent<Rigidbody>();
        Init();
        jumpTimer = timeToJump;
    }

    //Mettere il sistema ad evento 
    //void OnEnable()
    //{
    //    EventManager.changeState += ChangeGameMode;
    //}

    void FixedUpdate()
    {
        Main();
    }

    void Update()
    {
        if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            SetArmsToStartRotation();
            CheckJump();
        }

        if (currentPlayerState != PlayerState.Dead)
        {
            if (GameManager.instance.transitionIsRunning)
            {
                StartCoroutine("ChangeGameMode");
            }
            else
            {
                if (currentPlayerState != PlayerState.Attacking)
                {
                    UpdateMovementAxes();
                    UpdateGroundBooleans();
                    Aim();
                    TryShooting();
                }
            }
        }
    }

    void LateUpdate()
    {
        sideBodyCollider.transform.rotation = sideBodyColliderStartRot;
        topBodyCollider.transform.rotation = topBodyColliderStartRot;
    }

    private void CheckJump()
    {
        if (jumpTimer < timeToJump)
        {
            jumpTimer += Time.deltaTime;
        }
        //GURRA ho rimesso il button down, poiché se il player sta planando c'è il rischio che appena tocchi terra salti subito involontariamente
        if (player.GetButtonDown("Jump") && jumpTimer >= timeToJump && canJump)
        {
            jumpFrame = true;
        }
        else
        {
            jumpFrame = false;
        }
    }
    private void SetArmsToStartRotation()
    {
        if (armsAim.transform.rotation != armsAimStartRotation)
        {
            armsAim.transform.rotation = armsAimStartRotation;

        }
    }
    void Init()
    {
        PlayerInit();
        RewiredInit();
        ShootInit();
        ArmsAimInit();
        Boundaries();
    }

    void Main()
    {
        if (currentPlayerState != PlayerState.Dead)
        {
            if (!GameManager.instance.transitionIsRunning)
            {
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {
                    MainSidescroll();
                }
                else
                {
                    MainTopdown();
                }
            }
        }
    }

    void MainSidescroll()
    {

        if (!isSidescroll)
        {
            SetPlayerToSidescroll();
        }
        inverseDirection = new Vector3(horizontalAxis, verticalAxis, 0);

        if (currentPlayerState == PlayerState.Moving)
        {
            if ((transform.position.x > sideXMin && horizontalAxis < -controllerDeadZone) || (transform.position.x < sideXMax && horizontalAxis > controllerDeadZone))
            {
                Move(Vector3.right, speed, "MoveHorizontal");
            }
            if (jumpFrame)
            {
                Jump();
            }
        }
        if (thereIsGround)
        {
            if (!canJump)
            {
                if (!anim_isJumping)
                {
                    SetAnimationFromRunToJump();
                }
                ApplyGravity();
            }
            else
            {
                if (rb.velocity.y <= 0)
                {
                    if (!anim_isRunning)
                    {
                        SetAnimationToRun();
                    }

                    ResetPlayerAfterJump();
                }
            }
        }


        if (player.GetButton("Jump"))
        {
            if (!canJump && rb.velocity.y < 0)
            {
                if (!anim_isGliding)
                {
                    SetAnimationFromJumpToGlide();
                }
                if (rb.velocity.y < -drag)
                {
                    StabilizeAcceleration();
                }
                else
                {
                    Glide();
                }
            }
        }

        if (transform.rotation != transformStartRotation)
        {
            transform.rotation = transformStartRotation;
        }

        ClampPositionSidescroll();
        UpdateArmsRotation();
        if (currentPlayerState == PlayerState.Moving && player.GetButtonDown("Meele") && canJump)
        {
            StartCoroutine("BiteAttack");
        }
    }

    void MainTopdown()
    {
        if (isSidescroll)
        {
            SetPlayerToTopdown();
        }

        if (currentPlayerState == PlayerState.Moving)
        {
            Move(Vector3.forward, speed, "MoveVertical");
            Move(Vector3.right, speed, "MoveHorizontal");

            transform.LookAt(new Vector3(aimTransform.transform.position.x, transform.position.y, aimTransform.transform.position.z));

            inverseDirection = new Vector3(-horizontalAxis, 0, -verticalAxis);
            playerForward = new Vector3(transform.forward.x, 0, transform.forward.z);
            anglePlayerDirection = Vector3.Angle(inverseDirection, playerForward);

            if (anglePlayerDirection <= playerBackwardsAnimationLimit)
            {
                if (!anim_isMovingBackwards)
                {
                    SetAnimationFromFlyToMoveBackwards();
                }
            }

            if (anglePlayerDirection > playerBackwardsAnimationLimit)
            {
                if (!anim_isFlying)
                {
                    SetAnimationFromMoveBackwardsToFly();
                }
            }
        }

        ClampPositionTopdown();

        if ((currentPlayerState == PlayerState.Moving) && player.GetButtonDown("Meele"))
        {
            StartCoroutine("TailAttack");
        }
    }

    void TryShooting()
    {
        if (fireTimer < fireRatio)
        {
            fireTimer += Time.deltaTime;
        }
        else if (player.GetButton("Shoot") && currentPlayerState == PlayerState.Moving)
        {
            Shoot();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!topTailCollider.enabled && currentPlayerState != PlayerState.Attacking)
        {
            if (!isInvincible && other.gameObject.layer == Register.instance.EnemyLayer)
            {
                currentLives--;
                if (IsDead())
                {
                    KillPlayer();
                }
            }
        }
    }

    void UpdateMovementAxes()
    {
        horizontalAxis = player.GetAxis("MoveHorizontal");
        verticalAxis = player.GetAxis("MoveVertical");
    }

    void UpdateGroundBooleans()
    {
        canJump = CheckGround(jumpCheckRayLength);
        thereIsGround = CheckGround(groundCheckRayLength);

        if (thereIsGround == false)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Force);

            if (!canJump && rb.velocity.y < 0)
            {
                if (!anim_isGliding)
                {
                    SetAnimationFromJumpToGlide();
                }
                if (rb.velocity.y < -drag)
                {
                    StabilizeAcceleration();
                }
                else
                {
                    Glide();
                }

                if (IsDead())
                {
                    KillPlayer();
                }
            }

        }
    }

    void UpdateArmsRotation()
    {
        Vector3 aim = aimTransform.transform.position - armsAim.transform.position;
        float aimAngle = Vector3.Angle(Vector3.right, aim);
        Vector3 cross = Vector3.Cross(Vector3.right, aim);

        if (aimAngle <= maxArmsRotation && cross.z >= 0)
        {
            shoulderAimL.transform.rotation = Quaternion.Euler(new Vector3(-aimAngle, 90f, 0));
            shoulderAimR.transform.rotation = Quaternion.Euler(new Vector3(-aimAngle, 90f, 0));
        }
        else if (aimAngle <= maxArmsRotation && cross.z < 0)
        {
            shoulderAimL.transform.rotation = Quaternion.Euler(new Vector3(aimAngle, 90f, 0));
            shoulderAimR.transform.rotation = Quaternion.Euler(new Vector3(aimAngle, 90f, 0));
        }

        if (aimAngle < upRotationAngle && cross.z >= 0)
        {

            gunsAimL.transform.rotation = Quaternion.Euler(new Vector3(-aimAngle, 90f, 0));
            gunsAimR.transform.rotation = Quaternion.Euler(new Vector3(-aimAngle, 90f, 0));
        }
        else if (aimAngle < downRotationAngle && cross.z < 0)
        {
            gunsAimL.transform.rotation = Quaternion.Euler(new Vector3(aimAngle, 90f, 0));
            gunsAimR.transform.rotation = Quaternion.Euler(new Vector3(aimAngle, 90f, 0));
        }

        if (aimAngle >= 90 && cross.z >= 0)
        {
            shoulderAimL.transform.rotation = Quaternion.Euler(new Vector3(-maxArmsRotation, 90f, 0));
            shoulderAimR.transform.rotation = Quaternion.Euler(new Vector3(-maxArmsRotation, 90f, 0));
            gunsAimL.transform.rotation = Quaternion.Euler(new Vector3(-upRotationAngle, 90f, 0));
            gunsAimR.transform.rotation = Quaternion.Euler(new Vector3(-upRotationAngle, 90f, 0));
        }
        else if (aimAngle >= 90 && cross.z < 0)
        {
            shoulderAimL.transform.rotation = Quaternion.Euler(new Vector3(maxArmsRotation, 90f, 0));
            shoulderAimR.transform.rotation = Quaternion.Euler(new Vector3(maxArmsRotation, 90f, 0));
            gunsAimL.transform.rotation = Quaternion.Euler(new Vector3(downRotationAngle, 90f, 0));
            gunsAimR.transform.rotation = Quaternion.Euler(new Vector3(downRotationAngle, 90f, 0));
        }
    }

    void SetPlayerToSidescroll()
    {
        isSidescroll = true;
        animator.SetBool(sidescrollAnimation, isSidescroll);
        transform.rotation = transformStartRotation;
    }

    void SetPlayerToTopdown()
    {
        isSidescroll = false;
        anim_isFlying = true;
        animator.SetBool(sidescrollAnimation, isSidescroll);
        animator.SetBool("isFlying", anim_isFlying);
        rb.velocity = Vector3.zero;
    }

    void SetAnimationFromRunToJump()
    {
        anim_isRunning = false;
        anim_isJumping = true;
        animator.SetBool(isRunningAnim, anim_isRunning);
        animator.SetBool(isJumpingAnim, anim_isJumping);
    }

    void SetAnimationToRun()
    {
        anim_isJumping = false;
        anim_isGliding = false;
        anim_isRunning = true;
        animator.SetBool(isJumpingAnim, anim_isJumping);
        animator.SetBool(isGlidingAnim, anim_isGliding);
        animator.SetBool(isRunningAnim, anim_isRunning);
    }

    void SetAnimationFromJumpToGlide()
    {
        anim_isJumping = false;
        anim_isGliding = true;
        animator.SetBool(isJumpingAnim, anim_isJumping);
        animator.SetBool(isGlidingAnim, anim_isGliding);
    }

    void SetAnimationFromFlyToMoveBackwards()
    {
        anim_isFlying = false;
        anim_isMovingBackwards = true;
        animator.SetBool(isFlyingAnim, anim_isFlying);
        animator.SetBool(movingBackwardsAnim, anim_isMovingBackwards);
    }

    void SetAnimationFromMoveBackwardsToFly()
    {
        anim_isFlying = true;
        anim_isMovingBackwards = false;
        animator.SetBool(movingBackwardsAnim, anim_isMovingBackwards);
        animator.SetBool(isFlyingAnim, anim_isFlying);
    }

    void ResetPlayerAfterJump()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Force);
    }

    void StabilizeAcceleration()
    {
        rb.AddForce(Vector3.up * Mathf.Abs((rb.velocity.y / gravity * drag)), ForceMode.Impulse);
    }

    void Jump()
    {
        jumpTimer = 0;
        rb.velocity = new Vector3(0, jumpForce, 0);
    }

    void Glide()
    {
        rb.AddForce(Vector3.up * glideSpeed, ForceMode.Force);
    }

    bool CheckGround(float rayLength)
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        return Physics.Raycast(ray, rayLength, groundMask);
    }

    void Move(Vector3 moveVector, float speed, string moveAxisName)
    {
        float moveAxis = player.GetAxis(moveAxisName);
        if (moveAxis < -controllerDeadZone || moveAxis > controllerDeadZone)
        {
            transform.Translate(moveVector * moveAxis * speed * Time.fixedDeltaTime, Space.World);
        }
    }

    void Aim()
    {
        if (sidescrollPlane == null && GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            sidescrollPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);
        }
        if (topDownPlane == null && GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            topDownPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);
        }

        if (topDownPlane != null && GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            aimRay = Camera.main.ScreenPointToRay(player.controllers.Mouse.screenPosition);
            if (topDownPlane.Value.Raycast(aimRay, out intersectionPoint))
            {
                aimVector = aimRay.GetPoint(intersectionPoint);
                aimTransform.transform.position = new Vector3(aimVector.x, transform.position.y, aimVector.z);
            }
        }

        if (sidescrollPlane != null && GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            aimRay = Camera.main.ScreenPointToRay(player.controllers.Mouse.screenPosition);
            if (sidescrollPlane.Value.Raycast(aimRay, out intersectionPoint))
            {
                aimVector = aimRay.GetPoint(intersectionPoint);
                aimTransform.transform.position = aimVector;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = PoolManager.instance.pooledBulletClass["PlayerBullet"].GetpooledBullet();
        bullet.transform.position = bulletSpawnPoints[gunIndex].position;
        bullet.transform.rotation = bulletSpawnPoints[gunIndex].rotation;
        bullet.SetActive(true);
        gunIndex++;
        if (gunIndex >= bulletSpawnPoints.Length)
        {
            gunIndex = 0;
        }
        fireTimer = 0.00f;
    }

    public void ClampPositionSidescroll()
    {

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, sideXMin, sideXMax),
            Mathf.Clamp(transform.position.y, sideYMin, sideYMax),
            Mathf.Clamp(transform.position.z, topZMin, topZMax)
        );
    }

    public void ClampPositionTopdown()
    {

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, topXMin, topXMax),
            startPosition.y + topdownPlayerHeight,
        Mathf.Clamp(transform.position.z, topZMin, topZMax)
        );
    }

    IEnumerator ChangeGameMode()
    {
        jumpTimer = timeToJump;
        anim_isMovingBackwards = false;
        anim_isFlying = false;
        anim_isRunning = false;
        horizontalAxis = 0;
        verticalAxis = 0;

        ResetLimbsRotation();

        if (transform.rotation != transformStartRotation)
        {
            transform.rotation = transformStartRotation;
        }

        if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            topBodyCollider.enabled = true;
            sideBodyCollider.enabled = false;
        }
        else
        {
            sideBodyCollider.enabled = true;
            topBodyCollider.enabled = false;
        }

        currentPlayerState = PlayerState.Moving;
        yield return null;
    }

    void ResetLimbsRotation()
    {
        shoulderAimL.transform.rotation = shoulderLStartRotation;
        shoulderAimR.transform.rotation = shoulderRStartRotation;
        gunsAimL.transform.rotation = gunLStartRotation;
        gunsAimR.transform.rotation = gunRStartRotation;

        shoulderAimL.transform.rotation = transform.rotation;
        shoulderAimR.transform.rotation = transform.rotation;
        gunsAimL.transform.rotation = transform.rotation;
        gunsAimR.transform.rotation = transform.rotation;
    }

    public bool IsDead()
    {
        if (currentLives <= 0 || ((transform.position.y <= sideYMin + 0.1f) && (transform.position.y >= sideYMin - 0.1f)))
        {
            return true;
        }
        return false;
    }

    public void ResetPlayerLives()
    {
        currentLives = lives;
    }

    private void KillPlayer()
    {
        currentPlayerState = PlayerState.Dead;
        playerModel.SetActive(false);
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator TailAttack()
    {
        currentPlayerState = PlayerState.Attacking;
        angleTailAttack = 0;
        topTailCollider.enabled = true;

        while (angleTailAttack < 360)
        {
            angleTailAttack += tailMeleeSpeed;
            transform.Rotate(Vector3.up, tailMeleeSpeed, Space.World);

            yield return null;
        }
        topTailCollider.enabled = false;
        currentPlayerState = PlayerState.Moving;
    }

    IEnumerator BiteAttack()
    {
        currentPlayerState = PlayerState.Attacking;
        rb.velocity = new Vector3(0, biteHeight, 0);
        currentPlayerState = PlayerState.Attacking;

        yield return new WaitForSeconds(biteCoolDown);
        currentPlayerState = PlayerState.Moving;
    }

    IEnumerator InvinciblePlayer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(RespawnTimer);
        ResetPlayerLives();
        StartCoroutine(InvinciblePlayer());
        currentPlayerState = PlayerState.Moving;
        playerModel.SetActive(true);
        //Position Reset
        transform.position = startPosition;
        rb.velocity = Vector3.zero;

    }

    //JET PARTICLES
    private void ActivateStrongerJetpack()
    {
        jetpackStrongerParticle.PlayAll(true);
        jetpackRegularParticle.StopAll(true);
    }
    private void ActivateRegularJetpack()
    {
        jetpackStrongerParticle.StopAll(true);
        jetpackRegularParticle.PlayAll(true);
    }
    private void DeactivateJetpack()
    {
        jetpackRegularParticle.StopAll(true);
        jetpackStrongerParticle.StopAll(true);
    }

    //INIT METHODS
    void PlayerInit()
    {
        //Player state initialization
        startPosition = transform.position;
        currentPlayerState = PlayerState.Moving;
        //properties = Register.instance.propertiesPlayer;
        anim_isRunning = true;

        //Player Stats
        //topdownPlayerHeight = properties.topdownPlayerHeight;
        //invincibleTime = properties.invincibleTime;
        currentLives = lives;
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
    }

    //Rewired Initialization
    void RewiredInit()
    {
        player = ReInput.players.GetPlayer(playerId);
    }


    void ShootInit()
    {
        //Shooting
        aimTransform = Instantiate(aimTransformPrefab, Vector3.zero, aimTransformPrefab.transform.rotation) as GameObject;
        //fireRatio = properties.fireRatio;
        //RespawnTimer = properties.respawnTimer;
        gunIndex = 0;
    }


    void ArmsAimInit()
    {
        //Changes during camera transition
        shoulderLStartRotation = shoulderAimL.transform.rotation;
        shoulderRStartRotation = shoulderAimR.transform.rotation;
        gunLStartRotation = gunsAimL.transform.rotation;
        gunRStartRotation = gunsAimR.transform.rotation;
        sideBodyColliderStartRot = sideBodyCollider.transform.rotation;
        topBodyColliderStartRot = topBodyCollider.transform.rotation;
        transformStartRotation = transform.rotation;
        armsAimStartRotation = armsAim.transform.rotation;
    }

    void Boundaries()
    {
        //Boundaries From Register
        sideXMin = Register.instance.xMin;
        sideXMax = Register.instance.xMax;
        sideYMin = Register.instance.yMin;
        sideYMax = Register.instance.yMax;
        topXMax = Register.instance.xMax;
        topXMin = Register.instance.xMin;
        topZMax = Register.instance.zMax;
        topZMin = Register.instance.zMin;
    }
}
