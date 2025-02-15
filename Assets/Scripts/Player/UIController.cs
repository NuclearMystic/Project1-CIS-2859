using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField]
    private GameObject pauseMenu;

    private Animator hitEffectAnimator;
    private static readonly int HitTrigger = Animator.StringToHash("Hit");

    public AudioSource hitSFX;
    public AudioSource recoverSFX;
    public AudioSource CountDownSFX;
    public AudioSource CountDownGoSFX;

    public GameObject roundWonImage;

    public Slider healthSlider;
    public TMP_Text scoreText;
    public TMP_Text roundText;
    public TMP_Text pointsScored;

    private float totalScore = 0f;

    public GameObject speedEffect;
    public GameObject jumpEffect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        hitEffectAnimator = GetComponent<Animator>();
        if (hitEffectAnimator == null)
        {
            Debug.LogError("UIController: No Animator found on this GameObject!");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            PauseGame();
        }
    }

    public void PlayerHitEffect()
    {
        if (hitEffectAnimator != null)
        {
            hitEffectAnimator.SetTrigger("PlayerHit");
        }
        else
        {
            Debug.LogWarning("UIController: Attempted to trigger hit effect, but Animator is missing!");
        }
    }

    public void PlayHitSFX()
    {
        hitSFX.Play();
    }

    public void PlayCountDownSFX()
    {
        CountDownSFX.Play();
    }

    public void PlayCountDownGoSFX()
    {
        CountDownGoSFX.Play();
    }

    public void PauseGame()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }else
        {
            pauseMenu.SetActive(true);
        }
    }

    public void UpdateRoundNumber(float number)
    {
        roundText.text = number.ToString();

    }

    public void UpdateScore(float score)
    {
        totalScore += score;
        ShowPointsScored(score);
        scoreText.text = totalScore.ToString();
    }

    public void ShowPointsScored(float points)
    {
        pointsScored.text = "+" + points.ToString();
        StartCoroutine(ShowPointCountdown());
    }

    IEnumerator ShowPointCountdown()
    {
        yield return new WaitForSeconds(1);
        pointsScored.text = "";
    }

    public void ShowRoundWon()
    {
        roundWonImage.SetActive(true);
    }

    public void HideRoundWon()
    {
        roundWonImage.SetActive(false);
    }
}
