using Unity.VisualScripting;
using UnityEngine;

public class ArcadeRacerController : MonoBehaviour
{
    [Header("Car Properties")]
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float brakeForce = 50f;
    [SerializeField] private float dragFactor = 0.95f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float changeDirectionFactor = 0.9f;
    [SerializeField] private float dirFactor = 0.9f;
    [SerializeField] private float gravityFactor = 1.5f;

    [Header("Drift Properties")]
    [SerializeField] private float minDriftAngle = 15f;
    [SerializeField] private float maxDriftAngle = 45f;

    [Header("Boost Properties")]
    [SerializeField] private float boostForce = 50f;
    [SerializeField] private float boostDuration = 2f;
    [SerializeField] private float boostCooldown = 5f;

    [Header("GroundCheck")]
    [SerializeField] private float groundCheckLength;
    [SerializeField] private float groundCheckHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private RaycastHit groundHit;

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

    public float dmg = 10;

    private void Start()
    {
        GameManager.Instance.controller = this;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down * 0.5f; // Lower center of mass for better stability
    }

    private void Update()
    {
        GroundCheck();
        HandleInput();
        HandleBoosting();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyDrift();
    }
    private void GroundCheck()
    {
        Vector3 center = transform.position;
        center = transform.position + Vector3.up * groundCheckHeight;

        isGrounded = Physics.Raycast(center, Vector3.down, out groundHit, groundCheckLength, groundLayer);

        Debug.Log(isGrounded);

    }
    private void HandleInput()
    {
        // Get input values
        float accelerationInput = Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");
        bool driftInput = Input.GetButton("Jump");
        bool boostInput = Input.GetButton("Fire1");

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
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, dragFactor * Time.deltaTime);
        }

        // Handle steering
        if(Mathf.Abs(currentSpeed) >= changeDirectionFactor)
        {
            float steerChange = steeringInput * turnSpeed;

            transform.Rotate(0, steerChange, 0);
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
        moveDirection = transform.forward;
        moveDirection.y = 0;
        moveDirection.Normalize();
        moveDirection *= currentSpeed;

        if (!isGrounded)
        {
            Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * dirFactor);

        }

        // Combine horizontal movement with vertical velocity
        Vector3 finalVelocity = moveDirection + (Vector3.up * (isGrounded ? rb.velocity.y : -gravityFactor * Mathf.Abs(rb.velocity.y)));
        rb.velocity = finalVelocity;
    }

    private void ApplyDrift()
    {
        return;
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
        return;
        canBoost = false;
        boostTimer = boostDuration;
        boostCooldownTimer = boostCooldown;
    }

    private void HandleBoosting()
    {
        return;
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
        Vector3 center = transform.position;
        center = transform.position + Vector3.up * groundCheckHeight;

        // Visualize ground detection ray in Scene view
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(center, center + Vector3.down * groundCheckLength);
    }



    // Public getters for UI or other systems
    public float GetSpeed() => currentSpeed;
    public bool IsDrifting() => isDrifting;
    public float GetBoostCooldown() => boostCooldownTimer;
    public bool IsGrounded() => isGrounded;
}