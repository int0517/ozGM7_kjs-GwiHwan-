
using UnityEngine;

public class GhostAudioController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource idleAudio;
    [SerializeField] private AudioSource chaseAudio;
    [SerializeField] private AudioSource attackAudio;

    private enum AudioState
    {
        None,
        Idle,
        Chase,
        Attack
    }

    private AudioState currentAudioState = AudioState.None;

    private void Awake()
    {
        // 반복 재생 설정
        if (idleAudio != null)
            idleAudio.loop = true;

        if (chaseAudio != null)
            chaseAudio.loop = true;

        // 공격 사운드는 한 번만 재생
        if (attackAudio != null)
            attackAudio.loop = false;
    }

    public void PlayForState(GhostStateEnum state)
    {
        switch (state)
        {
            case GhostStateEnum.Patrol:
            case GhostStateEnum.Search:
                PlayIdleAudio();
                break;

            case GhostStateEnum.Chase:
                PlayChaseAudio();
                break;

            case GhostStateEnum.Attack:
                PlayAttackAudio();
                break;
        }
    }

    private void PlayIdleAudio()
    {
        // Patrol과 Search 사이에서는 재시작하지 않음
        if (currentAudioState == AudioState.Idle)
            return;

        StopAllAudio();

        currentAudioState = AudioState.Idle;

        if (idleAudio != null)
            idleAudio.Play();
    }

    private void PlayChaseAudio()
    {
        if (currentAudioState == AudioState.Chase)
            return;

        StopAllAudio();

        currentAudioState = AudioState.Chase;

        if (chaseAudio != null)
            chaseAudio.Play();
    }

    private void PlayAttackAudio()
    {
        // 같은 Attack 상태에서 중복 재생하지 않음
        if (currentAudioState == AudioState.Attack)
            return;

        StopAllAudio();

        currentAudioState = AudioState.Attack;

        if (attackAudio != null)
            attackAudio.Play();
    }

    public void StopAllAudio()
    {
        if (idleAudio != null)
            idleAudio.Stop();

        if (chaseAudio != null)
            chaseAudio.Stop();

        if (attackAudio != null)
            attackAudio.Stop();
    }

    public void StopAllGhostAudio()
    {
        StopAllAudio();
        currentAudioState = AudioState.None;
    }
}
