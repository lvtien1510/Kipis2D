using TMPro;
using UnityEngine;

public class LevelScoreDisplay : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        string key = "Diamond_Lv" + level;
        int score = PlayerPrefs.GetInt(key, 0);
        scoreText.text = score.ToString();
    }
}
