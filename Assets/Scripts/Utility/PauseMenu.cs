using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public AudioSource[] audioSources;

    private void OnEnable()
    {
        if (audioSources != null)
        {
            foreach (var source in audioSources)
            {
                source.Pause();
            }
            Time.timeScale = 0f;
        }
    }

    private void OnDisable()
    {
        if (audioSources != null)
        {
            foreach (var source in audioSources)
            {
                source.UnPause();
            }
            Time.timeScale = 1f;
        }
    }
}
