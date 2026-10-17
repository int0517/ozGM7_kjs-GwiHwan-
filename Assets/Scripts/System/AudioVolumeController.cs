
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
        // 저장된 볼륨 불러오기 (기본값 1)
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);

        // 슬라이더 범위 설정
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        // 저장된 값으로 슬라이더 초기화
        volumeSlider.SetValueWithoutNotify(savedVolume);

        // 저장된 볼륨 적용
        SetVolume(savedVolume);

        // 슬라이더 변경 이벤트 등록
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // 선형 볼륨 값을 데시벨로 변환
        float decibels = volume <= 0.0001f
            ? -80f
            : Mathf.Log10(volume) * 20f;

        // AudioMixer 볼륨 변경
        audioMixer.SetFloat(VolumeParameter, decibels);

        // 볼륨 설정 저장
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
