using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public AudioSource hitSFXSource;

    public void TakeDamage(float damage)
    {
        health -= damage;
        UIController.Instance.PlayerHitEffect();
        Debug.Log("Player took " + damage + " damage! Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // respawn or game over logic here
    }
}
