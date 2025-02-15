using System.Collections;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn; 

    private void Start()
    {
        if (transform.childCount == 0)
        {
            SpawnObject();
        }
        StartCoroutine(CheckAndRespawn());
    }

    private IEnumerator CheckAndRespawn()
    {
        while (true)
        {
            if (transform.childCount == 0) 
            {
                yield return new WaitForSeconds(120f);
                if (transform.childCount == 0) 
                {
                    SpawnObject();
                }
            }
            yield return null; 
        }
    }

    private void SpawnObject()
    {
        if (objectsToSpawn.Length == 0) return;

        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        GameObject spawnedObject = Instantiate(objectsToSpawn[randomIndex], transform.position, Quaternion.identity);
        spawnedObject.transform.SetParent(transform);
    }
}
