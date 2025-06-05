using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float startDelay = 0f;      // ⏳ Thời gian chờ trước khi bắt đầu
    [SerializeField] private float idleTime = 2f;
    [SerializeField] private float warningTime = 0.5f;
    [SerializeField] private float riseDuration = 2f;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D damageCollider;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (damageCollider != null) damageCollider.enabled = false;

        StartCoroutine(SpikeCycle());
    }

    private IEnumerator SpikeCycle()
    {
        // ⏳ Chờ trước khi bắt đầu
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // 1. Hạ xuống (Idle)
            SetAnimatorState(false, false, true); // isFalling = true
            damageCollider.enabled = false;
            yield return new WaitForSeconds(idleTime);

            // 2. Cảnh báo
            SetAnimatorState(true, false, false); // isWarning = true
            yield return new WaitForSeconds(warningTime);

            // 3. Nhô lên (Rise)
            SetAnimatorState(false, true, false); // isRising = true
            damageCollider.enabled = true;
            yield return new WaitForSeconds(riseDuration);

            // Quay lại Idle (Fall)
            SetAnimatorState(false, false, true); // isFalling = true
        }
    }

    private void SetAnimatorState(bool warning, bool rising, bool falling)
    {
        animator.SetBool("isWarning", warning);
        animator.SetBool("isRising", rising);
        animator.SetBool("isFalling", falling);
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
}
