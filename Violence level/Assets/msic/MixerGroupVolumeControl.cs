using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerGroupVolumeControl : MonoBehaviour
{
    // Audio mixer and mixer group, as well the parameter that will be used to control their volume.
    public AudioMixerGroup mixerGroup;
    public AudioMixer mixer;
    public string mixerParameter;
    // The increase / decrease volume steps.
    public float volumeSteps;
    // The minimum value of the mixer, in dB. 
    public const float minMixerVolume = -80.0f;
    // Used to toggle the mixer on and off;
    private int toggleVolume;
    // The volume the mixer is using now.
    private float toggleVolumeSaved;

    private Coroutine fadeFunction;

    void Start ()
    {
        toggleVolume = 0;
	}

    // Resets the mixer group to its default volume.
    public void DefaultVolume()
    {
        // ClearFloat reverts the changes in mixerParameter to its default.
        mixer.ClearFloat(mixerParameter);
    }

    // Changes the volume of the mixer group.
    public void Volume(float volume)
    {
        mixer.SetFloat(mixerParameter, volume);
    }

    // Fades in using the AudioFader object in deltaTime seconds. If no volume is specified, it's assumed 
    // it will fade to 0 dB;
    public void FadeIn(AudioFader fader, float deltaTime, float volume = 0.0f)
    {
        fadeFunction = fader.FadeIn(mixer, mixerParameter, deltaTime, volume);
    }

    // Fades out using the AudioFader object in deltaTime seconds. If no values is specified, it will use the 
    // lowest mixer volume value.
    public void FadeOut(AudioFader fader, float deltaTime, float volume = minMixerVolume)
    {
        fadeFunction = fader.FadeOut(mixer, mixerParameter, deltaTime, volume);
    }

    // gets the mixer volume.
    public float GetVolume()
    {
        float value;
        mixer.GetFloat(mixerParameter, out value);
        return value;
    }

    // Toggle the volume on and off.
    public void ToggleVolumeOnOff()
    {
        if(toggleVolume == 0)
        {
            toggleVolumeSaved = GetVolume();
            mixer.SetFloat(mixerParameter, -80.0f);
        }
        else
            mixer.SetFloat(mixerParameter, toggleVolumeSaved);
        // Toggle will alternate between 0 and 1 everytime this funciton is called.
        toggleVolume = 1 - toggleVolume;

    }

    // Increases the volume by the volumeSteps dBs.
    public void IncreaseVolume()
    {
        float volume = GetVolume();
        volume += volumeSteps;
        Volume(volume);
    }

    // Decreases the volume by the volumeSteps dBs.
    public void DecreaseVolume()
    {
        float volume = GetVolume();
        volume -= volumeSteps;
        Volume(volume);
    }
}
