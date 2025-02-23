/*
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private AudioSource AudioSource;
    [SerializeField]
    private TextMeshProUGUI ValueText;
    [SerializeField]
    private AudioMixMode MixMode;

    public void OnChanggeSlider(float Value)
    {
        ValueText.SetText($"{Value.ToString("N4")}");

        switch (MixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                AudioSource.Volume = Value;
                break;
            case AudioMixMode.LinearMixerMode:
                Mixer.SetFloat("Volume", (-89 + Value * 100));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
        }
    }



    public enum AudioMixMode
    {
        LinearAudioSliderVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}
*/