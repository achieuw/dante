using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Simple class to handle situations with a min and max value, instead of having 2 separated floats.
[System.Serializable]
public class MinMax
{
    public float min;
    public float max;
}

// Simple class to gather an exposed mixer parameter name and the value that will be assigned to it.
[System.Serializable]
public class MixerAccesser
{
    public string parameterName;
    public float parameterValue;
}

public enum GameMixerGroups { MainTheme = 0, AudioFX, SpecialSounds, MainLoop, NumberOfChoices }

// The MusicController class will manage the music.
// It is specific for this game, unlike the stemPlayers, that are meant to be more general.
public class MusicController : MonoBehaviour
{
    // The mixer and the different mixer groups for the sounds in the game.
    public AudioMixer mixer;
    public MixerGroupVolumeControl[] mixerControl;
    // The stemPlayers that contains the different main loop music.
    public StemPlayer[] stemPlayers;
    // Of the stemPlayers above, one will be chosen to play the main loop music.
    private StemPlayer mainLoopMusic;

    // The main theme music, that will be played at the beginning and in the game over / restart sections.
    public AudioSource mainTheme;
    public float mainThemeVolume;
    // Time to crossfade between the mainLoopMusic and mainTheme.
    public float crossfadeTime;
    // when fading from main loop music to main theme, some random reentry point will be chosen.
    public float[] mainThemeReentryPoints;

    // lock to avoid multiple calls to crossfading before they are done.
    // If a crossfade is called while another is being done, it will wait and then begin to crossfade.
    private bool isFading;

    // The game should wait until the music controller is ready before using it.
    private bool musicControllerReady = false;

    // This fader is used just for the main theme at the moment.
    public AudioFader fader { get; private set; }

    // Initializes the MusicController.
    void Start()
    {
        // Creates a fader.
        fader = gameObject.AddComponent<AudioFader>() as AudioFader;
        isFading = false;

        // This coroutine will be sure all players are initialized before making the MusicController ready.
        StartCoroutine(InitializePlayers_());        
    }

    // The MusicController will not be ready until all stemPlayers are ready. Any other future initialization should be put here as well.
    // Any component that wishes to use the music controller should check if it's ready before the first use.
    IEnumerator InitializePlayers_()
    {
        for (int i = 0; i < stemPlayers.Length; i++)
            while (!stemPlayers[i].StemPlayerReady())
                yield return null;
        musicControllerReady = true;
        mainLoopMusic = stemPlayers[0];

        mainLoopMusic.Play();

        StartCoroutine(MainLoop_());
    }

    private IEnumerator MainLoop_()
    {
        while (!musicControllerReady)
            yield return null;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                mainLoopMusic.FadeInStem(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                mainLoopMusic.FadeInStem(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                mainLoopMusic.FadeInStem(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                mainLoopMusic.FadeInStem(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                mainLoopMusic.FadeInStem(4);

            else if (Input.GetKeyDown(KeyCode.Alpha6))
                mainLoopMusic.FadeOutStem(0);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                mainLoopMusic.FadeOutStem(1);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                mainLoopMusic.FadeOutStem(2);
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                mainLoopMusic.FadeOutStem(3);
            else if (Input.GetKeyDown(KeyCode.Alpha0))
                mainLoopMusic.FadeOutStem(4);

            else if (Input.GetKeyDown(KeyCode.Q))
                mainLoopMusic.FadeIn(5);
            else if (Input.GetKeyDown(KeyCode.W))
                mainLoopMusic.FadeOut(5);



            yield return null;
        }
    }

    // Chooses the main loop music based on the player's choice.
    // If the player hasn't chosen any, it will be based on difficulty.
    private IEnumerator ChooseMainLoopMusic()
    {
        bool choosing = true;

        while (choosing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                mainLoopMusic = stemPlayers[0];
                choosing = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                mainLoopMusic = stemPlayers[1];
                choosing = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                mainLoopMusic = stemPlayers[2];
                choosing = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                mainLoopMusic = stemPlayers[3];
                choosing = false;
            }
            yield return null;
        }
    }

    // Starts the main loop music. All ongoing coroutines and invokes will be cancelled.
    public void PlayMainLoopMusic()
    {
        CancelInvoke();
        StopAllCoroutines();
        mainLoopMusic.Play();
    }


    // Stops the main loop music
    // All ongoing coroutines and invokes will be cancelled.
    public void StopMainLoopMusic(float fadeTime = 0.0f)
    {
        CancelInvoke();
        StopAllCoroutines();
        mainLoopMusic.Stop(fadeTime);
    }

    // Stops everything that is playing or fading. Kind of  "S**t hit the fan" button.
    public void PanicButton()
    {
        CancelInvoke();
        StopAllCoroutines();
        mainLoopMusic.Stop();
        mainTheme.Stop();
    }

    // Plays the main theme music, with the possibility of a fade in. 
    // All ongoing coroutines and invokes will be cancelled.
    public void PlayMainTheme(float fadeTime = 0.0f)
    {
        CancelInvoke();
        StopAllCoroutines();
        // If there is a main loop music playing, it needs to be stopped.
        mainLoopMusic.Stop(0.1f);
        // Resets the main theme to begin at the start point, in case it was changed when crossfading.
        mainTheme.time = 0.0f;
        fader.StartAndFadeIn(mainTheme, fadeTime, mainThemeVolume);
    }

    // Stops the main theme, with the possibility of a fade out.
    public void StopMainTheme(float fadeTime = 0.0f)
    {
        fader.FadeOutAndStop(mainTheme, fadeTime);
    }


    // Will stop the main loop and start the main theme music. This implementation is done with a crossfade, 
    // but can implemented in another way. Returns the time it will take to crossfade them.
    public float MainLoopToMainTheme()
    {
        if (mainThemeReentryPoints.Length > 0)
            mainTheme.time = mainThemeReentryPoints[Random.Range(0, mainThemeReentryPoints.Length)];
        CrossFade(mainTheme, mainLoopMusic, crossfadeTime);
        return crossfadeTime;
    }


    // Toggles the volum on and off for different mixer groups.
    public void ToggleVolume(GameMixerGroups group)
    {
        switch (group)
        {
            case GameMixerGroups.AudioFX:
                mixerControl[(int)GameMixerGroups.AudioFX].ToggleVolumeOnOff();
                break;
            case GameMixerGroups.MainTheme:
                mixerControl[(int)GameMixerGroups.MainTheme].ToggleVolumeOnOff();
                break;
            case GameMixerGroups.SpecialSounds:
                mixerControl[(int)GameMixerGroups.SpecialSounds].ToggleVolumeOnOff();
                break;
            case GameMixerGroups.MainLoop:
                mainLoopMusic.GetMixerControl().ToggleVolumeOnOff();
                break;
        }
    }

    // Increases the volume for different mixer groups.
    public void IncreaseVolume(GameMixerGroups group)
    {
        switch (group)
        {
            case GameMixerGroups.AudioFX:
                mixerControl[(int)GameMixerGroups.AudioFX].IncreaseVolume();
                break;
            case GameMixerGroups.MainTheme:
                mixerControl[(int)GameMixerGroups.MainTheme].IncreaseVolume();
                break;
            case GameMixerGroups.SpecialSounds:
                mixerControl[(int)GameMixerGroups.SpecialSounds].IncreaseVolume();
                break;
            case GameMixerGroups.MainLoop:
                mainLoopMusic.GetMixerControl().IncreaseVolume();
                break;
        }
    }
    // Decreases the volume for different mixer groups.
    public void DecreaseVolume(GameMixerGroups group)
    {
        switch (group)
        {
            case GameMixerGroups.AudioFX:
                mixerControl[(int)GameMixerGroups.AudioFX].DecreaseVolume();
                break;
            case GameMixerGroups.MainTheme:
                mixerControl[(int)GameMixerGroups.MainTheme].DecreaseVolume();
                break;
            case GameMixerGroups.SpecialSounds:
                mixerControl[(int)GameMixerGroups.SpecialSounds].DecreaseVolume();
                break;
            case GameMixerGroups.MainLoop:
                mainLoopMusic.GetMixerControl().DecreaseVolume();
                break;
        }
    }
    // Sets the default volume for different mixer groups
    public void DefaultVolume(GameMixerGroups group)
    {
        switch (group)
        {
            case GameMixerGroups.AudioFX:
                mixerControl[(int)GameMixerGroups.AudioFX].DefaultVolume();
                break;
            case GameMixerGroups.MainTheme:
                mixerControl[(int)GameMixerGroups.MainTheme].DefaultVolume();
                break;
            case GameMixerGroups.SpecialSounds:
                mixerControl[(int)GameMixerGroups.SpecialSounds].DefaultVolume();
                break;
            case GameMixerGroups.MainLoop:
                mainLoopMusic.GetMixerControl().DefaultVolume();
                break;
        }
    }
    // Cross fades an AudioSource in with a StemPlayer out in xfadeTime seconds.
    // Cancels all invokes and stops all coroutines.
    public void CrossFade(AudioSource audioIn, StemPlayer stemsOut, float xfadeTime)
    {
        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(CrossFade_(audioIn, stemsOut, xfadeTime));
    }
    IEnumerator CrossFade_(AudioSource audioIn, StemPlayer stemsOut, float xfadeTime)
    {
        // Waits until nothing is fading.
        while (isFading)
            yield return null;
        isFading = true;
        // Fades out the stem player that is supposed to stop playing.
        stemsOut.Stop(xfadeTime);
        // Fades in the audio source that is supposed to start playing.
        fader.StartAndFadeIn(audioIn, xfadeTime, mainThemeVolume);
        // Waits for the fade.
        yield return new WaitForSeconds(xfadeTime);
        isFading = false;
    }

    // Cross fades a StemPlayer in with as AudioSource out in xfadeTime seconds.
    public void CrossFade(StemPlayer stemsIn, AudioSource audioOut, float xfadeTime)
    {
        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(CrossFade_(stemsIn, audioOut, xfadeTime));
    }
    IEnumerator CrossFade_(StemPlayer stemsIn, AudioSource audioOut, float xfadeTime)
    {   
        // Same principle as the former funtions.
        while (isFading)
            yield return null;
        isFading = true;
        stemsIn.Play(xfadeTime);
        fader.FadeOutAndStop(audioOut, xfadeTime);

        yield return new WaitForSeconds(xfadeTime);
        isFading = false;
    }
}
