using UnityEngine;
using System.Collections;

public class FallingRock : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField] private float warningTime = 2f;
    [SerializeField] private float brokenDuration = 3f;

    private Animator animator;
    private Collider2D platformCollider;

    private bool isBreaking = false;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (platformCollider == null) platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBreaking && collision.collider.CompareTag("Player"))
        {
            StartCoroutine(BreakSequence());
        }
    }

    private IEnumerator BreakSequence()
    {
        isBreaking = true;

        // Cảnh báo
        SetAnimatorState(warning: true, broken: false, restoring: false);
        yield return new WaitForSeconds(warningTime);

        // Bị vỡ
        SetAnimatorState(warning: false, broken: true, restoring: false);
        platformCollider.enabled = false;
        yield return new WaitForSeconds(brokenDuration);

        // Phục hồi
        SetAnimatorState(warning: false, broken: false, restoring: true);
        yield return new WaitForSeconds(1f); // animation khôi phục tầm 1s

        // Quay về idle
        SetAnimatorState(warning: false, broken: false, restoring: false);
        platformCollider.enabled = true;
        isBreaking = false;
    }

    private void SetAnimatorState(bool warning, bool broken, bool restoring)
    {
        animator.SetBool("isWarning", warning);
        animator.SetBool("isBroken", broken);
        animator.SetBool("isRestoring", restoring);
    }
}
