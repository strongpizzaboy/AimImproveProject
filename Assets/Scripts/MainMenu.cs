using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button play_button;
    [SerializeField] private Button quit_button;
    [SerializeField] private Slider sensitivity_slider;
    [SerializeField] private Slider music_volume_slider;
    [SerializeField] private TMP_Text sensitivity_text;
    [SerializeField] private TMP_Text music_volume_text;
    [SerializeField] private TMP_Text progress_text;
    [SerializeField] private string first_level_name = "Floor 1";

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (play_button) play_button.onClick.AddListener(StartGame);
        if (quit_button) quit_button.onClick.AddListener(QuitGame);

        if (sensitivity_slider)
        {
            sensitivity_slider.minValue = 0.1f;
            sensitivity_slider.maxValue = 10f;
            sensitivity_slider.value = 2f;
            sensitivity_slider.onValueChanged.AddListener(OnSensitivityChanged);
        }

        if (music_volume_slider)
        {
            music_volume_slider.minValue = 0f;
            music_volume_slider.maxValue = 1f;
            music_volume_slider.value = 0.5f;
            music_volume_slider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        UpdateProgressText();
    }

    void StartGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetGame();

        SceneManager.LoadScene(first_level_name);
    }

    void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("sensitivity", value);
        if (sensitivity_text)
            sensitivity_text.text = "Sensitivity: " + value.ToString("0.0");
    }

    void OnMusicVolumeChanged(float value)
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(value);
        if (music_volume_text)
            music_volume_text.text = "Music: " + value.ToString("0.0");
    }

    void UpdateProgressText()
    {
        if (progress_text && GameManager.Instance != null)
        {
            progress_text.text = "Best Floor: " + GameManager.Instance.current_floor +
                "\nAccuracy: " + GameManager.Instance.GetAccuracy().ToString("0.0") + "%";
        }
    }
}