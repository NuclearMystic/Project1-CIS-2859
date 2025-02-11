using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public static RoundController Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject spawnPointOne;
    [SerializeField] private GameObject spawnPointTwo;
    [SerializeField] private GameObject gateOne;
    [SerializeField] private GameObject gateTwo;

    public float startCountdown = 5f;
    public float enemiesToSpawn = 1f;

    public float CurrentRound { get; private set; } = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple RoundController instances detected! Destroying duplicate. THERE CAN ONLY BE ONE!");
            Destroy(gameObject);
        }
    }

    public void LoseRound()
    {
        Debug.Log("Round Lost!");
    }

    public void WinRound()
    {
        Debug.Log("Round Won!");
    }

    public void StartNextRound()
    {
        Debug.Log("Begin next round!");
    }
}
