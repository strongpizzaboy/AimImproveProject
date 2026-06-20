using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bullet_speed = 50f;
    public float bullet_lifetime = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = transform.forward * bullet_speed;
        }
        Destroy(gameObject, bullet_lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy) enemy.TakeHit(1);
        }
        Destroy(gameObject);
    }
}