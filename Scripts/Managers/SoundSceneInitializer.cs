using UnityEngine;

public class SoundSceneInitializer : MonoBehaviour
{
    private void Start()
    {
        AudioManager audioManager = AudioManager.Instance;
        if (audioManager != null)
        {
            audioManager.ReinitializeSliders();
        }
    }
}
