using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip move;
    public AudioClip diamond;
    public AudioClip jump;
    public AudioClip attack;
    public AudioClip death;
    public AudioClip hit;
    public AudioClip explode;
    private const string MUSIC_MUTE_KEY = "MusicMuted";
    private const string SFX_MUTE_KEY = "SFXMuted";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.clip = background;

        // Load mute settings
        musicSource.mute = PlayerPrefs.GetInt(MUSIC_MUTE_KEY, 0) == 1;
        SFXSource.mute = PlayerPrefs.GetInt(SFX_MUTE_KEY, 0) == 1;

        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!SFXSource.mute)
            SFXSource.PlayOneShot(clip);
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt(MUSIC_MUTE_KEY, musicSource.mute ? 1 : 0);
    }

    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
        PlayerPrefs.SetInt(SFX_MUTE_KEY, SFXSource.mute ? 1 : 0);
    }

    public bool IsMusicMuted() => musicSource.mute;
    public bool IsSFXMuted() => SFXSource.mute;

    public AudioSource GetMusicSource() => musicSource;
    public AudioSource GetSFXSource() => SFXSource;
}
