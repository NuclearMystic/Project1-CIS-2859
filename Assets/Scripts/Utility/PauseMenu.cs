using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public AudioSource[] audioSources;

    private void OnEnable()
    {
        foreach (var source in audioSources)
        {
            source.Pause();
        }
        Time.timeScale = 0f; 
        Debug.Log("Game Paused");
    }

    private void OnDisable()
    {
        foreach (var source in audioSources)
        {
            source.UnPause();
        }
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
}
