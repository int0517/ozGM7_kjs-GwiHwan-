using UnityEngine;

public class GhostPatrolState : GhostState
{
    public GhostPatrolState(GhostController ghost) : base(ghost) { }

    public override void Enter()
    {
        ghost.Agent.speed = ghost.PatrolSpeed;

        if (ghost.Waypoints.Length > 0)
        {
            ghost.Agent.SetDestination(ghost.Waypoints[ghost.CurrentWaypointIndex].position);
        }
    }

    public override void Update()
    {
        // 플레이어 발견
        if (ghost.CanSeePlayer())
        {
            ghost.ChangeState(GhostStateEnum.Chase);
            return;
        }

        // 웨이포인트 도착
        if (!ghost.Agent.pathPending && ghost.Agent.remainingDistance <= ghost.Agent.stoppingDistance)
        {
            ghost.ChangeState(GhostStateEnum.Search);
        }
    }
}