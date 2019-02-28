using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// A fader for audio sources and mixer exposed volume parameters.
public class AudioFader : MonoBehaviour
{
    // Those are the lowest and highest volumes in the mixer in Unity.
    // Made const to avoid "magic numbers" around the code (there are already some...).
    public const float lowestMixerVolume = -80.0f;
    public const float highestMixerVolume = 20.0f;

    // Starts an audio source from 0.0f and fades in to the volume
    public void StartAndFadeIn(AudioSource audio, float fadeTime, float volume = 1.0f)
    {
        if (audio.isPlaying)
            return;
        audio.volume = 0.0f;
        audio.Play();
        StartCoroutine(Fade_(audio, fadeTime, volume));
    }
    // Stops the audio source after fading out. Assumes that the fade will be total, meaning the volume will get to 0.0f
    public void FadeOutAndStop(AudioSource audio, float fadeTime)
    {
        StartCoroutine(FadeOutAndStop_(audio, fadeTime));
    }
    private IEnumerator FadeOutAndStop_(AudioSource audio, float fadeTime)
    {
        yield return Fade_(audio, fadeTime, 0.0f);
        audio.Stop();
    }

    // Fades in a clip to a given volume during fadeTime seconds.
    public void FadeIn(AudioSource audio, float fadeTime, float volume = 1.0f)
    {
        StartCoroutine(Fade_(audio, fadeTime, volume));
    }
    // Fades out a clip to a given volume during fadeTime seconds. 
    // If no veolume is given, it's assumed the clip will fade to 0
    public void FadeOut(AudioSource audio, float fadeTime, float volume = 0.0f)
    {
        StartCoroutine(Fade_(audio, fadeTime, volume));
    }

    private IEnumerator Fade_(AudioSource audio, float fadeTime, float volume)
    {
        float time = 0.0f;
        // Calculates how much the volume will change. Will be used in all fading functions.
        float deltaVolume = volume - audio.volume;

        while (time < fadeTime)
        {
            audio.volume += deltaVolume * Time.deltaTime / fadeTime;
            time += Time.deltaTime;
            yield return null;
        }
        // Because of imprecisions in Time.deltaTime, the given volume will be set at the end to be sure it's
        // the desired one. This will be used in all fading functions.
        audio.volume = volume;
        yield return null;
    }

    // Fades in a mixer instead of an audio source. The same principle as above.
    public Coroutine FadeIn(AudioMixer audio, string parameter, float fadeTime, float volume = 0.0f)
    {
        return StartCoroutine(Fade_(audio, parameter, fadeTime, volume));
    }
    // Fades out a mixer instead of an audio source. Same principle as above, just using the GetFloat and SetFloat
    // to deal with the way the mixer works in unity.
    public Coroutine FadeOut(AudioMixer audio, string parameter, float fadeTime, float volume = lowestMixerVolume)
    {
        return StartCoroutine(Fade_(audio, parameter, fadeTime, volume));
    }

    private IEnumerator Fade_(AudioMixer audio, string parameter, float fadeTime, float volume)
    {
        float time = 0.0f;
        float currentVolume;
        // mixers work differently than audiosources. To get a value, a string name of an exposed mixer parameter 
        // needs to be sent as the first parameter in the GetFloat function, and an 'out' variable as second. 
        // That second variable will be modified with the mixer's value for the parameter.
        audio.GetFloat(parameter, out currentVolume);
        float deltaVolume = volume - currentVolume;

        while (time < fadeTime)
        {
            currentVolume += deltaVolume * Time.deltaTime / fadeTime;
            // To set a value to the mixer, SetFloat needs to be used with the first parameter being the name of the mixer's
            // exposed parameter, and the second the value that will be set in the first one.
            audio.SetFloat(parameter, currentVolume);
            time += Time.deltaTime;
            yield return null;
        } 
        audio.SetFloat(parameter, volume);
        yield return null;
    }

    // Stop all ongoing fades.
    public void StopAllFades()
    {
        StopAllCoroutines();
    }

    // Those two function below are redundant, since it's easy to do the thing directly with audio source
    // and mixer, maybe they will be removed.

    // Sets the volume of an audioclip to the desired volume at once.
    public void SetVolume(AudioSource audio, float volume)
    {
        audio.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
    }

    // Same thing with the mixer.
    public void SetVolume(AudioMixer audio, string parameter, float volume)
    {
        audio.SetFloat(parameter, Mathf.Clamp(volume, lowestMixerVolume, highestMixerVolume));
    }
}
