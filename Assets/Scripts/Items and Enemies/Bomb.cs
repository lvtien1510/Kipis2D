using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 2f;        // Sau khi chạm đất
    [SerializeField] private float maxLifetime = 5f;           // Tổng thời gian sống tối đa

    private Animator animator;
    private Rigidbody2D rb;

    private bool hasLanded = false;
    private bool isExploding = false;

    private float landedTimer = 0f;
    private float lifetimeTimer = 0f;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Đếm thời gian tồn tại kể từ khi ném
        lifetimeTimer += Time.deltaTime;

        // Nổ nếu sống quá lâu
        if (!isExploding && lifetimeTimer >= maxLifetime)
        {
            StopPhysics(); // đảm bảo dừng lực
            TriggerExplode();
        }

        // Nếu đã chạm đất và chưa nổ -> đếm để nổ sau delay
        if (hasLanded && !isExploding)
        {
            landedTimer += Time.deltaTime;
            if (landedTimer >= explosionDelay)
            {
                TriggerExplode();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            // Gây sát thương
            Health health = collision.collider.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(2, transform);

            StopPhysics();
            TriggerExplode();
        }
        else if (!hasLanded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hasLanded = true;
            StopPhysics();
        }
    }

    private void StopPhysics()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.isKinematic = true;
    }

    private void TriggerExplode()
    {
        if (isExploding) return;
        isExploding = true;
        audioManager.PlaySFX(audioManager.explode);
        animator.SetTrigger("Explode");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
