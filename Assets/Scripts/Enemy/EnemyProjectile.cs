using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 10f;

    private Rigidbody rb;
    private Collider projectileCollider;
    private Transform shooter;

    public void SetShooter(Transform enemy)
    {
        shooter = enemy;
        Collider enemyCollider = enemy.GetComponent<Collider>();

        if (enemyCollider != null)
        {
            projectileCollider = GetComponent<Collider>();
            if (projectileCollider != null)
            {
                Physics.IgnoreCollision(projectileCollider, enemyCollider);
            }
            else
            {
                Debug.LogError("Projectile collider not found!");
            }
        }
        else
        {
            Debug.LogError("Shooter collider not found!");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on projectile!");
            return;
        }

        rb.velocity = transform.forward * speed;

        Destroy(gameObject, lifetime);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit: " + other.name);

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
