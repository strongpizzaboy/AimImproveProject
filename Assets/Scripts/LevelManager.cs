using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float level_time = 60f;
    [SerializeField] private string next_level_name;
    [SerializeField] private TMP_Text timer_text;
    [SerializeField] private TMP_Text score_text;
    [SerializeField] private TMP_Text message_text;
    [SerializeField] private TMP_Text targets_remaining_text;
    [SerializeField] private TMP_Text floor_text;
    [SerializeField] private GameObject next_button;
    [SerializeField] private AudioClip win_sfx;
    [SerializeField] private AudioClip lose_sfx;
    [SerializeField] private float auto_advance_time = 3f;

    private float countdown;
    private int enemies_killed = 0;
    private int total_enemies = 0;
    private bool is_playing = false;
    private AudioSource audio_source;

    public static bool IsPlaying { get; private set; }

    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        countdown = level_time;
        IsPlaying = true;
        is_playing = true;

        EnemyBehavior[] all_enemies = FindObjectsByType<EnemyBehavior>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        total_enemies = all_enemies.Length;

        Debug.Log("Total enemies: " + total_enemies);

        if (message_text) message_text.enabled = false;
        if (next_button) next_button.SetActive(false);

        if (floor_text && GameManager.Instance != null)
            floor_text.text = "Floor " + GameManager.Instance.current_floor;

        UpdateScoreText();
        UpdateTargetsRemainingText();
    }

    void Update()
    {
        if (PauseMenu.IsPaused) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextLevel();
        }

        if (!is_playing) return;

        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = 0;
            LevelLost();
        }

        UpdateTimerText();
    }

    public void EnemyKilled()
    {
        enemies_killed++;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(100);
            GameManager.Instance.AddHit();
        }

        UpdateScoreText();
        UpdateTargetsRemainingText();

        Debug.Log("Enemies killed: " + enemies_killed + "/" + total_enemies);

        if (enemies_killed >= total_enemies)
        {
            LevelWon();
        }
    }

    void LevelWon()
    {
        is_playing = false;
        IsPlaying = false;

        PlaySound(win_sfx);
        DisplayMessage("Floor Cleared!\nLoading next floor...");

        if (next_button) next_button.SetActive(true);

        Invoke("LoadNextLevel", auto_advance_time);
    }

    public void LevelLost()
    {
        is_playing = false;
        IsPlaying = false;
        PlaySound(lose_sfx);
        DisplayMessage("Time's Up!");
        Invoke("ReloadScene", 2f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(next_level_name))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GoToNextFloor(next_level_name);
            else
                SceneManager.LoadScene(next_level_name);
        }
        else
        {
            Debug.LogWarning("Next level name not set!");
            Time.timeScale = 0f;
        }
    }

    void UpdateTimerText()
    {
        if (timer_text)
            timer_text.text = countdown.ToString("0.00");
    }

    void UpdateScoreText()
    {
        if (score_text)
            score_text.text = "Score: " + enemies_killed * 100;
    }

    void UpdateTargetsRemainingText()
    {
        if (targets_remaining_text)
        {
            int remaining = total_enemies - enemies_killed;
            targets_remaining_text.text = "Targets: " + remaining;
        }
    }

    void DisplayMessage(string message)
    {
        if (message_text)
        {
            message_text.text = message;
            message_text.enabled = true;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip && audio_source)
        {
            audio_source.clip = clip;
            audio_source.Play();
        }
    }
}