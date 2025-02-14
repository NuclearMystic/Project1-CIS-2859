using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundController : MonoBehaviour
{
    public static RoundController Instance { get; private set; }

    [Header("Game Objects")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPointOne;
    [SerializeField] private Transform spawnPointTwo;
    [SerializeField] private Animator gateOneAnimator;
    [SerializeField] private Animator gateTwoAnimator;
    [SerializeField] private Animator countdownAnimator;

    [Header("Player References")]
    [SerializeField] private Transform player;
    [SerializeField] private CameraLook cameraLookScript;
    [SerializeField] private PlayerMovement playerMovementScript;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverScreen; 

    public float countdownTime = 3f;
    public int enemiesPerSide = 1;

    public int CurrentRound { get; private set; } = 1;

    private int activeEnemies = 0;
    private bool roundInProgress = false;
    private Transform arenaCenter;

    public AudioSource gameOverSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple RoundController instances detected! Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameObject centerObj = GameObject.FindGameObjectWithTag("Center");
        if (centerObj != null)
        {
            arenaCenter = centerObj.transform;
        }
        else
        {
            Debug.LogError("No object tagged as 'Center' found in the scene!");
        }

        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        if (roundInProgress) yield break;
        roundInProgress = true;
        enemiesPerSide = CurrentRound;
        activeEnemies = enemiesPerSide * 2;

        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateRoundNumber(CurrentRound);
        }
        else
        {
            Debug.LogWarning("UIController instance not found!");
        }

        Debug.Log($"Starting Round {CurrentRound} - Total Enemies: {activeEnemies}");

        if (player != null && arenaCenter != null)
        {
            player.position = arenaCenter.position;
            DisablePlayerControls();
        }

        countdownAnimator.SetTrigger("StartCountdown");
        yield return new WaitForSeconds(countdownTime);

        if (UIController.Instance != null)
        {
            UIController.Instance.HideRoundWon();
        }

        Debug.Log("Go!");
        EnablePlayerControls();

        gateOneAnimator.SetTrigger("Open");
        gateTwoAnimator.SetTrigger("Open");
        yield return new WaitForSeconds(1f);

        SpawnEnemies(spawnPointOne);
        SpawnEnemies(spawnPointTwo);

        yield return new WaitForSeconds(1f);

        gateOneAnimator.SetTrigger("Close");
        gateTwoAnimator.SetTrigger("Close");
    }

    private void DisablePlayerControls()
    {
        if (cameraLookScript != null) cameraLookScript.enabled = false;
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        Debug.Log("Player controls disabled.");
    }

    private void EnablePlayerControls()
    {
        if (cameraLookScript != null) cameraLookScript.enabled = true;
        if (playerMovementScript != null) playerMovementScript.enabled = true;
        Debug.Log("Player controls enabled.");
    }

    private void SpawnEnemies(Transform spawnPoint)
    {
        for (int i = 0; i < enemiesPerSide; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyController enemyScript = enemy.GetComponent<EnemyController>();

            if (enemyScript != null)
            {
                enemyScript.OnDeath += EnemyDefeated;
            }
            else
            {
                Debug.LogError("Spawned enemy is missing the EnemyController script!");
            }
        }
    }

    private void EnemyDefeated()
    {
        activeEnemies--;

        Debug.Log($"Enemy defeated! Remaining: {activeEnemies}");

        if (activeEnemies <= 0)
        {
            WinRound();
        }
    }

    public void WinRound()
    {
        UIController.Instance.UpdateScore(1000);
        UIController.Instance.ShowRoundWon();
        FindObjectOfType<PlayerHealth>().RestoreHealth(10);
        Debug.Log($"Round {CurrentRound} Won!");
        roundInProgress = false;
        CurrentRound++;
        StartCoroutine(StartRound());
    }

    public void LoseRound()
    {
        roundInProgress = false;

        if (gameOverScreen != null)
        {
            gameOverSFX.Play();
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Game Over screen is not assigned in the Inspector!");
        }

        StartCoroutine(WaitAndLoadLeaderboard());
    }

    private IEnumerator WaitAndLoadLeaderboard()
    {
        yield return new WaitForSecondsRealtime(5f); 
        SceneManager.LoadScene("Leaderboard"); 
    }
}
