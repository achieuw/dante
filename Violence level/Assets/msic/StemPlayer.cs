using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


// Base class for all stem players.
public class StemPlayer : MonoBehaviour
{
    // The class that controls the valus of the mixer group the stemplayer sends its audio to.
    protected MixerGroupVolumeControl mixerControl;

    // the maximum volume stems will play
    public float stemVolume;
    // Saves the original max volume in case it needs tobe restored.
    private float originalStemVolume;
    // The initial fade in time in seconds
    public float initialFade;
    // The fade time when stems are fading in or out each other.
    public float fadeTime;
    // In case the program wants to change the fading time, the original will be stored and can be reset.
    private float originalFadeTime;

    // The player will just do one fade a time when using some functions. This is at lock.
    public bool waitLock;
    // How many stems are playing
    protected int stemsPlaying;

    // The audio clips that will be connected to audio sources.
    public AudioClip[] audioClips;
    // The audio sources of the stem player.
    protected AudioSource[] audioPlayers;
    // The fader of the stem player.
    protected AudioFader fader;

    // If not all stems are initialized, no increase / decrease of playing stems will be allowed.
    // This doesn't mean that the player is not ready, but that the stems are not playing yet.
    // The player should begin to play with any of the Play function to initialize stems.
    protected bool allStemsInitialized;
    // Indicates that the fader and all stems are ready to be played.
    protected bool stemPlayerReady = false;

    protected virtual void Start()
    {
        fader = gameObject.AddComponent<AudioFader>() as AudioFader;
        if (audioClips.Length == 0 || fader == null)
        {
            Debug.Log("no audio clips or fader found");
        }
        // Saves the original fade time.
        originalFadeTime = fadeTime;
        // Clamps the volume between 0 and 1, in case it was wrongly input. 
        // Can be taken out later.
        stemVolume = Mathf.Clamp(stemVolume, 0.0f, 1.0f);
        originalStemVolume = stemVolume;
        // The volume control of this stem's mixer group.
        mixerControl = GetComponent<MixerGroupVolumeControl>();

        // Creates new audio sources and assigns audio clips to them.
        audioPlayers = new AudioSource[audioClips.Length];
        for (int i = 0; i < audioPlayers.Length; i++)
        {
            audioPlayers[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
            audioPlayers[i].clip = audioClips[i];
        }

        // Load the default values
        ResetValues();
        // Stem player is ready to be used
        stemPlayerReady = true;
    }

    // Resets the stem player values to default.
    protected virtual void ResetValues()
    {
        allStemsInitialized = false;
        fadeTime = originalFadeTime;
        waitLock = false;
        // Layered stems will loop. Stems will not play on awake, but when some of the
        // Play() functions are called.
        foreach (AudioSource audio in audioPlayers)
        {
            audio.playOnAwake = false;
            audio.volume = 0.0f;
            audio.loop = true;
            audio.priority = 0;
            audio.outputAudioMixerGroup = mixerControl.mixerGroup;
        }
        stemsPlaying = 0;
        stemVolume = originalStemVolume;
    }

    // If the stem player is ready to be used or not.
    public bool StemPlayerReady()
    {
        return stemPlayerReady;
    }

    // All ongoing fades are stopped.
    public void StopAllOngoingCoroutines()
    {
        StopAllCoroutines();
        fader.StopAllFades();
    }

    // Begin to play with the stem numbered stemNr.
    public void Play(int stemNr, float fadeT)
    {
        // If the stems are already initialized or fading, nothing is done;
        if (allStemsInitialized || waitLock)
            return;
        else
            StartCoroutine(Play_(Mathf.Clamp(stemNr, 0, audioPlayers.Length - 1), fadeT));
    }

    // Play starts the player. If no fade is wanted, this function should be used
    // passing 0.0f as parameter, not the function below.
    public void Play(float fadeT)
    {
        Play(0, fadeT);
    }
    // If Play is called without parameters, the built-in initial fade is used.
    // For a non-fade Play, Play(0.0f) should be used.
    public void Play(int stemNr = 0)
    {
        Play(stemNr, initialFade);
    }
    // The actual implementation of the play
    private IEnumerator Play_(int stemNr, float fadeT)
    {
        // Locks the player.
        waitLock = true;
        // Starts all stems.
        foreach (AudioSource audio in audioPlayers)
            audio.Play();
        // Fades in the selected stem and wait for its fade time.
        fader.FadeIn(audioPlayers[stemNr], fadeT, stemVolume);
        yield return new WaitForSeconds(fadeT);
        stemsPlaying = 1;
        // Stems are initialized.
        allStemsInitialized = true;
        waitLock = false;
    }

    // Stops all ongoing fades and then stops all stems, with a fade out of fadeT seconds.
    // Different kinds of stem players may need to handle the fade out differently.
    public void Stop(float fadeT)
    {
        StopAllOngoingCoroutines();
        StartCoroutine(Stop_(fadeT));
    }

    private IEnumerator Stop_(float fadeT)
    {
        // Locks any fade attempt while execution this.
        waitLock = true;
        // All stems fades out and stop.
        foreach (AudioSource audio in audioPlayers)
            fader.FadeOutAndStop(audio, fadeT);
        // Waits for the fade time to pass.
        yield return new WaitForSeconds(fadeT);
        // Resets all values, which means the stems are not playing.
        ResetValues();
    }

    // Fades in a stem with specified fade time
    public void FadeInStem(int stemNr, float fadeT)
    {
        if (waitLock)
            return;
        stemNr = Mathf.Clamp(stemNr, 0, audioPlayers.Length - 1);
        StartCoroutine(FadeInStem_(stemNr, fadeT));
    }

    // Fades in a stem with the default fade time
    public void FadeInStem(int stemNr)
    {
        FadeInStem(stemNr, fadeTime);
    }

    private IEnumerator FadeInStem_(int stemNr, float fadeT)
    {
        waitLock = true;
        if(audioPlayers[stemNr].volume == 0.0f)
        {
            // Fade in the stem.
            fader.FadeIn(audioPlayers[stemNr], fadeT, stemVolume);
            // Waits for its fade in time to pass.
            yield return new WaitForSeconds(fadeTime);
            stemsPlaying++;
        }
        waitLock = false;
    }

    // Fades out a stem with specified fade time
    public void FadeOutStem(int stemNr, float fadeT)
    {
        if (waitLock)
            return;
        stemNr = Mathf.Clamp(stemNr, 0, audioPlayers.Length - 1);
        StartCoroutine(FadeOutStem_(stemNr, fadeT));
    }

    // Fades out a stem with the default fade time
    public void FadeOutStem(int stemNr)
    {
        FadeOutStem(stemNr, fadeTime);
    }

    private IEnumerator FadeOutStem_(int stemNr, float fadeT)
    {
        waitLock = true;
        if (audioPlayers[stemNr].volume > 0.0f)
        {
            // Fade out the stem.
            fader.FadeOut(audioPlayers[stemNr], fadeT, 0.0f);
            // Waits for its fade in time to pass.
            yield return new WaitForSeconds(fadeTime);
            stemsPlaying--;
        }
        waitLock = false;
    }


    //Sets a new fade time for the stems.
    public void SetFadeTime(float newFadeTime)
    {
        StartCoroutine(ChangeFadeTime_(newFadeTime));
    }
    // Resets the fade time to the original one.
    public void ResetFadeTime()
    {
        StartCoroutine(ChangeFadeTime_(originalFadeTime));
    }
    // Implementation of the changing time, with fade lock.
    IEnumerator ChangeFadeTime_(float fadeT)
    {
        // The changing of the fading time should wait until all fadings are done.
        while (waitLock)
            yield return null;
        fadeTime = fadeT;
    }

    // Volume, FadeIn and FadeOut function below will change the stem player's mixer group's volume, not individual stems,
    // so all the music from the stem player will be affected.
    public void Volume(float volume)
    {
        mixerControl.Volume(volume);
    }

    public void FadeIn(float fadeT, float volume = 0.0f)
    {
        mixerControl.FadeIn(fader, fadeT, volume);
    }

    public void FadeOut(float fadeT, float volume = MixerGroupVolumeControl.minMixerVolume)
    {
        mixerControl.FadeOut(fader, fadeT, volume);
    }

    // Returns a reference to the volume control of this mixer group.
    public MixerGroupVolumeControl GetMixerControl()
    {
        return mixerControl;
    }

    // Stops all ongoing fades and all stems.
    public virtual void Stop()
    {
        StopAllOngoingCoroutines();
        foreach (AudioSource audio in audioPlayers)
            audio.Stop();
        ResetValues();
    }

    // The fade time between stems
    public virtual float FadeTime()
    {
        return fadeTime;
    }

    public float StemLength()
    {
        return audioClips[0].length;
    }

    private void SetStemsVolume(float newVolume)
    {
        StartCoroutine(SetStemsVolume_(newVolume));
    }
     // Change the max volume of the stems.
    private IEnumerator SetStemsVolume_(float newVolume)
    {
        while (waitLock)
            yield return null;

        // Is the lock needed?
        //waitLock = true;
        stemVolume = newVolume;
        for (int i = 0; i < audioPlayers.Length; i++)
        {
            if (audioPlayers[i].volume > 0.0f)
                audioPlayers[i].volume = newVolume;
        }
        //waitLock = false;
    }

}
