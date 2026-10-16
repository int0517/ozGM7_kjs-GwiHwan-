
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

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // AudioSource가 지정되지 않았다면 직접 가져오기
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    public void PlayPlayerAudio(int index)
    {
        PlayAudio(playerAudio, index);
    }

    public void PlayGhostAudio(int index)
    {
        PlayAudio(ghostAudio, index);
    }

    public void PlaySystemAudio(int index)
    {
        PlayAudio(systemAudio, index);
    }

    private void PlayAudio(AudioClip[] clips, int index)
    {
        if (clips == null || index < 0 || index >= clips.Length)
        {
            Debug.LogWarning($"잘못된 오디오 인덱스: {index}");
            return;
        }

        if (clips[index] == null)
        {
            Debug.LogWarning($"AudioClip이 비어 있습니다: {index}");
            return;
        }

        audioSource.PlayOneShot(clips[index]);
    }
}
