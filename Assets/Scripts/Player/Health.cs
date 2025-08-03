using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int startingHealth = 3;
    private int currentHealth;

    [Header("Components")]
    [SerializeField] private GameObject[] components;

    [Header("UI")]
    [SerializeField] private HealthUI[] hearts;

    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private GameObject impulsePrefab;
    [SerializeField] private float bounceForce = 5f; // lực bật lên
    public bool isKnockbacked { get; private set; } = false;
    AudioManager audioManager;

    private bool isDead = false;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    public void TakeDamage(int damage, Transform attacker = null)
    {
        if (currentHealth <= 0 || isDead) return;

        int previousHealth = currentHealth;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        UpdateHeartUI(previousHealth);

        Bounce(attacker); // Bật ngược lại phía attacker

        // Camera Shake
        if (impulsePrefab != null)
        {
            impulsePrefab.SetActive(false);
            impulsePrefab.SetActive(true);
        }

        if (currentHealth > 0)
        {
            audioManager.PlaySFX(audioManager.hit);
            anim.SetTrigger("Hit");
        }
        else
        {
            isDead = true;
            anim.SetTrigger("Die");
            audioManager.PlaySFX(audioManager.death);
            foreach (GameObject component in components)
            {
                if (component != null)
                    Destroy(component, 3f);
            }

            StartCoroutine(ReloadSceneAfterDelay(1f));
        }
    }

    public void Bounce(Transform attacker)
    {
        if (rb != null && attacker != null)
        {
            isKnockbacked = true;

            Vector2 direction = (transform.position - attacker.position).normalized;
            direction.y = Mathf.Clamp(direction.y + 0.5f, 0.5f, 1f); // tạo vòng cung hợp lý
            direction.x = Mathf.Clamp(direction.x, -1f, 1f);

            rb.velocity = Vector2.zero;
            rb.AddForce(direction * bounceForce, ForceMode2D.Impulse);

            // Tắt knockback sau 0.5s
            StartCoroutine(ResetKnockback(0.5f));
        }
    }

    private IEnumerator ResetKnockback(float delay)
    {
        yield return new WaitForSeconds(delay);
        isKnockbacked = false;
    }


    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void AddHealth(int value)
    {
        int previousHealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);

        for (int i = previousHealth; i < currentHealth; i++)
        {
            if (i >= 0 && i < hearts.Length)
                hearts[i].gameObject.SetActive(true); // hồi máu
        }
    }

    private void UpdateHeartUI(int previousHealth)
    {
        for (int i = previousHealth - 1; i >= currentHealth; i--)
        {
            if (i >= 0 && i < hearts.Length)
            {
                hearts[i].BreakHeart();
            }
        }
    }
}
