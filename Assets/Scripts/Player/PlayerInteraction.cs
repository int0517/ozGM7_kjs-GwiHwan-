using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask doorLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) TryInteract();
    }

    private void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // 아이템
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, itemLayer))
        {
            Item item = hit.collider.GetComponent<Item>();

            if (item != null)
            {
                item.GetItem();
                return;
            }
        }

        // 문
        if (Physics.Raycast(ray, out RaycastHit doorHit, interactDistance, doorLayer))
        {
            DoorManager doorManager = doorHit.collider.GetComponent<DoorManager>();

            if (doorManager != null)
            {
                doorManager.DoorOpen();
                return;
            }
        }
    }
}
