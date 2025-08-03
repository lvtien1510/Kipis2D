using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifeTime = 5f;

    private Vector2 moveDirection;
    private float lifeTimer;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
            BulletPool.Instance.ReturnBullet(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health health = collision.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(1, transform);

            BulletPool.Instance.ReturnBullet(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}
