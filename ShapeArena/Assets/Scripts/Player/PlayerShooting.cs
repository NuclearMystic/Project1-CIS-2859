using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Camera playerCamera;
    public float shootRange = 100f;
    public float damage = 5f;
    public LayerMask shootableLayer;

    [Header("Effects")]
    public Transform muzzlePoint; 
    public ParticleSystem muzzleFlash;
    public GameObject impactEffectPrefab; 
    public GameObject bulletHolePrefab; 
    public AudioSource gunshotAudioSource; 

    [Header("Physics")]
    public float knockbackForce = 100f;

    bool canShoot = true;

    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot == true) 
        {
            Shoot();
            canShoot = false;
        }
    }

    private void Shoot()
    {
        PlayMuzzleFlash();
        PlayGunshotSound();

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootRange, shootableLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);

            HandleEnemyHit(hit);
            ApplyKnockback(hit);
            CreateBulletHole(hit);
            CreateImpactFlash(hit);
        }

        StartCoroutine(ShotCooldown());
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash)
        {
            muzzleFlash.Stop();
            muzzleFlash.Play();
        }
    }

    private void PlayGunshotSound()
    {
        if (gunshotAudioSource)
        {
            gunshotAudioSource.Play();
        }
    }

    private void HandleEnemyHit(RaycastHit hit)
    {
        EnemyController enemy = hit.collider.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.DamageHealth(damage);
        }

        ArenaShape shape = hit.collider.GetComponent<ArenaShape>();
        if (shape != null)
        {
            shape.ApplyDamage((int)damage); 
        }
    }

    private void ApplyKnockback(RaycastHit hit)
    {
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 knockbackDirection = -hit.normal;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    private void CreateBulletHole(RaycastHit hit)
    {
        if (bulletHolePrefab)
        {
            if (!hit.collider.gameObject.CompareTag("Enemy"))
            {
                Vector3 spawnPosition = hit.point + hit.normal * 0.01f;
                Quaternion spawnRotation = Quaternion.LookRotation(hit.normal);
                GameObject bulletHole = Instantiate(bulletHolePrefab, spawnPosition, spawnRotation);
                bulletHole.transform.SetParent(hit.collider.transform);
            }

        }
    }

    private void CreateImpactFlash(RaycastHit hit)
    {
        if (impactEffectPrefab)
        {
            GameObject impactFlash = Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactFlash, 0.1f);
        }
    }
}
