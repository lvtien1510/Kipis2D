using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiamondManager : MonoBehaviour
{
    public TextMeshProUGUI textDiamond;
    public int currentDiamond;
    private int valuation = 1;
    AudioManager audioManager;

    private string levelKey;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        levelKey = GetDiamondKeyForCurrentLevel();
        currentDiamond = 0;
        textDiamond.text = currentDiamond.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            currentDiamond += valuation;
            textDiamond.text = currentDiamond.ToString();
            audioManager.PlaySFX(audioManager.diamond);
            Destroy(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        SaveDiamondScore();
    }

    private void SaveDiamondScore()
    {
        int previousBest = PlayerPrefs.GetInt(levelKey, 0);
        if (currentDiamond > previousBest)
        {
            PlayerPrefs.SetInt(levelKey, currentDiamond);
            PlayerPrefs.Save();
        }
    }

    private string GetDiamondKeyForCurrentLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name; // e.g., "Lv1"
        return "Diamond_" + currentScene;
    }
}
