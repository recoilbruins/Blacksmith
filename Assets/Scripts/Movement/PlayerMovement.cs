using BlackSmithAnimator;
using BlackSmithInput;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speed Controls")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7f;
    [SerializeField] private float groundedRotationSpeed = 10f;


    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isJumping;
    [HideInInspector]
    public bool isDodging;
    [HideInInspector]
    public bool isUsingRootMotion;
    [HideInInspector]
    private bool isBlocking;

    [Header("Falling")]
    public float airTimer;
    public float leapingVelocity;
    public float fallingSpeed;
    public float rayCastHeight = 0;
    public float raycastRadius;
    public LayerMask groundLayer;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    [Header("Dodge Force")]
    [SerializeField] private float dodgeForce;

    bool isAnimationLocked = false;
    bool checkForBeingStuck = false;
    float timeElapsed = 0f;

    public Rigidbody rb;
    
    AnimationManager animationManager;

    Transform cam;
    Vector3 moveDirection;

    private void Awake()
    {
        animationManager = GetComponent<AnimationManager>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }
    public void UpdateAnimationBools()
    {
        isAnimationLocked = animationManager.animator.GetBool("isAnimationLocked");
        isUsingRootMotion = animationManager.animator.GetBool("isUsingRootMotion");
        isJumping = animationManager.animator.GetBool("isJumping");
        animationManager.animator.SetBool("isGrounded", isGrounded);
        isDodging = animationManager.animator.GetBool("isDodging");
        isBlocking = animationManager.animator.GetBool("isBlocking");
    }

    private void Update()
    {
        if(isJumping)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed > 0.25f)
            {
                checkForBeingStuck = true;
                timeElapsed = 0f;
            }
        }
        else
        {
            if(timeElapsed > 0f)
            {
                timeElapsed = 0f;
            }
        }
    }


    public void UpdateAllMovement(float moventSpeedMultiplier)
    {
        HandleFallingAndLanding();
        if (isAnimationLocked) { return; }
        Movement(moventSpeedMultiplier);
        Rotation();
        Jump();
        Dodge();
    }

    private void Movement(float moventSpeedMultiplier)
    {
        if (isJumping)
        {
            return;
        }
        moveDirection = cam.transform.forward * InputManager.instance.vertical;
        moveDirection = moveDirection + cam.right * InputManager.instance.horizontal;
        moveDirection.y = 0;

        if (InputManager.instance.isSprintPressed)
        {
            moveDirection *= sprintingSpeed * moventSpeedMultiplier;
        }
        else
        {
            if(InputManager.instance.moveAmount < 0.5f || isBlocking)
            {
                moveDirection *= walkingSpeed * moventSpeedMultiplier;
            }
            else
            {
                moveDirection *= runningSpeed * moventSpeedMultiplier;
            }
            
        }

        if(isGrounded && !isJumping)
        {
            Vector3 moveVelocity = moveDirection;
            rb.linearVelocity = moveVelocity;
        }

    }

    private void Rotation()
    {
        if (isJumping) { return; }

        Vector3 direction = Vector3.zero;

        direction = cam.forward * InputManager.instance.vertical;
        direction = direction + cam.right * InputManager.instance.horizontal;
        direction.Normalize();
        direction.y = 0;

        if(direction == Vector3.zero)
        {
            direction = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, groundedRotationSpeed * Time.deltaTime);
        if (isGrounded && !isJumping)
        {
            transform.rotation = playerRotation;
        }
        

    }

    public void HandleFallingAndLanding()
    {
        RaycastHit raycastHit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y += rayCastHeight;
        targetPosition = transform.position;

        if (!isGrounded && !isJumping)
        {
            if (!isAnimationLocked && airTimer > 0.2f)
            {
                animationManager.PlayAnimation("Falling", isAnimationLocked: true);
            }

            animationManager.animator.SetBool("isUsingRootMotion", false);
            airTimer = airTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingSpeed * airTimer);

        }

        if (Physics.SphereCast(rayCastOrigin, raycastRadius, -Vector3.up, out raycastHit, rayCastHeight, groundLayer))
        {
            if (!isGrounded && isAnimationLocked)
            {
                //animationManager.animator.SetBool("isGrounded", true);
                animationManager.PlayAnimation("Land", isAnimationLocked: true, isUsingRootMotion: true);
            }
            Vector3 rayCastHitPoint = raycastHit.point;
            targetPosition.y = rayCastHitPoint.y;
            isGrounded = true;
            airTimer = 0;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            /*Vector3 yVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = yVelocity;*/

            rb.linearVelocity = Vector3.zero;
            
            if (isAnimationLocked || InputManager.instance.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        // Check for if the player jumps along a wall and is stuck in a jumping state, set falling state
        if(checkForBeingStuck)
        {
            if (isGrounded && isJumping)
            {
                isJumping = false;
                animationManager.PlayAnimation("Falling", isAnimationLocked: true);
                checkForBeingStuck = false;
                timeElapsed = 0f;
            }
            else
            {
                checkForBeingStuck = false;
            }
        }
        
    }

    private void Jump()
    {
        if(InputManager.instance.isJumpPressed)
        {
            if(isGrounded)
            {
                animationManager.animator.SetBool("isJumping", true);
                animationManager.PlayAnimation("Jump", isAnimationLocked: false);

                /*float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                Vector3 playerVelocity = moveDirection;
                playerVelocity.y = jumpingVelocity;*/
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
            InputManager.instance.isJumpPressed = false;
        }
    }

    private void Dodge()
    {
        if (isAnimationLocked || !isGrounded || isJumping /*isBlocking*/) 
        {
            //inputManager.isDodgePressed = false;
            return; 
        }
        if(InputManager.instance.isDodgePressed)
        {
            if(InputManager.instance.moveAmount > 0)
            {
                animationManager.PlayAnimation("RollForwardBase", isAnimationLocked: true, isUsingRootMotion:true, isDodging:true);
            }
            else
            {
                animationManager.PlayAnimation("BackStep", isAnimationLocked: true, isUsingRootMotion:true, isDodging:true);
            }
            InputManager.instance.isDodgePressed = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = transform.position;
        pos.y -= rayCastHeight;
        Gizmos.DrawWireSphere(pos, raycastRadius);
    }
}
