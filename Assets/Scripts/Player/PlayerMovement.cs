using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Speed")]
    [SerializeField] float speed;

    [Header("Jump")]
    [SerializeField] float jumpTime;
    [SerializeField] float jumpPower;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    private float maxFallSpeed = -12;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private float horizontalInput;
    private bool isJumping;
    private float jumpCounter;
    private Vector2 vecGravity;
    public bool isDead = false;
    private bool isSpringing = false;

    private Health health;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (isDead) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (!health.isKnockbacked)
        {
            Vector2 velocity = rb.velocity;
            velocity.x = horizontalInput * speed;
            rb.velocity = velocity;
        }

        animator.SetFloat("Move", Mathf.Abs(horizontalInput));
        animator.SetFloat("Jump", rb.velocity.y);
        animator.SetBool("IsGround", IsGround());
        if (!isSpringing)
        {
            // Start jump
            if (Input.GetButtonDown("Jump") && IsGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                isJumping = true;
                jumpCounter = 0;
            }

            // Continue jumping while holding
            if (rb.velocity.y > 0 && isJumping)
            {
                jumpCounter += Time.deltaTime;
                if (jumpCounter > jumpTime)
                    isJumping = false;

                float t = jumpCounter / jumpTime;
                float currentJumpM = jumpMultiplier;

                if (t > 0.5f)
                    currentJumpM *= (1 - t);

                Vector2 extraVelocity = vecGravity * currentJumpM * Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + extraVelocity.y);
            }

            // Cancel jump early
            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }

            // Apply gravity boost when falling
            if (rb.velocity.y < 0)
            {
                float fallSpeed = rb.velocity.y - vecGravity.y * fallMultiplier * Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, fallSpeed);
            }

            // Limit fall speed
            if (rb.velocity.y < maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
            }
        }
        Flip();
    }
    public void SetSpringing(float duration)
    {
        StartCoroutine(SpringingRoutine(duration));
    }

    private IEnumerator SpringingRoutine(float time)
    {
        isSpringing = true;
        yield return new WaitForSeconds(time);
        isSpringing = false;
    }

    private void Flip()
    {
        if (horizontalInput > 0.01f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else if (horizontalInput < -0.01f)
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
    }

    private bool IsGround()
    {
        return Physics2D.OverlapCapsule(
            groundCheck.position,
            new Vector2(0.4f, 0.1f),
            CapsuleDirection2D.Horizontal,
            0f,
            groundLayer
        );
    }
}
