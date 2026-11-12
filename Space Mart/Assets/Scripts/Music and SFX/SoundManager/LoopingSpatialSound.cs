using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopingSpatialSound : MonoBehaviour
{
    [Header("Sound Library ID")]
    public string soundName; // Must match the Group ID in your Sound Library

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (SoundManager.Instance != null && !string.IsNullOrEmpty(soundName))
        {
            AudioClip clip = SoundManager.Instance.GetClip(soundName);

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.spatialBlend = 1f; // Forces full 3D spatial audio
                audioSource.Play();
            }
        }
    }
}