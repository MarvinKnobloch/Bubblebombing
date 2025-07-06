using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Audioslider : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string volumeString;
    private Slider slider;

    [SerializeField] private TextMeshProUGUI sliderText;

    private bool skipFirstSound;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        skipFirstSound = true;
    }
    private void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat("SliderValue" + volumeString);
        sliderText.text = Mathf.Round(slider.normalizedValue * 100).ToString();
    }
    public void MasterValuechange(float slidervalue)
    {
        sliderText.text = Mathf.Round(slider.normalizedValue * 100).ToString();

        SetDecibel(slidervalue, volumeString, 0);

        if (skipFirstSound == true)
        {
            skipFirstSound = false;
            return;
        }
        //else AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
    }
    public void MusicValueChange(float slidervalue)
    {
        sliderText.text = Mathf.Round(slider.normalizedValue * 100).ToString();

        SetDecibel(slidervalue, volumeString, 0);
    }
    public void SoundEffectValueChange(float slidervalue)
    {
        sliderText.text = Mathf.Round(slider.normalizedValue * 100).ToString();

        SetDecibel(slidervalue, volumeString, 10);

        if(skipFirstSound == true)
        {
            skipFirstSound = false;
            return;
        }
        //else AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
    }

    private void SetDecibel(float value, string audioString, int maxDecibel)
    {
        PlayerPrefs.SetInt("AudioHasBeenChange", 1);
        PlayerPrefs.SetFloat("SliderValue" + audioString, value);

        float decibel = value / 100f * 80f - 80;

        audioMixer.SetFloat(audioString, Mathf.Clamp(decibel, -80, 0));
    }
}
