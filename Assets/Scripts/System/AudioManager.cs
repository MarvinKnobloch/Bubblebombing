using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Space]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterVolume;
    [SerializeField] private string musicVolume;
    [SerializeField] private string soundVolume;

    public enum MusicSongs
    {
        Empty,
        Menu,
        Level,
    }
    public enum UtilitySounds
    {
        Empty,
        MenuSelect,
        CurrencyGain,
    }
    public enum PlayerSounds
    {
        Empty,
        PlayerDeath,
    }
    public enum EnemySounds
    {
        Empty
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("AudioHasBeenChange") == 0)
        {
            PlayerPrefs.SetFloat("SliderValue" + masterVolume, 0.8f);

            //PlayerPrefs.SetFloat(masterVolume, Mathf.Log10(PlayerPrefs.GetFloat("SliderValue" + masterVolume) * 20));
            //SetVolume(masterVolume, 0);

            PlayerPrefs.SetFloat("SliderValue" + musicVolume, 0.8f);
            //PlayerPrefs.SetFloat(musicVolume, Mathf.Log10(PlayerPrefs.GetFloat("SliderValue" + musicVolume) * 20));
            //SetVolume(musicVolume, 0);

            PlayerPrefs.SetFloat("SliderValue" + soundVolume, 2.4f);
            //PlayerPrefs.SetFloat(soundVolume, Mathf.Log10(PlayerPrefs.GetFloat("SliderValue" + soundVolume) * 20));
            //SetVolume(soundVolume, 0);
        }

        SetDecibel(PlayerPrefs.GetFloat("SliderValue" + masterVolume), masterVolume, 0);
        SetDecibel(PlayerPrefs.GetFloat("SliderValue" + musicVolume), musicVolume, 0);
        SetDecibel(PlayerPrefs.GetFloat("SliderValue" + soundVolume), soundVolume, 10);

    }
    //private void SetVolume(string volumename, float maxdb)
    //{
    //    audioMixer.SetFloat(volumename, PlayerPrefs.GetFloat(volumename));
    //    bool gotvalue = audioMixer.GetFloat(volumename, out float soundvalue);
    //    if (gotvalue == true)
    //    {
    //        if (soundvalue > maxdb)
    //        {
    //            audioMixer.SetFloat(volumename, maxdb);
    //        }
    //    }
    //}
    private void SetDecibel(float sliderValue, string audioString, int maxDecibel)
    {
        float decibel = Mathf.Log10(sliderValue) * 20;

        audioMixer.SetFloat(audioString, decibel);
        bool gotvalue = audioMixer.GetFloat(audioString, out float soundvalue);

        if (gotvalue == true)
        {
            if (soundvalue > maxDecibel)
            {
                Debug.Log(soundvalue);
                audioMixer.SetFloat(audioString, maxDecibel);
            }
        }
        PlayerPrefs.SetFloat(audioString, decibel);
    }
}
