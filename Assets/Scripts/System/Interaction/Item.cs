using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;

    public void TryInteract()
    {
        PlayerInventoryManager.Instance.GetItem(itemSO);
        AudioManager.Instance.PlaySystemAudio(5, 0.1f);
        Destroy(gameObject);
    }
}
