using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip background_music;
    [SerializeField] private float music_volume = 0.5f;

    private AudioSource audio_source;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audio_source = GetComponent<AudioSource>();
        audio_source.clip = background_music;
        audio_source.loop = true;
        audio_source.volume = music_volume;
        audio_source.Play();
    }

    public void SetVolume(float value)
    {
        music_volume = value;
        audio_source.volume = value;
    }
}