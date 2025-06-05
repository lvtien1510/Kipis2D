using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private float nextAttackTime = 0f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayers;
    [Range(1f, 2.4f)]
    [SerializeField] private float attackRate; // Tốc độ tấn công

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAttack();

    }

    void HandleAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKey(KeyCode.K))
            {
                PerformAttack();
                nextAttackTime = Time.time + 1f / attackRate; // Tính thời gian cho lần tấn công tiếp theo
            }
        }
    }

    private void PerformAttack()
    {
        // Kích hoạt animation
        animator.SetTrigger("Attack");

        // Kiểm tra các enemy trong phạm vi
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            // Gây sát thương, truyền Transform để enemy biết hướng bị đánh
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1, transform);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
