using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Components")]
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Player Detection")]
    public Transform player;
    public float sightRange = 15f;
    public float shootRange = 10f;
    public LayerMask playerLayer;

    [Header("Wandering Settings")]
    public float wanderRadius = 20f;
    public float wanderDelay = 2f;
    private float wanderTimer;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 1f;
    private float shootTimer;
    private bool canSeePlayer = false;

    [Header("Audio Settings")]
    public AudioSource footstepAudioSource;
    public AudioSource projectileAudioSource;

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

        StartWandering();
    }

    private void Update()
    {
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

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private bool CheckPlayerVisibility()
    {
        if (!player) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.position, out hit, playerLayer))
            {
                return hit.collider.CompareTag("Player");
            }
        }
        return false;
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
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
        // stop before shooting
        agent.isStopped = true;
        // then trigger shot
        animator.SetTrigger("Shoot");
        // start attack cooldown
        shootTimer = shootCooldown;
    }

    // call from animation event frame
    public void OnShootEvent()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

            if (projectileScript != null)
            {
                projectileScript.SetShooter(transform); 
            }
        }

        // restore enemy movement after attack
        agent.isStopped = false; 
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = Random.insideUnitSphere * dist;
        randDir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, dist, layermask);
        return navHit.position;
    }

    public void ApplyDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " damage!");
        // do something when attack or dead
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
