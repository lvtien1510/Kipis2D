using UnityEngine;
using UnityEngine.UI;

public class SoundToggleUI : MonoBehaviour
{
    [Header("UI Buttons")]
    public GameObject soundOnButton;
    public GameObject soundOffButton;
    public GameObject sfxOnButton;
    public GameObject sfxOffButton;

    private void Start()
    {
        UpdateUI();
    }

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
        UpdateUI();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool musicMuted = AudioManager.instance.IsMusicMuted();
        bool sfxMuted = AudioManager.instance.IsSFXMuted();

        soundOnButton.SetActive(!musicMuted);
        soundOffButton.SetActive(musicMuted);

        sfxOnButton.SetActive(!sfxMuted);
        sfxOffButton.SetActive(sfxMuted);
    }
}
