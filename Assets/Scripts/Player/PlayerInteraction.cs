using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) CheckInteract();
    }

    private void CheckInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable obj = hit.collider.GetComponent<IInteractable>();

            if (obj != null)
            {
                obj.TryInteract();
                return;
            }
        }
    }
}
