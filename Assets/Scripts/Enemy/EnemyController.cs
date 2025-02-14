using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public event System.Action OnDeath;

    [Header("Enemy Stats")]
    [SerializeField] private float enemyHealth = 25f;
    [SerializeField] private GameObject healthPickup;

    [Header("AI Components")]
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Player Detection")]
    private Transform player;
    [SerializeField] private float sightRange = 75f;
    [SerializeField] private float shootRange = 25f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Wandering Settings")]
    [SerializeField] private float wanderRadius = 20f;
    [SerializeField] private float wanderDelay = 2f;
    private float wanderTimer;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 1f;
    private float shootTimer;
    private bool canSeePlayer = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource projectileAudioSource;

    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        shootTimer = shootCooldown;
        wanderTimer = wanderDelay;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    private void Update()
    {
        if (isDead) return;

        shootTimer -= Time.deltaTime;
        canSeePlayer = CheckPlayerVisibility();

        if (canSeePlayer)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }


    private bool CheckPlayerVisibility()
    {
        if (!player) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange)
        {
            if (Physics.Linecast(transform.position, player.position, out RaycastHit hit, playerLayer))
            {
                return hit.collider.CompareTag("Player");
            }
        }
        return false;
    }

    private void ChasePlayer()
    {
        if (!player) return;
        agent.isStopped = false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // only begin to move if player is not within shootying range
        if (distanceToPlayer > shootRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position);

            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; 
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (distanceToPlayer <= shootRange && shootTimer <= 0f)
        {
            Attack();
        }
    }

    private void Wander()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            wanderTimer -= Time.deltaTime;

            if (wanderTimer <= 0f)
            {
                StartWandering();
                wanderTimer = wanderDelay;
            }
        }
    }

    private void StartWandering()
    {
        Vector3 randomPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(randomPos);
    }

    private void Attack()
    {
        agent.isStopped = true;

        // enemy needs to rotate to face player when not moving and trying to shoot
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; 
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        animator.SetTrigger("Shoot");
        shootTimer = shootCooldown;
    }

    public void OnShootEvent()
    {
        if (isDead) return;

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

            if (projectileScript != null)
            {
                projectileScript.SetShooter(transform);
            }
        }

        if (agent != null && agent.isOnNavMesh && !isDead)
        {
            agent.isStopped = false;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: agent is inactive or not on the NavMesh.");
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = Random.insideUnitSphere * dist;
        randDir += origin;
        NavMesh.SamplePosition(randDir, out NavMeshHit navHit, dist, layermask);
        return navHit.position;
    }

    public void DamageHealth(float damage)
    {
        if (isDead) return;

        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        if (isDead) return;

        UIController.Instance.UpdateScore(100);
        isDead = true;
        animator.SetTrigger("Die");

        if (agent != null)
        {
            agent.isStopped = true;
            if (!agent.isOnNavMesh)
            {
                Debug.LogWarning($"{gameObject.name} died but was not on the NavMesh!");
            }
            agent.enabled = false; 
        }

        OnDeath?.Invoke();
        StartCoroutine(DespawnEnemy());
    }

    private IEnumerator DespawnEnemy()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(healthPickup, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void PlayFootstepSFX()
    {
        footstepAudioSource.Play();
    }

    public void PlayShootSFX()
    {
        projectileAudioSource.Play();
    }
}
