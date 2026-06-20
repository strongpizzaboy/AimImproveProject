using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState { Idle, Moving, Hit }
    public EnemyState current_state = EnemyState.Moving;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private int health = 1;
    [SerializeField] private float idle_duration = 1f;
    [SerializeField] private float hit_duration = 0.2f;
    [SerializeField] private ParticleSystem death_effect;
    [SerializeField] private AudioClip death_sfx;
    [SerializeField] private Image _healthbar;

    private int maxHealth;
    private Vector3 startPosition;
    private float state_timer = 0f;
    private Renderer enemy_renderer;
    private Color original_color;

    void Start()
    {
        maxHealth = health;
        startPosition = transform.position;
        enemy_renderer = GetComponent<Renderer>();

        if (enemy_renderer)
            original_color = enemy_renderer.material.color;

        state_timer = Random.Range(0f, 2f);
    }

    void Update()
    {
        if (!LevelManager.IsPlaying) return;

        state_timer -= Time.deltaTime;

        switch (current_state)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;
            case EnemyState.Moving:
                UpdateMoving();
                break;
            case EnemyState.Hit:
                UpdateHit();
                break;
        }
    }

    void UpdateIdle()
    {
        if (state_timer <= 0f)
        {
            current_state = EnemyState.Moving;
            state_timer = Random.Range(2f, 5f);
        }
    }

    void UpdateMoving()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPosition + direction.normalized * offset;

        if (state_timer <= 0f)
        {
            current_state = EnemyState.Idle;
            state_timer = idle_duration;
        }
    }

    void UpdateHit()
    {
        if (state_timer <= 0f)
        {
            if (enemy_renderer)
                enemy_renderer.material.color = original_color;

            current_state = EnemyState.Moving;
            state_timer = Random.Range(2f, 5f);
        }
    }

    public void TakeHit(int damage = 1)
    {
        health -= damage;
        UpdateHealthBar();

        current_state = EnemyState.Hit;
        state_timer = hit_duration;

        if (enemy_renderer)
            enemy_renderer.material.color = Color.white;

        if (health <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthBar()
    {
        if (_healthbar)
            _healthbar.fillAmount = (float)health / maxHealth;
    }

    void Die()
    {
        if (death_effect)
            Instantiate(death_effect, transform.position, transform.rotation);

        if (death_sfx)
            AudioSource.PlayClipAtPoint(death_sfx, transform.position);

        LevelManager level_manager = FindAnyObjectByType<LevelManager>();
        if (level_manager)
            level_manager.EnemyKilled();

        Destroy(gameObject);
    }
}