
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GhostAttackState : GhostState
{
    private Coroutine killCoroutine;

    private const float moveDuration = 1;

    public GhostAttackState(GhostController ghost) : base(ghost) { }

    public override void Enter()
    {
        // 귀신 정지
        ghost.Agent.isStopped = true;
        ghost.Agent.ResetPath();

        // 플레이어 이동 및 회전 연출 시작
        killCoroutine = ghost.StartGhostCoroutine(KillPlayer());
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        // 실행 중인 코루틴 정리
        ghost.StopGhostCoroutine(killCoroutine);
        killCoroutine = null;

        // 플레이어 이동 허용
        GameManager.Instance.SetMoveAble(true);

        // 귀신 이동 재개
        ghost.Agent.isStopped = false;
    }

    private IEnumerator KillPlayer()
    {
        Transform player = ghost.Player;
        Transform rootPos = ghost.RootPos;

        if (player == null || rootPos == null)
        {
            Debug.LogWarning("Player 또는 RootPos가 연결되지 않았습니다.");
            yield break;
        }

        GameManager.Instance.SetMoveAble(false);

        Vector3 startPosition = player.position;
        Quaternion startRotation = player.rotation;

        Vector3 targetPosition = rootPos.position;
        Quaternion targetRotation = rootPos.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);

            player.SetPositionAndRotation(
                Vector3.Lerp(startPosition, targetPosition, t),
                Quaternion.Slerp(startRotation, targetRotation, t)
            );

            yield return null;
        }

        // 최종 위치와 회전을 정확히 일치시킴
        player.SetPositionAndRotation(targetPosition, targetRotation);

        yield return new WaitForSeconds(2f);

        yield return RestartSequence();
    }

    private IEnumerator RestartSequence()
    {
        // 재시작 중에는 귀신의 상태 전환과 감지를 제한
        ghost.SetRestarting(true);

        // 검은 화면 표시
        SetBlackScreenAlpha(1f);

        GameManager.Instance.SetMoveAble(false);

        ghost.Agent.isStopped = true;
        ghost.Agent.ResetPath();

        Transform ghostRestart = ghost.GhostRestartPos;
        Transform playerRestart = ghost.PlayerRestartPos;

        if (ghostRestart == null || playerRestart == null)
        {
            Debug.LogWarning("RestartPos가 연결되지 않았습니다.");
            ghost.SetRestarting(false);
            yield break;
        }

        // 귀신 위치 이동
        ghost.transform.SetPositionAndRotation(
            ghostRestart.position,
            ghostRestart.rotation
        );

        // NavMeshAgent의 내부 위치도 동기화
        ghost.Agent.Warp(ghostRestart.position);
        ghost.Agent.isStopped = true;
        ghost.Agent.ResetPath();

        // 플레이어 위치 이동
        ghost.Player.SetPositionAndRotation(
            playerRestart.position,
            playerRestart.rotation
        );

        // 검은 화면 유지
        yield return new WaitForSeconds(1f);

        // 순찰 상태로 전환하기 전에 NavMeshAgent 상태 정리
        ghost.Agent.Warp(ghostRestart.position);
        ghost.Agent.ResetPath();
        ghost.Agent.isStopped = false;

        // 귀신의 상태를 순찰로 변경
        ghost.ChangeState(GhostStateEnum.Patrol);

        // 화면 복구
        SetBlackScreenAlpha(0f);

        // 재시작 종료 및 게임 재개
        ghost.SetRestarting(false);
        GameManager.Instance.SetMoveAble(true);
    }

    private void SetBlackScreenAlpha(float alpha)
    {
        Image blackScreen = ghost.BlackScreen;

        if (blackScreen == null)
        {
            Debug.LogWarning("BlackScreen이 연결되지 않았습니다.");
            return;
        }

        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }
}
