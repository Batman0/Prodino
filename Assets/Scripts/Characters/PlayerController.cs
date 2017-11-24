using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState { CanMove, CanShoot, CanMoveAndShoot, CantMoveOrShoot }

	[Header("General")]
	[HideInInspector]
	public PlayerState currentPlayerState;
	[HideInInspector]
	public Vector3 startPosition;
	[SerializeField]
	private GameObject playerModel;
	private PropertiesPlayer properties;
	private int enemyLayer = 12;
	private float playerBackwardsAnimationLimit = 25;
	private RaycastHit hit;
	private Rigidbody rb;
	public LayerMask groundMask;
	[SerializeField]
	private Transform landmark;
	public Collider sideBodyCollider;
	public Collider topBodyCollider;
	public Collider topTailCollider;
	private int life;
    private bool isInvincible;
    private float invincibleTime;

    [Header("Input")]
    private Player player;
    private const int playerId = 0;

	[Header("Movement")]
    public bool canJump = true;
    public bool thereIsGround;
    private float speed;
    private float jumpForce;
    private float upRotationAngle;
    private float downRotationAngle;
    public float jumpCheckRayLength;
    public float groundCheckRayLength;
    private float controllerDeadZone = 0.1f;
	private float gravity;
	private float glideSpeed;
	private float topdownPlayerHeight;
	private float angle;
	private float horizontal;
	private float horizontalAxis;
	private float verticalAxis;
	private float minimumX;
	private float maximumX;
    private Quaternion sideScrollRotation;
    private Quaternion armsAimStartRotation;
    private Quaternion sideBodyColliderStartRot;
    private Quaternion topBodyColliderStartRot;
    private Quaternion topTailBodyColliderStartRot;

    [Header("Shooting")]
    private float fireRatio;
    private float fireTimer;
	public Transform[] bulletSpawnPoints;
    private float RespawnTimer;
    private const string playerBulletTag = "PlayerBullet";
    
    [Header("BulletPool")]
    private PoolManager.PoolBullet bulletPool;

    [Header("Guns")]
    private Quaternion shoulderRStartRotation;
    private Quaternion shoulderLStartRotation;
    public GameObject armRx;
    public Transform gunRx;
    public GameObject armLx;
    public Transform gunLx;
    private float maxArmsRotation;
    private float angleS = 0;
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

    [Header("Boundaries")]
    public float sideXMin;
    public float sideXMax;
    public float sideYMin = 5.5f;
    public float sideYMax;
    public float topXMin, topXMax, topZMin, topZMax;

    [Header("Animations")]
    private bool isSidescroll;
    private bool anim_isRunning;
    private bool anim_isFlying;
    private bool anim_isMovingBackwards;
    private bool anim_isGliding;
    private bool anim_isJumping;
    private Vector3 inverseDirection;
    private Vector3 playerForward;
    private float anglePlayerDirection;
    public Animator animator;

    [Header("Particles")]
    public ParticleSystemManager jetpackStrongerParticle;
    public ParticleSystemManager jetpackRegularParticle;

    [Header("TailMelee")]
    private float tailMeleeSpeed;

	[Header("BiteMelee")]
    private float biteATKSpeed;
    private bool biteCoolDownActive;
    private float biteCoolDown;

    void Awake()
    {
        Register.instance.player = this;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        Main();
    }

    void LateUpdate()
    {
        sideBodyCollider.transform.rotation = sideBodyColliderStartRot;
        topBodyCollider.transform.rotation = topBodyColliderStartRot;
    }

    void OnTriggerEnter(Collider other)
    {
		if (!topTailCollider.enabled && !biteCoolDownActive)
        {
            if (!isInvincible && other.gameObject.layer == enemyLayer)
            {
                //Debug.Log("CCCCCCCCC");
                life--;

                if (IsDead())
                {
                    life = properties.lives;
                    playerModel.SetActive(false);
                    StartCoroutine("EnablePlayer");
                }
                
                if (other.transform.tag.StartsWith("EnemyBullet"))
                {
                    other.transform.gameObject.SetActive(false);
                }
            }
        }
        else if (topTailCollider.enabled)
        {
            if (other.gameObject.layer == enemyLayer && !IsDead())
            {
                other.transform.gameObject.SetActive(false);
            }
        }

		if (biteCoolDownActive) {
			if (other.gameObject.layer == enemyLayer && !IsDead())
			{
                other.transform.gameObject.SetActive(false);
            }
		}
    }

    void Init()
    {
        //It initializes the Rewired's player.
        player = ReInput.players.GetPlayer(playerId);
        minimumX = -70f;
        maximumX = 70;
        bulletPool = PoolManager.instance.pooledBulletClass["PlayerBullet"];
        properties = Register.instance.propertiesPlayer;
        speed = properties.xSpeed;
        jumpForce = properties.jumpForce;
        glideSpeed = properties.glideSpeed;
        upRotationAngle = properties.upRotationAngle;
        downRotationAngle = properties.downRotationAngle;
        fireRatio = properties.fireRatio;
        RespawnTimer = properties.respawnTimer;
        gravity = properties.gravity;
        maxArmsRotation = properties.maxArmsRotation;
        tailMeleeSpeed = properties.tailMeleeSpeed;
        biteATKSpeed = properties.biteATKSpeed;
        biteCoolDownActive = properties.biteCoolDownActive;
        biteCoolDown = properties.biteCoolDown;
        topdownPlayerHeight = properties.topdownPlayerHeight;
        invincibleTime = properties.invincibleTime;
        sideBodyColliderStartRot = sideBodyCollider.transform.rotation;
        topBodyColliderStartRot = topBodyCollider.transform.rotation;
        sideScrollRotation = transform.rotation;
        armsAimStartRotation = armsAim.transform.rotation;
        life = properties.lives;
        transform.position = new Vector3(transform.position.x, landmark.position.y, transform.position.z);
        startPosition = transform.position;
        aimTransform = Instantiate(aimTransformPrefab, Vector3.zero, aimTransformPrefab.transform.rotation) as GameObject;
        currentPlayerState = PlayerState.CanMoveAndShoot;
        gunIndex = 0;
        shoulderLStartRotation = shoulderAimL.transform.rotation;
        shoulderRStartRotation = shoulderAimR.transform.rotation;
    }

    void Main()
    {
        if (!IsDead())
        {
            if (!GameManager.instance.transitionIsRunning)
            {
                UpdateMovementAxises();
                UpdateGroundBooleans();
                Aim();

                switch (GameManager.instance.currentGameMode)
                {
                    case GameMode.SIDESCROLL:
                        MainSidescroll();
                        break;
                    case GameMode.TOPDOWN:
                        MainTopdown();
                        break;
                }

                if (fireTimer < fireRatio)
                {
                    fireTimer += Time.deltaTime;
                }
                else if (player.GetButton("Shoot") && (currentPlayerState == PlayerState.CanMoveAndShoot || currentPlayerState == PlayerState.CanShoot))
                {
                    Shoot();
                }

            }
            else
            {
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && armsAim.transform.rotation != armsAimStartRotation)
                {
                    armsAim.transform.rotation = armsAimStartRotation;
                }
                ChangePerspective();
            }
        }
    }

    void MainSidescroll()
    {
        if (!isSidescroll)
        {
            SetPlayerToSidescroll();
        }
        //if (transform.rotation != sideScrollRotation)
        //{
        //    transform.rotation = sideScrollRotation;
        //}
        inverseDirection = new Vector3(horizontalAxis, verticalAxis, 0);

        if (currentPlayerState == PlayerState.CanMoveAndShoot || currentPlayerState == PlayerState.CanMove)
        {
			if ((transform.position.x > sideXMin && horizontalAxis < -controllerDeadZone) || (transform.position.x < sideXMax && horizontalAxis > controllerDeadZone))
            {
                Move(Vector3.right, speed, "MoveHorizontal");
            }
            if (player.GetButtonDown("Jump") && canJump)
            {
                Jump();
            }
        }
        if (thereIsGround && !canJump)
        {
            if (!anim_isJumping)
            {
                SetAnimationFromRunToJump();
            }
            ApplyGravity();
        }
        else if (thereIsGround && canJump)
        {
            if (rb.velocity.y < 0)
            {
                if (!anim_isRunning)
                {
                    SetAnimationToRun();
                }
                ResetPlayerAfterJump();
            }
        }
        if (player.GetButton("Jump"))
        {
            if (!canJump && rb.velocity.y < -0.5f)
            {
                if (!anim_isGliding)
                {
                    SetAnimationFromJumpToGlide();
                }

                if (rb.velocity.y < -2)
                {
                    StabilizeAcceleration();
                }
                else
                {
                    Glide();
                }
            }
        }

        //if (transform.rotation != sideScrollRotation)
        //{
        //    transform.rotation = sideScrollRotation;
        //}

        ClampPositionSidescroll();

        if (currentPlayerState == PlayerState.CanMoveAndShoot || currentPlayerState == PlayerState.CanShoot)
        {
            UpdateArmsRotation();
        }

        if ((currentPlayerState == PlayerState.CanMoveAndShoot || currentPlayerState == PlayerState.CanMove) && player.GetButtonDown("Meele") && !biteCoolDownActive && canJump)
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
        //if (rb.velocity != Vector3.zero)
        //{

        //}e
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
        if (currentPlayerState == PlayerState.CanMoveAndShoot || currentPlayerState == PlayerState.CanMove)
        {
            Move(Vector3.forward, speed, "MoveVertical");
            Move(Vector3.right, speed, "MoveHorizontal");
        }
        if (currentPlayerState == PlayerState.CanMoveAndShoot || currentPlayerState == PlayerState.CanShoot)
        {
            TurnAroundGO(transform);
            gunsAimL.transform.position = new Vector3(gunsAimL.transform.position.x, aimTransform.transform.position.y, shoulderAimL.transform.position.z);
            gunsAimR.transform.position = new Vector3(gunsAimR.transform.position.x, aimTransform.transform.position.y, shoulderAimR.transform.position.z);
        }

        ClampPositionTopdown();

        if ((currentPlayerState == PlayerState.CanMoveAndShoot || currentPlayerState == PlayerState.CanMove) && player.GetButtonDown("Meele"))
        {
            StartCoroutine("TailAttack");
        }
    }

    void UpdateMovementAxises()
    {
        horizontalAxis = player.GetAxis("MoveHorizontal");
        verticalAxis = player.GetAxis("MoveVertical");
    }

    void UpdateGroundBooleans()
    {
        canJump = CheckGround(jumpCheckRayLength);
        thereIsGround = CheckGround(groundCheckRayLength);
    }

    void UpdateArmsRotation()
    {
        Vector3 aim = aimTransform.transform.position - armsAim.transform.position;
        float aimAngle = Vector3.Angle(Vector3.right, aim);
        Vector3 cross = Vector3.Cross(Vector3.right, aim);

        if (aimAngle <= maxArmsRotation && cross.z >= 0)
        {
            TurnAroundGO(shoulderAimL.transform);
            TurnAroundGO(shoulderAimR.transform);
        }
        else if (aimAngle <= maxArmsRotation && cross.z < 0)
        {
            TurnAroundGO(shoulderAimL.transform);
            TurnAroundGO(shoulderAimR.transform);
        }

        if (aimAngle <= upRotationAngle && cross.z >= 0)
        {

            gunsAimL.transform.rotation = Quaternion.Euler(new Vector3(-aimAngle, 90f, 0));
            gunsAimR.transform.rotation = Quaternion.Euler(new Vector3(-aimAngle, 90f, 0));
        }
        else if (aimAngle <= downRotationAngle && cross.z < 0)
        {

            gunsAimL.transform.rotation = Quaternion.Euler(new Vector3(aimAngle, 90f, 0));
            gunsAimR.transform.rotation = Quaternion.Euler(new Vector3(aimAngle, 90f, 0));
        }
    }

    void SetPlayerToSidescroll()
    {
        isSidescroll = true;
        animator.SetBool("sidescroll", isSidescroll);
        transform.rotation = sideScrollRotation;
    }

    void SetPlayerToTopdown()
    {
        isSidescroll = false;
        anim_isFlying = true;
        animator.SetBool("sidescroll", isSidescroll);
        animator.SetBool("isFlying", anim_isFlying);
        rb.velocity = Vector3.zero;
    }

    void SetAnimationFromRunToJump()
    {
        anim_isRunning = false;
        anim_isJumping = true;
        animator.SetBool("isRunning", anim_isRunning);
        animator.SetBool("isJumping", anim_isJumping);
    }

    void SetAnimationToRun()
    {
        anim_isJumping = false;
        anim_isGliding = false;
        anim_isRunning = true;
        animator.SetBool("isJumping", anim_isJumping);
        animator.SetBool("isGliding", anim_isGliding);
        animator.SetBool("isRunning", anim_isRunning);
    }

    void SetAnimationFromJumpToGlide()
    {
        anim_isJumping = false;
        anim_isGliding = true;
        animator.SetBool("isJumping", anim_isJumping);
        animator.SetBool("isGliding", anim_isGliding);
    }

    void SetAnimationFromFlyToMoveBackwards()
    {
        anim_isFlying = false;
        anim_isMovingBackwards = true;
        animator.SetBool("isFlying", anim_isFlying);
        animator.SetBool("isMovingBackwards", anim_isMovingBackwards);
    }

    void SetAnimationFromMoveBackwardsToFly()
    {
        anim_isMovingBackwards = false;
        anim_isFlying = true;
        animator.SetBool("isMovingBackwards", anim_isMovingBackwards);
        animator.SetBool("isFlying", anim_isFlying);
    }

    void ResetPlayerAfterJump()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, landmark.position.y, transform.position.z);
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    void StabilizeAcceleration()
    {
        rb.AddForce(Vector3.up * Mathf.Abs((rb.velocity.y / 3)), ForceMode.Impulse);
    }

    void Jump()
    {
        rb.velocity = new Vector3(0, jumpForce, 0);
    }

    void Glide()
    {
        rb.AddForce(Vector3.up * glideSpeed, ForceMode.Force);
    }

    bool CheckGround(float rayLength)
    {
        Ray ray = new Ray(new Vector3 (transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        if (Physics.Raycast(ray, rayLength, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
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
                //aimTransform.transform.position = aimVector;
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

    void TurnAroundGO(Transform transform)
    {
        switch (GameManager.instance.currentGameMode)
        {
            case GameMode.SIDESCROLL:
                transform.LookAt(new Vector3(aimTransform.transform.position.x, aimTransform.transform.position.y, transform.position.z));
                break;
            case GameMode.TOPDOWN:
                transform.LookAt(new Vector3(aimTransform.transform.position.x, transform.position.y, aimTransform.transform.position.z));
                break;
        }
    }

    void Shoot()
    {
        GameObject bullet = bulletPool.GetpooledBullet();
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
//        transform.position = new Vector3(
//        Mathf.Clamp(transform.position.x, Register.instance.xMin + sideXMin, Register.instance.xMax - sideXMax),
//        Mathf.Clamp(transform.position.y, Register.instance.yMin + sideYMin, Register.instance.yMax - sideYMax),
//        0.0f);

		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, Register.instance.xMin , Register.instance.xMax ),
			Mathf.Clamp(transform.position.y, Register.instance.yMin , Register.instance.yMax ),
			Mathf.Clamp(transform.position.z, Register.instance.zMin , Register.instance.zMax)
		);


//		transform.position = new Vector3(
//			Mathf.Clamp(transform.position.x, sideXMin,  sideXMax),
//			Mathf.Clamp(transform.position.y,  sideYMin,  sideYMax),
//			0.0f);
    }

    public void ClampPositionTopdown()
    {
//        if (Register.instance.zMin.HasValue && Register.instance.zMax.HasValue)
//        transform.position = new Vector3(
//        Mathf.Clamp(transform.position.x, Register.instance.xMin + topXMin, Register.instance.xMax - topXMax),
//        landmark.position.y + topdownPlayerHeight,
//        Mathf.Clamp(transform.position.z, Register.instance.zMin.Value + topZMin, Register.instance.zMax.Value - topZMax)
//        );


			transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, Register.instance.xMin , Register.instance.xMax),
				landmark.position.y + topdownPlayerHeight,
			Mathf.Clamp(transform.position.z, Register.instance.zMin , Register.instance.zMax)
			);
    }

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
		angleX = Mathf.Clamp (angleX, minimumX, maximumX);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

    void ChangePerspective()
    {
        if (GameManager.instance.transitionIsRunning)
        {
            shoulderAimL.transform.rotation = shoulderLStartRotation;
            shoulderAimR.transform.rotation = shoulderRStartRotation;

            if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
            {
                if (!sideBodyCollider.enabled)
                {
                    topBodyCollider.enabled = false;
                    sideBodyCollider.enabled = true;
                }
            }
            else
            {
                if (!topBodyCollider.enabled)
                {
                    sideBodyCollider.enabled = false;
                    topBodyCollider.enabled = true;
                }
            }
        }
    }

	public bool IsDead()
	{
		return life <= 0;
	}

    IEnumerator TailAttack()
    {
        currentPlayerState = PlayerState.CantMoveOrShoot;
        angle = 0;
        topTailCollider.enabled = true;
      
        while (angle < 360)
        {
            angle += tailMeleeSpeed;
            transform.Rotate(Vector3.up, tailMeleeSpeed, Space.World);

            yield return null;
        }
        topTailCollider.enabled = false;
        currentPlayerState = PlayerState.CanMoveAndShoot;
    }

	IEnumerator BiteAttack()
	{
        currentPlayerState = PlayerState.CantMoveOrShoot;
        rb.velocity = new Vector3(0, biteATKSpeed, 0);
		biteCoolDownActive = true;

        yield return new WaitForSeconds (biteCoolDown);
        biteCoolDownActive = false;
        currentPlayerState = PlayerState.CanMoveAndShoot;
	}

    IEnumerator EnablePlayer()
    {
        currentPlayerState = PlayerState.CantMoveOrShoot;
        yield return new WaitForSeconds(RespawnTimer);
        currentPlayerState = PlayerState.CanMoveAndShoot;
        playerModel.SetActive(true);
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        rb.velocity = Vector3.zero;
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

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
}
