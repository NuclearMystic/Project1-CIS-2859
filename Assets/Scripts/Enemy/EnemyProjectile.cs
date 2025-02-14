using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 10f;
    public float homingStrength = 0.1f;

    private Rigidbody rb;
    private Collider projectileCollider;
    private Transform shooter;
    private Transform player;

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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb.velocity = transform.forward * speed;

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 newDirection = Vector3.Lerp(rb.velocity.normalized, directionToPlayer, homingStrength).normalized;

        rb.velocity = newDirection * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            return;
        }

        if (other.CompareTag("SpawnedObject"))
        {
            other.GetComponent<ArenaShape>().ApplyDamage((int)damage);
        }

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
