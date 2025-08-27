using BlackSmithAnimator;
using BlackSmithInput;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speed Controls")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7f;
    [SerializeField] private float groundedRotationSpeed = 15f;
    [SerializeField] private float lockOnRotationSpeed = 15f;


    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public bool isJumping;
    [HideInInspector]
    public bool isDodging;
    [HideInInspector]
    public bool isUsingRootMotion;
    [HideInInspector]
    public bool isLockedOn;
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

    private bool isAnimationLocked = false;
    private bool checkForBeingStuck = false;
    private float timeElapsed = 0f;

    [Header("Lock on Target")]
    public Transform lockOnTarget;


    public Rigidbody rb;
    
    private AnimationManager animationManager;

    private Transform cam;
    private Vector3 moveDirection;

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


    public void UpdateAllMovement(float movementSpeedMultiplier)
    {
        HandleFallingAndLanding();
        if (isAnimationLocked) { return; }
        if(!isLockedOn || InputManager.instance.isSprintPressed)
        {
            NormalMovement(movementSpeedMultiplier);
            NormalRotation();
        }
        else
        {
            NormalMovement(movementSpeedMultiplier);
            LockOnRotation();
        }
        Jump();
        Dodge();
    }

    private void NormalMovement(float moventSpeedMultiplier)
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



    private void NormalRotation()
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

    private void LockOnRotation()
    {
        Vector3 direction = lockOnTarget.position - transform.position;
        direction.y = 0f; // Ignore vertical difference

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnRotationSpeed * Time.deltaTime);

        // Apply only Y-axis rotation
        transform.rotation = Quaternion.Euler(0f, smoothedRotation.eulerAngles.y, 0f);
    }

    public void HandleFallingAndLanding()
    {
        RaycastHit raycastHit;
        Vector3 rayCastOrigin = transform.position + Vector3.up * rayCastHeight;
        Vector3 targetPosition = transform.position;

        bool wasGrounded = isGrounded;
        isGrounded = Physics.SphereCast(rayCastOrigin, raycastRadius, Vector3.down, out raycastHit, rayCastHeight, groundLayer);
        bool isAboutToLand = false;

        if (!isGrounded && !isJumping)
        {
            float predictionDistance = 1.5f + (rb.linearVelocity.y * Time.deltaTime); // slightly adaptive

            if (Physics.SphereCast(rayCastOrigin, raycastRadius, Vector3.down, out raycastHit, predictionDistance, groundLayer))
            {
                isAboutToLand = true;
            }

            if (!isAnimationLocked && airTimer > 0.2f)
            {
                animationManager.PlayAnimation("Falling", isAnimationLocked: true);
            }

            if (isAboutToLand)
            {
                animationManager.PlayAnimation("Land", isAnimationLocked: true, transitionDuration: 0f, isUsingRootMotion: true);
                //animationManager.animator.SetBool("isGrounded", true);
            }

            animationManager.animator.SetBool("isUsingRootMotion", false);
            airTimer += Time.deltaTime;

            rb.AddForce(transform.forward * leapingVelocity, ForceMode.Acceleration);
            rb.AddForce(Vector3.down * fallingSpeed * airTimer, ForceMode.Acceleration);
        }

        else
        {
            if (!wasGrounded && isAnimationLocked)
            {
                // Just landed
                animationManager.PlayAnimation("Land", isAnimationLocked: true, isUsingRootMotion: true);
                // Optional: pass airTimer as a fallDistance to scale land impact
                // animationManager.animator.SetFloat("fallDistance", airTimer);
            }

            // Snap position to ground if needed
            if (isGrounded)
            {
                targetPosition.y = raycastHit.point.y;
                airTimer = 0f;
            }
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
                //animationManager.PlayAnimation("Falling", isAnimationLocked: true, transitionDuration:0f);
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
            if (isLockedOn)
            {
                if (InputManager.instance.horizontal >= 0.5f)
                {
                    animationManager.PlayAnimation("RollRightBase", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);
                }
                else if (InputManager.instance.horizontal <= -0.5f)
                {
                    animationManager.PlayAnimation("RollLeftBase", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);
                }
                else if (InputManager.instance.vertical > 0.5f)
                {
                    animationManager.PlayAnimation("RollForwardBase", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);
                }
                else if (InputManager.instance.vertical < -0.5f)
                {
                    animationManager.PlayAnimation("RollBackwardBase", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);
                }
                else
                {
                    animationManager.PlayAnimation("BackStep", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);
                }
                // animationManager.PlayAnimation("BaseRoll", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);

            }
            else
            {
                if (InputManager.instance.moveAmount > 0)
                {
                    animationManager.PlayAnimation("RollForwardBase", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);
                }
                else
                {
                    animationManager.PlayAnimation("BackStep", isAnimationLocked: true, isUsingRootMotion: true, isDodging: true);
                }
            }
            InputManager.instance.isDodgePressed = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + Vector3.up * rayCastHeight;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector3.down * rayCastHeight);
        Gizmos.DrawWireSphere(origin + Vector3.down * rayCastHeight, raycastRadius);
    }
}
