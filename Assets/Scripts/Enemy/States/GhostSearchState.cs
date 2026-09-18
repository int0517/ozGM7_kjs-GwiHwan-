using UnityEngine;

public class GhostSearchState : GhostState
{
    private float searchTimer;
    private Quaternion searchStartRotation;
    private bool initialized;

    public GhostSearchState(GhostController ghost) : base(ghost) { }

    public override void Enter()
    {
        searchTimer = 0f;
        initialized = false;

        ghost.Agent.speed = ghost.SearchSpeed;
        ghost.Agent.ResetPath();
    }

    public override void Update()
    {
        if (ghost.CanSeePlayer())
        {
            ghost.ChangeState(GhostStateEnum.Chase);

            return;
        }

        if (!initialized)
        {
            initialized = true;
            searchStartRotation = ghost.transform.rotation;
            ghost.Agent.ResetPath();
        }

        searchTimer += Time.deltaTime;

        float angle = Mathf.Sin(searchTimer * ghost.LookSpeed * Mathf.Deg2Rad) * ghost.LookAngle;

        ghost.transform.rotation = searchStartRotation * Quaternion.Euler(0, angle, 0);

        if (searchTimer >= ghost.SearchTime)
        {
            ghost.transform.rotation = searchStartRotation;
            ghost.SetNearestWaypoint();
            ghost.ChangeState(GhostStateEnum.Patrol);
        }
    }

    public override void Exit()
    {
        searchTimer = 0f;
        initialized = false;
    }
}