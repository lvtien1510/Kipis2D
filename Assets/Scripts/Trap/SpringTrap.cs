using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTrap : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f; // Lực đẩy lên

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }

            PlayerMovement move = collision.GetComponent<PlayerMovement>();
            if (move != null)
            {
                move.SetSpringing(0.5f); // trong 0.5s không can thiệp velocity.y
            }

            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Bounce");
            }
        }
    }
}
