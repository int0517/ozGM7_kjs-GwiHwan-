using UnityEngine;

public class GhostActive : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GhostController ghost;
    [SerializeField] private StairsRoomDoorManager door;

    private void OnTriggerStay(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            ghost.GhostActive();
            door.DoorLock(true);
            GetComponent<Collider>().enabled = false;
        }
    }
}
