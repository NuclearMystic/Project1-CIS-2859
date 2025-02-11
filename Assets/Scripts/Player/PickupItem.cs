using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ItemType
    {
        Health,
        Speed,
        Jump
    }

    [Header("Pickup Settings")]
    public ItemType itemType; 
    public float effectAmount = 5f;
    public float effectDuration = 5f; 

    private Vector3 startPosition;
    public float floatSpeed = 2f;
    public float floatHeight = 0.2f;
    public float rotationSpeed = 50f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        HandleFloating();
    }

    private void HandleFloating()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.Instance.recoverSFX.Play();
            ApplyEffect(other);
            Destroy(gameObject);
        }
    }

    private void ApplyEffect(Collider player)
    {
        switch (itemType)
        {
            case ItemType.Health:
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.RestoreHealth(effectAmount);
                }
                break;

            case ItemType.Speed:
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.ApplySpeedBoost(effectAmount, effectDuration);
                }
                break;

            case ItemType.Jump:
                playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.ApplyJumpBoost(effectAmount, effectDuration);
                }
                break;
        }
    }
}
