
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<AudioManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }

            return instance;
        }
    }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] playerAudio;
    [SerializeField] private AudioClip[] ghostAudio;
    [SerializeField] private AudioClip[] systemAudio;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Audio")]
    [SerializeField] private AudioClip whiteNoiseClip;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource 자동 생성 및 연결
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        // 배경음 설정
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        // 효과음 설정
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        PlayBGM(whiteNoiseClip, 0.001f);
    }

    // 배경음 재생
    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.volume = Mathf.Clamp01(volume);
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // 배경음 정지
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // 배경음 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    // 플레이어 효과음
    public void PlayPlayerAudio(int index, float volume = 1f)
    {
        PlaySFX(playerAudio, index, volume);
    }

    // 귀신 효과음
    public void PlayGhostAudio(int index, float volume = 1f)
    {
        PlaySFX(ghostAudio, index, volume);
    }

    // 시스템 효과음
    public void PlaySystemAudio(int index, float volume = 1f)
    {
        PlaySFX(systemAudio, index, volume);
    }

    // 공통 효과음 재생
    private void PlaySFX(AudioClip[] clips, int index, float volume)
    {
        if (clips == null ||
            index < 0 ||
            index >= clips.Length ||
            clips[index] == null)
        {
            Debug.LogWarning($"잘못된 오디오 인덱스: {index}");
            return;
        }

        sfxSource.PlayOneShot(
            clips[index],
            Mathf.Clamp01(volume)
        );
    }
}
