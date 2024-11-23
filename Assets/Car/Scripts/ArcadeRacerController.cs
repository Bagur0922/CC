using UnityEngine;

public class ArcadeRacerController : MonoBehaviour
{
    [Header("Car Properties")]
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float brakeForce = 50f;
    [SerializeField] private float dragFactor = 0.95f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float gripFactor = 0.9f;
    [SerializeField] private float driftThreshold = 0.8f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer; // Set this to your ground layer in Inspector
    [SerializeField] private float groundRayLength = 1f; // Distance to check for ground
    [SerializeField] private float groundRayHeight = 1f; // Distance to check for ground
    [SerializeField] private float gravityForce = 20f; // Force of gravity
    [SerializeField] private float groundedGravityForce = 5f; // Smaller gravity when grounded
    [SerializeField] private float heightOffset = 0.5f; // Desired height above ground
    [SerializeField] private float springForce = 50f; // Force to maintain height
    [SerializeField] private float springDamping = 5f; // Damping for smooth landing

    [Header("Drift Properties")]
    [SerializeField] private float minDriftAngle = 15f;
    [SerializeField] private float maxDriftAngle = 45f;
    [SerializeField] private float driftRecoveryRate = 2f;

    [Header("Boost Properties")]
    [SerializeField] private float boostForce = 50f;
    [SerializeField] private float boostDuration = 2f;
    [SerializeField] private float boostCooldown = 5f;

    // Private variables for car state
    private float currentSpeed;
    private float currentTurnAngle;
    private float driftFactor;
    private bool isDrifting;
    private bool canBoost = true;
    private float boostTimer;
    private float boostCooldownTimer;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool isGrounded;
    private RaycastHit groundHit;
    private float verticalVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down * 0.5f; // Lower center of mass for better stability
        rb.useGravity = false; // We'll handle gravity manually
    }

    private void Update()
    {
        HandleInput();
        HandleBoosting();
    }

    private void FixedUpdate()
    {
        CheckGround();
        ApplyGravity();
        ApplyMovement();
        ApplyDrift();
    }

    private void CheckGround()
    {
        Transform center = transform;
        center.position = center.position + Vector3.up * groundRayHeight;

        // Cast a ray downward to detect ground
        isGrounded = Physics.Raycast(center.position, Vector3.down, out groundHit, groundRayLength, groundLayer);
        Debug.DrawRay(center.position, Vector3.down * groundRayLength, Color.red, 1f);
        Debug.Log(isGrounded);

        if (isGrounded)
        {
            // Calculate the desired position above ground
            float targetHeight = groundHit.point.y + heightOffset;
            float currentHeight = transform.position.y;
            float heightError = targetHeight - currentHeight;

            // Apply spring force to maintain height
            float springVelocity = (heightError * springForce) - (rb.velocity.y * springDamping);
            verticalVelocity = springVelocity;

            // Align with ground normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            // Apply stronger gravity when in air
            verticalVelocity -= gravityForce * Time.fixedDeltaTime;
        }
        else
        {
            // Apply lighter gravity when grounded to keep the car pressed to the ground
            verticalVelocity -= groundedGravityForce * Time.fixedDeltaTime;
        }
    }

    private void HandleInput()
    {
        // Get input values
        float accelerationInput = Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");
        bool driftInput = Input.GetButton("Jump");
        bool boostInput = Input.GetButton("Fire1");

        // Only allow acceleration when grounded
        if (isGrounded)
        {
            if (accelerationInput > 0)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            }
            else if (accelerationInput < 0)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed * 0.5f, brakeForce * Time.deltaTime);
            }
            else
            {
                currentSpeed *= dragFactor;
            }

            // Handle steering
            float targetTurnAngle = steeringInput * turnSpeed;
            currentTurnAngle = Mathf.Lerp(currentTurnAngle, targetTurnAngle, Time.deltaTime * 4f);

            // Handle drifting
            if (driftInput && Mathf.Abs(steeringInput) > driftThreshold && currentSpeed > maxSpeed * 0.5f)
            {
                isDrifting = true;
                driftFactor = Mathf.Lerp(driftFactor, 1f, Time.deltaTime);
            }
            else
            {
                isDrifting = false;
                driftFactor = Mathf.Lerp(driftFactor, 0f, Time.deltaTime * driftRecoveryRate);
            }
        }
        else
        {
            // Reduce turn control in air
            float airControlFactor = 0.3f;
            float targetTurnAngle = steeringInput * turnSpeed * airControlFactor;
            currentTurnAngle = Mathf.Lerp(currentTurnAngle, targetTurnAngle, Time.deltaTime * 2f);
        }

        // Handle boosting
        if (boostInput && canBoost && isGrounded)
        {
            ActivateBoost();
        }
    }

    private void ApplyMovement()
    {
        // Calculate movement direction
        moveDirection = transform.forward * currentSpeed;

        // Combine horizontal movement with vertical velocity
        Vector3 finalVelocity = moveDirection + (Vector3.up * verticalVelocity);
        rb.velocity = finalVelocity;

        // Apply rotation
        if (isGrounded)
        {
            float turnAmount = currentTurnAngle * Time.fixedDeltaTime;
            if (isDrifting)
            {
                turnAmount *= 1f + driftFactor;
            }
            transform.Rotate(0f, turnAmount, 0f);

            // Apply grip
            Vector3 lateralVelocity = Vector3.Project(rb.velocity, transform.right);
            rb.velocity -= lateralVelocity * gripFactor * (isDrifting ? (1f - driftFactor) : 1f);
        }
    }

    private void ApplyDrift()
    {
        if (isDrifting && isGrounded)
        {
            float driftAngle = Mathf.Lerp(minDriftAngle, maxDriftAngle, driftFactor) * Mathf.Sign(currentTurnAngle);
            Vector3 newRotation = transform.eulerAngles;
            newRotation.y += driftAngle;
            // carModel.rotation = Quaternion.Euler(newRotation);
        }
    }

    private void ActivateBoost()
    {
        canBoost = false;
        boostTimer = boostDuration;
        boostCooldownTimer = boostCooldown;
    }

    private void HandleBoosting()
    {
        if (boostTimer > 0)
        {
            boostTimer -= Time.deltaTime;
            currentSpeed += boostForce * Time.deltaTime;
        }

        if (boostCooldownTimer > 0)
        {
            boostCooldownTimer -= Time.deltaTime;
            if (boostCooldownTimer <= 0)
            {
                canBoost = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize ground detection ray in Scene view
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRayLength);
    }

    // Public getters for UI or other systems
    public float GetSpeed() => currentSpeed;
    public bool IsDrifting() => isDrifting;
    public float GetBoostCooldown() => boostCooldownTimer;
    public bool IsGrounded() => isGrounded;
}