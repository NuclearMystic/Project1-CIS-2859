using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject spawnPointOne;
    [SerializeField] private GameObject spawnPointTwo;
    [SerializeField] private GameObject gateOne;
    [SerializeField] private GameObject gateTwo;


    public float startCountdown = 5f;
    public float enemiesToSpawn = 1f;



    public void LoseRound()
    {

    }

    public void WinRound()
    {

    }
}
