using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState { Moving, Attacking, Dead}
    //GENERAL non va tanto bene, organizzare meglio questa sezione
    [Header("General")]
    private PlayerState currentPlayerState;
    [HideInInspector]
    public Vector3 startPosition;
    [SerializeField]
    private GameObject playerModel;
    private PropertiesPlayer properties;
    //GURRA Ricavarsi il layer in un altro modo 
    private int enemyLayer = 12;
    private float playerBackwardsAnimationLimit = 25;
    private RaycastHit hit;
    private Rigidbody rb;
    public LayerMask groundMask;
    //GURRA Questa cosa del landmark va sistemata
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
    //GURRA Fissate i valori e rendetele private le variabili sui raycast
    public float jumpCheckRayLength;
    public float groundCheckRayLength;
    private float controllerDeadZone = 0.1f;
    private float gravity;
    private float glideSpeed;
    private float topdownPlayerHeight;
    private float horizontalAxis;
    private float verticalAxis;
    private Quaternion transformStartRotation;
    private Quaternion armsAimStartRotation;
    private Quaternion sideBodyColliderStartRot;
    private Quaternion topBodyColliderStartRot;
    private Quaternion topTailBodyColliderStartRot;

    [Header("Shooting")]
    private float fireRatio;
    private float fireTimer;
    public Transform[] bulletSpawnPoints;
    //GURRA questa non penso vada in shooting
    private float RespawnTimer;
    private const string playerBulletTag = "PlayerBullet";

    [Header("BulletPool")]
    private PoolManager.PoolBullet bulletPool;

    [Header("Guns")]
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
    private Quaternion gunLStartRotation;
    private Quaternion gunRStartRotation;
    private Quaternion shoulderRStartRotation;
    private Quaternion shoulderLStartRotation;

    [Header("Boundaries")]
    private float sideXMin, sideXMax, sideYMin, sideYMax;
    private float topXMin, topXMax, topZMin, topZMax;

    [Header("Animations")]
    //GURRA mettete anche i nomi delle animazioni come variabili, così non si scrive ogni volta la stringa a mano quando si richiama un'animazione
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

    [Header("Particles")]
    public ParticleSystemManager jetpackStrongerParticle;
    public ParticleSystemManager jetpackRegularParticle;

    [Header("TailMelee")]
    private float tailMeleeSpeed;
    private float angleTailAttack;

    [Header("BiteMelee")]
    //GURRA con questa varibile in realtà si va a modificare l'altezza dell'attacco, non la velocità
    private float biteATKSpeed;
    private float biteCoolDown;

    void Awake()
    {
        Register.instance.player = this;
        rb = GetComponent<Rigidbody>();
        Init();
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        Main();
    }

    void Update()
    {
        if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && armsAim.transform.rotation != armsAimStartRotation)
        {
            armsAim.transform.rotation = armsAimStartRotation;
        }

        if (currentPlayerState != PlayerState.Dead)
        {
            if (GameManager.instance.transitionIsRunning)
            {
                StartCoroutine("ChangePerspective");
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

    void Init()
    {
        //GURRA visto che avete messo questi commenti facendo una divisione sensata, raggruppate in metodi queste sezioni
        //Player state initialization
        startPosition = transform.position;
        currentPlayerState = PlayerState.Moving;
        properties = Register.instance.propertiesPlayer;
        anim_isRunning = true;

        //Rewired Initialization
        player = ReInput.players.GetPlayer(playerId);

        //Arm Rotation in Sidescroll
        upRotationAngle = properties.upRotationAngle;
        downRotationAngle = properties.downRotationAngle;
        maxArmsRotation = properties.maxArmsRotation;

        //Jumping
        speed = properties.xSpeed;
        jumpForce = properties.jumpForce;
        glideSpeed = properties.glideSpeed;

        //Shooting
        aimTransform = Instantiate(aimTransformPrefab, Vector3.zero, aimTransformPrefab.transform.rotation) as GameObject;
        fireRatio = properties.fireRatio;
        RespawnTimer = properties.respawnTimer;
        gravity = properties.gravity;
        gunIndex = 0;

        //Attacks
        tailMeleeSpeed = properties.tailMeleeSpeed;
        biteATKSpeed = properties.biteATKSpeed;
        biteCoolDown = properties.biteCoolDown;

        //Player Stats
        topdownPlayerHeight = properties.topdownPlayerHeight;
        invincibleTime = properties.invincibleTime;
        life = properties.lives;
        transform.position = new Vector3(transform.position.x, landmark.position.y, transform.position.z);

        //Changes during camera transition
        shoulderLStartRotation = shoulderAimL.transform.rotation;
        shoulderRStartRotation = shoulderAimR.transform.rotation;
        gunLStartRotation = gunsAimL.transform.rotation;
        gunRStartRotation = gunsAimR.transform.rotation;
        sideBodyColliderStartRot = sideBodyCollider.transform.rotation;
        topBodyColliderStartRot = topBodyCollider.transform.rotation;
        transformStartRotation = transform.rotation;
        armsAimStartRotation = armsAim.transform.rotation;

        //Player boundaries From Register
        sideXMin = Register.instance.xMin;
        sideXMax = Register.instance.xMax;
        sideYMin = Register.instance.yMin;
        sideYMax = Register.instance.yMax;
        topXMax = Register.instance.xMax;
        topXMin = Register.instance.xMin;
        topZMax = Register.instance.zMax;
        topZMin = Register.instance.zMin;
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
            //GURRA potrebbe essere utile inserire un timer per il salto, per evitare che il salto venga effettuato più volte nei vari update
            //GURRA non va assolutamente bene il getbuttondown qui. Nel fixed update rischio di perdere l'input. mettere il controllo sull'input nell'update e lasciare il salto qua nel fixed.
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
            //GURRA da rivedere questa parte, è evidente che in game succedono cose strane, tipo il player che sta mezz'ora sul lato alto dello schermo
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
                //GURRA rivedere questa parte
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
                //GURRA assolutamente NO. vedo un sacco di numeri magici, questo -2, poi in StabilizeAcceleration c'è una cosa divia per 3. cosa rappresentano questi numeri? a cosa serve il metodo?
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

        if (transform.rotation != transformStartRotation)
        {
            transform.rotation = transformStartRotation;
        }

        ClampPositionSidescroll();
        UpdateArmsRotation();
        //GURRA se playerstate è moving è ovvio che non sia attacking, no? poi viene usato il canjump come controllo del fatto che lui non stia saltando. Non va bene, ci va una variabile apposta. Io magari posso dire che trevor sta camminando nella 
        //melma e quindi non può saltare, ma questo non significa che stia già saltando. Vedi sopra quando viene gestito il glide.
        if ((currentPlayerState == PlayerState.Moving) && player.GetButtonDown("Meele") && currentPlayerState != PlayerState.Attacking && canJump)
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

            TurnAroundGO(transform);

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
            if (!isInvincible && other.gameObject.layer == enemyLayer)
            {
                life--;
                if (IsDead())
                {
                    KillPlayer();
                }

                if (other.transform.tag.StartsWith("EnemyBullet"))
                {
                    other.transform.gameObject.SetActive(false);
                }
            }
        }
        //GURRA aggiustare questo if, fare riferimento allo stato Attacking, così c'è anche meno ridondanza
        else if (topTailCollider.enabled)
        {
            if (other.gameObject.layer == enemyLayer && !IsDead())
            {
                other.transform.gameObject.SetActive(false);
            }
        }

        if (currentPlayerState == PlayerState.Attacking)
        {
            if (other.gameObject.layer == enemyLayer && !IsDead())
            {
                other.transform.gameObject.SetActive(false);
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
        animator.SetBool("sidescroll", isSidescroll);
        transform.rotation = transformStartRotation;
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
        anim_isFlying = true;
        anim_isMovingBackwards = false;
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
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        //GURRA sintetizzate qua con return physics.raycast...
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

    //GURRA a cosa serve passare il transform in un metodo interno???
    void TurnAroundGO(Transform transform)
    {
        //GURRA questo metodo viene chiamato in top down, controllo superfluo.
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
            landmark.position.y + topdownPlayerHeight,
        Mathf.Clamp(transform.position.z, topZMin, topZMax)
        );
    }

    IEnumerator ChangePerspective()
    {
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
        if (life <= 0)
        {
            currentPlayerState = PlayerState.Dead;
            return true;
        }
        return false;
    }

    public void ResetPlayerLives()
    {
        life = properties.lives;
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
        rb.velocity = new Vector3(0, biteATKSpeed, 0);
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

    private void KillPlayer()
    {
        currentPlayerState = PlayerState.Dead;
        playerModel.SetActive(false);
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(RespawnTimer);
        ResetPlayerLives();
        StartCoroutine(InvinciblePlayer());
        currentPlayerState = PlayerState.Moving;
        playerModel.SetActive(true);
        //Reset Positiion after being hit ?
        //GURRA sì, resettiamo la posizione
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        rb.velocity = Vector3.zero;

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
