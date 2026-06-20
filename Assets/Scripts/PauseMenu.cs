using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pause_panel;
    [SerializeField] private Button resume_button;
    [SerializeField] private Button reset_button;
    [SerializeField] private Button exit_button;

    private bool is_paused = false;

    public static bool IsPaused { get; private set; }

    void Start()
    {
        if (pause_panel) pause_panel.SetActive(false);

        if (resume_button) resume_button.onClick.AddListener(Resume);
        if (reset_button) reset_button.onClick.AddListener(ResetLevel);
        if (exit_button) exit_button.onClick.AddListener(ExitGame);

        is_paused = false;
        IsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (is_paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        is_paused = true;
        IsPaused = true;
        Time.timeScale = 0f;

        if (pause_panel) pause_panel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        is_paused = false;
        IsPaused = false;
        Time.timeScale = 1f;

        if (pause_panel) pause_panel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResetLevel()
    {
        // always restore time scale before changing scenes
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
