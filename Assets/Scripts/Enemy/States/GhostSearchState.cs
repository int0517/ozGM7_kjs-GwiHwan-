using UnityEngine;

public class GhostSearchState : GhostState
{
    private float searchTimer;
    private Quaternion searchStartRotation;

    public GhostSearchState(GhostController ghost) : base(ghost) { }

    public override void Enter()
    {
        searchTimer = 0f;

        ghost.Agent.speed = ghost.SearchSpeed;
        ghost.Agent.ResetPath();

        searchStartRotation = ghost.transform.rotation;
    }

    public override void Update()
    {
        // 둘러보는 중 플레이어 발견
        if (ghost.CanSeePlayer())
        {
            ghost.ChangeState(GhostStateEnum.Chase);
            return;
        }

        searchTimer += Time.deltaTime;

        // 좌우로 둘러보기
        float angle = Mathf.Sin(searchTimer * ghost.LookSpeed * Mathf.Deg2Rad) * ghost.LookAngle;

        ghost.transform.rotation = searchStartRotation * Quaternion.Euler(0f, angle, 0f);

        // 탐색 종료
        if (searchTimer >= ghost.SearchTime)
        {
            ghost.transform.rotation = searchStartRotation;
            searchTimer = 0f;

            // 다음 웨이포인트를 선택
            ghost.MoveNextWaypoint();

            // Patrol로 전환
            ghost.ChangeState(GhostStateEnum.Patrol);
        }
    }

    public override void Exit()
    {
        searchTimer = 0f;
    }
}