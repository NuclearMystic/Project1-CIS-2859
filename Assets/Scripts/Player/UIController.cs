using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField]
    private GameObject pauseMenu;

    private Animator hitEffectAnimator;
    private static readonly int HitTrigger = Animator.StringToHash("Hit");

    public AudioSource hitSFX;
    public AudioSource recoverSFX;

    public Slider healthSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

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
}
