using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Special sounds should play via this player to avoid multiple special sounds
// playing at the same time.
public class SpecialSoundsPlayer : MonoBehaviour {

    // The audio source that will play the clips.
    private AudioSource audioSource;
    // a lock to avoid multiple special sounds to play at the same time.
    bool isPlaying;

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        // At the beginning, nothing is playing.
        isPlaying = false;
	}

    // If there is nothing playing, calls the subroutine that will play the clip.
    public void PlayClip(AudioClip clip, float volume = 0.0f)
    {
        if (isPlaying)
            return;
        else
            StartCoroutine(PlayClip_(clip,volume));
    }
    // Assumes nothing is playing.
    private IEnumerator PlayClip_(AudioClip clip, float volume)
    {
        // Locks the player, so no other sound will be played.
        isPlaying = true;
        // If the volune is 0, assumes no volume was specified and uses the audio source's volume, and plays the clip.
        if (volume == 0.0f)
            volume = audioSource.volume;
        audioSource.PlayOneShot(clip, volume);
        // Waits for a slight less time than the clips lenght.
        yield return new WaitForSeconds(clip.length - 0.1f);

        // Releases the sound player to be used again.
        isPlaying = false;
    }
}
