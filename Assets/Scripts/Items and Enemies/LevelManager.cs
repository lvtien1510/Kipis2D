using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private const string LevelKey = "LevelUnlocked";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 1); // Mặc định mở khóa Lv1
    }

    public void UnlockNextLevel(int currentLevel)
    {
        int unlocked = GetUnlockedLevel();
        if (currentLevel >= unlocked)
        {
            PlayerPrefs.SetInt(LevelKey, currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    public bool IsLevelUnlocked(int level)
    {
        return level <= GetUnlockedLevel();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level Selection")
        {
            // Cập nhật lại các nút level sau khi reset
            LevelButton[] levelButtons = FindObjectsOfType<LevelButton>();
            foreach (var btn in levelButtons)
            {
                bool isUnlocked = IsLevelUnlocked(btn.level);
                btn.button.interactable = isUnlocked;
                if (btn.lockImage != null)
                    btn.lockImage.SetActive(!isUnlocked);
            }
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        for (int i = 1; i <= 15; i++) // Giả sử bạn có tối đa 10 màn chơi
        {
            PlayerPrefs.DeleteKey("Diamond_Lv" + i);
        }

        PlayerPrefs.Save();
        SceneTransitionManager.Instance.TransitionToScene("Level Selection");
    }
}
