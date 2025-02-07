using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    private Animator hitEffectAnimator;
    private static readonly int HitTrigger = Animator.StringToHash("Hit");

    public AudioSource hitSFX;

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
}
