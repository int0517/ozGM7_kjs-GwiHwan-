using UnityEngine;

public class FlashLight : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject batteryUI;

    public void TryInteract()
    {
        PlayerInventoryManager.Instance.GetItem(itemSO);
        flashlight.SetActive(true);
        batteryUI.SetActive(true);
        Destroy(gameObject);
    }
}
