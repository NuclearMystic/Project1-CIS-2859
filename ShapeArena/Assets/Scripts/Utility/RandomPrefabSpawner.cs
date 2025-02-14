using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] prefabsToSpawn;

    [Header("Spawn Timing")]
    public SpawnInterval spawnInterval = SpawnInterval.Medium;

    private float spawnTimer;

    public enum SpawnInterval
    {
        // 1 - 3 secs
        Short, 
        // 3 - 7 secs
        Medium,
        // 7 - 15 secs
        Long   
    }

    private void Start()
    {
        ResetSpawnTimer();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnPrefab();
            ResetSpawnTimer();
        }
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(GetMinSpawnTime(), GetMaxSpawnTime());
    }

    private float GetMinSpawnTime()
    {
        switch (spawnInterval)
        {
            case SpawnInterval.Short: return 1f;
            case SpawnInterval.Medium: return 3f;
            case SpawnInterval.Long: return 7f;
            default: return 1f;
        }
    }

    private float GetMaxSpawnTime()
    {
        switch (spawnInterval)
        {
            case SpawnInterval.Short: return 3f;
            case SpawnInterval.Medium: return 7f;
            case SpawnInterval.Long: return 15f;
            default: return 3f;
        }
    }

    private void SpawnPrefab()
    {
        if (prefabsToSpawn.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, prefabsToSpawn.Length);
        GameObject prefabToSpawn = prefabsToSpawn[randomIndex];

        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}
