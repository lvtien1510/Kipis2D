using UnityEngine;
using System.Collections;

public class FallingSpikeTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float warningDelay = 0.5f;
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float fallSpeed = 10f;         // tốc độ rơi tùy chỉnh
    [SerializeField] private float stopOffset = 0.1f;       // khoảng cách dừng gần mặt đất
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D damageCollider;

    private bool isTriggered = false;
    private bool isFalling = false;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (damageCollider != null) damageCollider.enabled = false;
    }

    private void Update()
    {
        if (!isTriggered)
        {
            // Phát hiện Player bên dưới
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectRange, playerLayer);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                StartCoroutine(TriggerFall());
            }
        }

        if (isFalling)
        {
            // Raycast kiểm tra mặt đất
            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, stopOffset, groundLayer);
            if (groundHit.collider != null)
            {
                StopFallAtGround(groundHit.point);
            }
            else
            {
                // Di chuyển thủ công theo fallSpeed
                transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator TriggerFall()
    {
        isTriggered = true;

        // 1. Cảnh báo
        animator.SetTrigger("Warning");
        yield return new WaitForSeconds(warningDelay);

        // 2. Rơi xuống
        animator.SetTrigger("Fall");
        damageCollider.enabled = true;
        isFalling = true;
    }

    private void StopFallAtGround(Vector2 contactPoint)
    {
        isFalling = false;
        damageCollider.enabled = false;

        // Cắm vào đất
        float spikeHeight = damageCollider.bounds.extents.y;
        transform.position = new Vector3(transform.position.x, contactPoint.y + spikeHeight, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageCollider.enabled && collision.CompareTag("Player"))
        {
            Health health = collision.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(1, transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * detectRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * stopOffset);
    }
}
