using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionDuration = 2f;

    private void Awake()
    {
        // Đảm bảo chỉ có 1 Instance tồn tại
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        transitionAnimator.SetTrigger("End");

        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(sceneName);

        yield return null;
        transitionAnimator.SetTrigger("Start");

    }
}
