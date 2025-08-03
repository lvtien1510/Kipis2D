using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA, pointB;
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float patrolWaitTime = 2f;
    public float attackCooldown = 3f;
    public float stuckCheckInterval = 1.5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 targetPoint;
    private bool isWaiting;
    private float lastAttackTime;
    private Animator animator;
    private Vector2 lastPosition;

    private bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPoint = (Vector2)pointB.position;
        //lastPosition = transform.position;
        //InvokeRepeating(nameof(CheckIfStuck), stuckCheckInterval, stuckCheckInterval);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        animator.SetFloat("Move", Mathf.Abs(rb.velocity.x));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.GetComponent<Health>().TakeDamage(1, transform);
        }
    }
    void Patrol()
    {
        if (isWaiting || isAttacking) return;

        float distance = Vector2.Distance(transform.position, targetPoint);
        if (distance < 0.2f)
        {
            StartCoroutine(WaitAtPoint());
            targetPoint = (targetPoint == (Vector2)pointA.position) ? (Vector2)pointB.position : (Vector2)pointA.position;
        }
        else
        {
            MoveTowards(targetPoint);
        }
    }

    System.Collections.IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(patrolWaitTime);
        isWaiting = false;
    }

    void ChasePlayer()
    {
        if (isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                StartAttack();
                lastAttackTime = Time.time;
            }
            rb.velocity = new Vector2(0, rb.velocity.y); // đứng yên khi tấn công
        }
        else
        {
            MoveTowards(player.position);
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetTrigger("Attack");
    }

    // Gọi trong Animation Event
    public void DealDamage()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            Debug.Log("Enemy deals damage to Player!");
            player.GetComponent<Health>().TakeDamage(1, this.transform);
        }
        isAttacking = false;
    }

    // Gọi trong Animation Event (cuối animation)
    public void EndAttack()
    {
        isAttacking = false;
    }

    void MoveTowards(Vector2 destination)
    {
        float direction = Mathf.Sign(destination.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // Xoay mặt
        if (direction != 0)
            transform.localScale = new Vector3(direction, 1, 1);
    }

    void CheckIfStuck()
    {
        float distanceMoved = Vector2.Distance(transform.position, lastPosition);

        if (distanceMoved < 0.05f && IsGrounded())
        {
            Debug.Log("Stuck detected → jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }

        lastPosition = transform.position;
    }

    bool IsGrounded()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.2f, groundLayer);
        return hit.collider != null;
    }
    void OnDrawGizmos()
    {
        // Vẽ đường patrol giữa A và B
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);

            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }

        // Vẽ detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vẽ attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Vẽ raycast kiểm tra grounded
        Gizmos.color = Color.white;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.1f);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.2f);
    }
}
