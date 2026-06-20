using UnityEngine;
using TMPro;
using System.Collections;


public class GunBehavior : MonoBehaviour
{
    [Header("Gun Stats")]
    public GameObject crosshair;
    public string gun_name = "Pistol";
    public int damage = 1;
    public float fire_rate = 0.5f;
    public int max_ammo = 12;
    public int reserve_ammo = 36;
    public float reload_time = 1.5f;
    public float shoot_range = 100f;
    public GameObject bullet_prefab;
    public Transform bullet_spawn_point;

    [Header("Scope Settings")]
    public bool can_scope = false;
    public float scoped_fov = 20f;
    public float normal_fov = 60f;
    public GameObject scope_overlay;

    [Header("Audio")]
    public AudioClip shoot_sfx;
    public AudioClip reload_sfx;
    public AudioClip empty_sfx;
    public AudioClip scope_sfx;

    [Header("Effects")]
    public Animator animator;
    public ParticleSystem muzzle_flash;
    public ParticleSystem hit_effect;

    private int current_ammo;
    private bool is_reloading = false;
    private bool is_scoped = false;
    private float next_fire_time = 0f;
    private AudioSource audio_source;
    private Camera player_camera;

    void Start()
    {
        animator = GetComponent<Animator>();
        current_ammo = max_ammo;
        audio_source = GetComponent<AudioSource>();
        player_camera = Camera.main;

        if (scope_overlay) scope_overlay.SetActive(false);
    }

    void OnEnable()
    {
        is_reloading = false;
        if (scope_overlay) scope_overlay.SetActive(false);
        if (player_camera) player_camera.fieldOfView = normal_fov;
        is_scoped = false;
    }

    void OnDisable()
    {
        is_scoped = false;
        if (scope_overlay) scope_overlay.SetActive(false);
        if (player_camera) player_camera.fieldOfView = normal_fov;
    }

    void Update()
    {
        if (!LevelManager.IsPlaying) return;

        HandleShoot();
        HandleReload();
        HandleScope();
    }

    void HandleShoot()
    {
        if (is_reloading) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (current_ammo <= 0)
            {
                PlaySound(empty_sfx);
                if (reserve_ammo > 0)
                {
                    StartCoroutine(Reload());
                }
                return;
            }

            if (Time.time >= next_fire_time)
            {
                Shoot();
                next_fire_time = Time.time + fire_rate;
                
            }
        }
    }

    void Shoot()
    {
        current_ammo--;
        PlaySound(shoot_sfx);
        animator.SetTrigger("Shooting");

        if (GameManager.Instance != null)
            GameManager.Instance.AddShot();

        if (bullet_prefab && bullet_spawn_point)
        {
            Instantiate(bullet_prefab, bullet_spawn_point.position, 
                bullet_spawn_point.rotation);
        }

        if (muzzle_flash) muzzle_flash.Play();

        RaycastHit hit;
        if (Physics.Raycast(player_camera.transform.position,
            player_camera.transform.forward, out hit, shoot_range))
        {
            if (hit_effect)
                Instantiate(hit_effect, hit.point,
                    Quaternion.LookRotation(hit.normal));

            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyBehavior enemy = hit.collider.GetComponent<EnemyBehavior>();
                if (enemy) enemy.TakeHit(damage);
            }
        }
    }

    void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !is_reloading)
        {
            if (current_ammo < max_ammo && reserve_ammo > 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload()
    {
        is_reloading = true;
        PlaySound(reload_sfx);

        yield return new WaitForSeconds(reload_time);

        int bullets_needed = max_ammo - current_ammo;
        int bullets_to_load = Mathf.Min(bullets_needed, reserve_ammo);

        current_ammo += bullets_to_load;
        reserve_ammo -= bullets_to_load;

        is_reloading = false;
    }

    void HandleScope()
    {
        if (!can_scope) return;

        if (Input.GetButtonDown("Fire2"))
        {
            is_scoped = !is_scoped;

            if (is_scoped)
            {
                player_camera.fieldOfView = scoped_fov;
                if (scope_overlay)
                {
                    crosshair.SetActive(false);
                    scope_overlay.SetActive(true);
                }
                PlaySound(scope_sfx);
            }
            else
            {
                player_camera.fieldOfView = normal_fov;

                if (scope_overlay)
                {
                    scope_overlay.SetActive(false);
                    crosshair.SetActive(true);
                }
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip && audio_source)
        {
            audio_source.PlayOneShot(clip);
        }
    }

    public string GetAmmoDisplay()
    {
        if (is_reloading) return "Reloading...";
        if (reserve_ammo <= 0 && current_ammo <= 0) return "OUT OF AMMO";
        return current_ammo + " / " + reserve_ammo;
    }

    public void AddAmmo(int amount)
    {
        reserve_ammo += amount;
    }
}