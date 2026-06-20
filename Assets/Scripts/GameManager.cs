using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int current_floor = 1;
    public int total_score = 0;
    public int total_shots = 0;
    public int total_hits = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int amount)
    {
        total_score += amount;
    }

    public void AddShot()
    {
        total_shots++;
    }

    public void AddHit()
    {
        total_hits++;
    }

    public float GetAccuracy()
    {
        if (total_shots == 0) return 0f;
        return (float)total_hits / total_shots * 100f;
    }

    public void GoToNextFloor(string next_scene)
    {
        current_floor++;
        SceneManager.LoadScene(next_scene);
    }

    public void ResetGame()
    {
        current_floor = 1;
        total_score = 0;
        total_shots = 0;
        total_hits = 0;
        SceneManager.LoadScene("MainMenu");
    }
}