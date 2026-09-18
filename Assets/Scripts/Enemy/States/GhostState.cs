using UnityEngine;

public abstract class GhostState : MonoBehaviour
{
    protected GhostController ghost;

    protected GhostState(GhostController ghost)
    {
        this.ghost = ghost;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}
