using UnityEngine;

public class GhostChaseState : GhostState
{
    private Vector3 lastKnownPlayerPosition;

    private float chaseMemoryTimer;
    private bool reachedLastKnownPosition;

    public GhostChaseState(GhostController ghost) : base(ghost) { }

    public override void Enter()
    {
        chaseMemoryTimer = 0f;
        reachedLastKnownPosition = false;

        ghost.Agent.speed = ghost.ChaseSpeed;

        if (ghost.Player != null)
        {
            lastKnownPlayerPosition = ghost.Player.position;

            ghost.Agent.SetDestination(ghost.Player.position);
        }
    }

    public override void Update()
    {
        if (ghost.CanSeePlayer())
        {
            chaseMemoryTimer = 0f;
            lastKnownPlayerPosition = ghost.Player.position;
            ghost.Agent.SetDestination(ghost.Player.position);

            return;
        }

        chaseMemoryTimer += Time.deltaTime;

        // 일정 시간 동안 마지막 위치를 추적
        if (chaseMemoryTimer < ghost.ChaseMemoryTime)
        {
            ghost.Agent.SetDestination(lastKnownPlayerPosition);

            return;
        }

        // 마지막 위치까지 도착
        if (!reachedLastKnownPosition)
        {
            ghost.Agent.SetDestination(lastKnownPlayerPosition);

            if (!ghost.Agent.pathPending &&  ghost.Agent.remainingDistance <= ghost.Agent.stoppingDistance)
            {
                reachedLastKnownPosition = true;

                ghost.ChangeState( GhostStateEnum.Search);
            }
        }
    }

    public override void Exit()
    {
        chaseMemoryTimer = 0f;
        reachedLastKnownPosition = false;
    }
}