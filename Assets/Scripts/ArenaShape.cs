using System.Collections;
using UnityEngine;

public abstract class ArenaShape : MonoBehaviour
{
    public int health;
    public float minScale = 0.01f;
    private Vector3 spawnOffset = new Vector3(0.5f, 0.5f, 0.5f);
    private const int damageAmount = 10;

    public float shrinkDuration = 60;

    public void Start()
    {
        Debug.Log("start method called");
        StartCoroutine(ShrinkOverTime());
    }

    public IEnumerator ShrinkOverTime()
    {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            float progress = elapsedTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject); 
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            SplitOrDestroy();
        }
    }

    private void SplitOrDestroy()
    {
        if (transform.localScale.x > minScale)
        {
            SplitShape();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SplitShape()
    {
        Vector3 newScale = transform.localScale * 0.75f;
        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnPosition = transform.position + spawnOffset * (i - 1);
            GameObject newShape = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            newShape.transform.localScale = newScale;
            newShape.GetComponent<ArenaShape>().health = health / 2; 
        }
        Destroy(gameObject);
    }
}