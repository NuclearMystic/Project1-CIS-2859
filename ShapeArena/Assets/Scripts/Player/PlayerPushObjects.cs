using UnityEngine;

public class PlayerPushObjects : MonoBehaviour
{
    public float pushStrength = 10f; 

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            float pushForce = pushStrength / rb.mass; 

            rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }
}
