using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float enemyHealth = 25f;
    private Animator animator;

    public GameObject healthPickup;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DamageHealth(float damage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        animator.SetTrigger("Die");
    }

    public void DespawnEnemy()
    {
        Instantiate(healthPickup, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
