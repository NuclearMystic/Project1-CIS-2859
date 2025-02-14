using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public Slider healthSlider;
    public TMP_Text scoreText;
    public TMP_Text roundText;

    private float totalScore = 0f;

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
        scoreText.text = totalScore.ToString();
    }
}
