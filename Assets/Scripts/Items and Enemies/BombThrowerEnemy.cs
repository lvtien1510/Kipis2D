using UnityEngine;

public class BombThrowerEnemy : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private Transform player;

    [Header("Throwing")]
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float cooldown = 3f;

    private Animator animator;

    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttacking;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Flip về phía player
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1); // Quay sang trái
        else
            transform.localScale = new Vector3(-1, 1, 1);

        if (distance <= attackRange && Time.time >= lastAttackTime + cooldown && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    // 👇 Gọi từ Animation Event tại khung ném
    public void ThrowBomb()
    {
        if (bombPrefab == null || throwPoint == null || player == null) return;

        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (player.position - throwPoint.position).normalized;

            float adjustedForce = throwForce * 0.5f; // lực nhẹ hơn
            Vector2 throwDirection = direction + new Vector2(0, 0.5f); // thêm hướng lên nhẹ

            rb.velocity = Vector2.zero;
            rb.AddForce(throwDirection.normalized * adjustedForce, ForceMode2D.Impulse);
        }

        isAttacking = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
