using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public GameObject wKeyUI;         // UI hiển thị nút "W"
    public string nextSceneName;      // Tên Scene tiếp theo
    private bool playerInRange = false;
    private GameObject player;
    private Animator animator;
    AudioManager audioManager;
    [SerializeField] private Animator transitionAnim;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        if (wKeyUI != null)
            wKeyUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.W))
        {
            wKeyUI.SetActive(false);
            StartCoroutine(EnterDoor());
        }
    }

    IEnumerator EnterDoor()
    {
        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
            anim.Play("DoorIn");
        animator.SetTrigger("DoorIn");
        audioManager.PlaySFX(audioManager.diamond);
        SceneTransitionManager.Instance.TransitionToScene(nextSceneName);

        // Lưu tiến độ nếu nextScene là một màn chơi hợp lệ (bắt đầu bằng "Lv")
        if (nextSceneName.StartsWith("Lv"))
        {
            int currentLevel = int.Parse(SceneManager.GetActiveScene().name.Replace("Lv", ""));
            LevelManager.Instance.UnlockNextLevel(currentLevel);
        }

        yield return new WaitForSeconds(1f);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            if (wKeyUI != null)
                wKeyUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (wKeyUI != null)
                wKeyUI.SetActive(false);
        }
    }
}
