using UnityEngine;

public class KeyJumpScareTrigger : MonoBehaviour
{
    [SerializeField] private GhostJumpScare ghost;

    private void OnDestroy()
    {
        ghost.PlayJumpScare();
    }
}
