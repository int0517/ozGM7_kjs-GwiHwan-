using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string VolumeKey = "MasterVolume";
    private const string VolumeParameter = "MasterVolume";

    private void Start()
    {
        // 인스펙터에서 이미 연결된 경우에도 초기화
        if (volumeSlider != null)
        {
            InitializeSlider();
        }
    }

    public void Bind(Slider slider)
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        }

        volumeSlider = slider;

        if (volumeSlider != null)
        {
            InitializeSlider();
        }
        else
        {
            Debug.LogWarning("AudioVolumeController: 슬라이더 참조가 없습니다.");
        }
    }

    private void InitializeSlider()
    {
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);

        volumeSlider.SetValueWithoutNotify(savedVolume);
        SetVolume(savedVolume);

        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        float decibels = volume <= 0.0001f
            ? -80f
            : Mathf.Log10(volume) * 20f;

        if (audioMixer != null)
        {
            audioMixer.SetFloat(VolumeParameter, decibels);
        }

        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }
}