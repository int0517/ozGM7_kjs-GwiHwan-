using UnityEngine;

public class GhostActive : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GhostController ghost;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == playerLayer)
        {
            ghost.GhostActive();
            GetComponent<Collider>().enabled = false;
        }
    }
}
