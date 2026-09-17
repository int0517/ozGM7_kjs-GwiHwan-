using UnityEngine;

public class GhostAnimationController : MonoBehaviour
{
    private Animator animator;
    private GhostController ghost;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ghost = GetComponent<GhostController>();
    }

    private void Update()
    {
        ApplyAnimation();
    }

    private void ApplyAnimation()
    {
        switch(ghost.CurrentState)
        {
            case GhostStateEnum.Patrol:
                animator.SetBool("isChasing", false);
                animator.SetBool("isSearching", false);
                break;

            case GhostStateEnum.Chase:
                animator.SetBool("isChasing", true);
                animator.SetBool("isSearching", false);
                break;

            case GhostStateEnum.Search:
                animator.SetBool("isChasing", false);
                animator.SetBool("isSearching", true);
                break;
        }
    }
}
