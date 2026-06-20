using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private float shoot_range = 100f;
    [SerializeField] private AudioClip shoot_sfx;
    [SerializeField] private AudioClip hit_sfx;
    [SerializeField] private ParticleSystem hit_effect;

    private AudioSource audio_source;
    private int shot_count = 0;
    private int hit_count = 0;

    void Start()
    {
        audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shot_count++;

        if (shoot_sfx && audio_source)
        {
            audio_source.PlayOneShot(shoot_sfx);
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, shoot_range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (hit_effect)
            {
                Instantiate(hit_effect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                hit_count++;
                HitEnemy(hit.collider.gameObject);
            }
        }
    }

    void HitEnemy(GameObject enemy)
    {
        if (hit_sfx && audio_source)
        {
            audio_source.PlayOneShot(hit_sfx);
        }

        EnemyBehavior enemy_behavior = enemy.GetComponent<EnemyBehavior>();
        if (enemy_behavior)
        {
            enemy_behavior.TakeHit();
        }
    }

    public int GetShotCount() { return shot_count; }
    public int GetHitCount() { return hit_count; }
}