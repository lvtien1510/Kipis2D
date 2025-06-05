using UnityEngine;

public class BulletTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float attackInterval = 3f;

    [Header("Bullet Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector2 shootDirection = Vector2.right;

    //| Hướng bắn mong muốn | Giá trị `shootDirection` |
    //| ------------------- | ------------------------ |
    //| Từ trái sang phải   | `(1, 0)`                 |
    //| Từ phải sang trái   | `(-1, 0)`                |
    //| Từ trên xuống       | `(0, -1)`                |
    //| Từ dưới lên         | `(0, 1)`                 |


    private Animator animator;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        InvokeRepeating(nameof(TriggerAttack), startDelay, attackInterval);
    }

    private void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    // Gọi từ Animation Event
    public void Shoot()
    {
        BulletPool.Instance.GetBullet(firePoint.position, shootDirection);
    }
}
