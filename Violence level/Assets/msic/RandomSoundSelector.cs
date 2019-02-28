using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plays a random clip on when the script is created in an object.
public class RandomSoundSelector : MonoBehaviour
{
    // List of clips.
    public AudioClip[] audioClips;
    // Audio source that will play a clip.
    private AudioSource audioSource;

    private void Start()
    {
        // Retrieves the audio source.
        audioSource = GetComponent<AudioSource>();
        // Randomly assigns a clip to the audio source and plays it.
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();

    }
}
