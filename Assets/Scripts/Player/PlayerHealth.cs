using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 25f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        UIController.Instance.PlayerHitEffect();
        UIController.Instance.healthSlider.value = health;
        Debug.Log("Player took " + damage + " damage! Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(float value)
    {
        health += value;
        UIController.Instance.healthSlider.value = health;
    }

    private void Die()
    {
        GetComponent<PlayerMovement>().enabled = false;
        RoundController.Instance.LoseRound();
        Debug.Log("Player has died!");
    }
}
