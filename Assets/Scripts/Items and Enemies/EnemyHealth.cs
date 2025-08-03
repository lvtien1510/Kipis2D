using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP;
    private int currentHP;
    private bool isDead = false;

    public float knockbackForce = 5f;
    public float knockbackDuration = 0.8f;

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private MonoBehaviour[] scripts;
    private EnemyPatrol enemyPatrol;

    [SerializeField] private GameObject impulsePrefab;

    public bool isKnockedBack { get; private set; } = false;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        scripts = GetComponents<MonoBehaviour>();
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        if (isDead) return;

        currentHP -= damage;
        animator.SetTrigger("Hit");

        if (impulsePrefab != null)
        {
            // Kích hoạt lại nếu đang tắt
            impulsePrefab.SetActive(false); // Reset trạng thái
            impulsePrefab.SetActive(true);  // OnEnable sẽ được gọi
        }
        // Knockback
        Vector2 knockDir = (transform.position - attacker.position).normalized;
        knockDir += Vector2.up * 1f;
        knockDir.Normalize();

        rb.velocity = Vector2.zero;
        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(HandleKnockback());

        if (currentHP <= 0)
        {
            Die(attacker);
        }
    }

    IEnumerator HandleKnockback()
    {
        isKnockedBack = true;

        // Nếu có enemyPatrol, tạm dừng
        if (enemyPatrol != null)
            enemyPatrol.enabled = false;

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;

        // Cho phép patrol lại
        if (enemyPatrol != null && !isDead)
            enemyPatrol.enabled = true;
    }

    void Die(Transform attacker)
    {
        isDead = true;
        animator.SetTrigger("Dead");
        foreach (var script in scripts)
        {
            if (script != this)
                script.enabled = false;
        }

        // Ngắt va chạm với Player
        Collider2D playerCol = attacker.GetComponent<Collider2D>();
        if (enemyCollider != null && playerCol != null)
        {
            Physics2D.IgnoreCollision(enemyCollider, playerCol, true);
        }

        Destroy(gameObject, 5f);
    }
}
