using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int level;
    public Button button;
    public GameObject lockImage;

    private void Start()
    {
        bool isUnlocked = LevelManager.Instance.IsLevelUnlocked(level);
        button.interactable = isUnlocked;
        if (lockImage != null)
            lockImage.SetActive(!isUnlocked);

        button.onClick.AddListener(() => LoadLevel());
    }

    private void LoadLevel()
    {
        SceneTransitionManager.Instance.TransitionToScene("Lv" + level);
    }
}
