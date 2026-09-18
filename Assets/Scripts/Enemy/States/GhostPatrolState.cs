using UnityEngine;

public class GhostPatrolState : GhostState
{
    private float waitTimer;

    public GhostPatrolState(GhostController ghost) : base(ghost) { }

    public override void Enter()
    {
        waitTimer = 0f;

        ghost.Agent.speed = ghost.PatrolSpeed;

        if (ghost.Waypoints.Length > 0)
        {
            ghost.Agent.SetDestination(ghost.Waypoints[ghost.CurrentWaypointIndex].position);
        }
    }

    public override void Update()
    {
        if (ghost.CanSeePlayer())
        {
            ghost.ChangeState(GhostStateEnum.Chase);

            return;
        }

        if (!ghost.Agent.pathPending && ghost.Agent.remainingDistance <= ghost.Agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= ghost.WaitTime)
            {
                waitTimer = 0f;

                ghost.MoveNextWaypoint();
            }
        }
    }

    public override void Exit()
    {
        waitTimer = 0f;
    }
}