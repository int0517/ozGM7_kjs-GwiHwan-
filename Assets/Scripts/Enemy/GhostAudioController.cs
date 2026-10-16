using UnityEngine;

public class GhostAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Ghost Audios")]
    [SerializeField] private AudioClip idleAudio;
    [SerializeField] private AudioClip chaseAudio;
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip keyDropAudio;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayIdleAudio()
    {
        PlaySound(idleAudio, true);
    }

    public void PlayChaseAudio()
    {
        PlaySound(chaseAudio, true);
    }

    public void PlayAttackAudio()
    {
        PlaySound(attackAudio, false);
    }

    public void PlayKeyDropAudio()
    {
        PlaySound(keyDropAudio, false, 0.2f);
    }

    private void PlaySound(AudioClip clip, bool loop, float vol = 0.1f)
    {
        if (clip == null) return;

        // 같은 사운드의 중복 재생 방지
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = vol;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
