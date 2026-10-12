using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;

    public void TryInteract()
    {
        PlayerInventoryManager.Instance.GetItem(itemSO);
        Destroy(gameObject);
    }
}
