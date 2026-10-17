using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private GhostJumpScare ghost;

    public void TryInteract()
    {
        PlayerInventoryManager.Instance.GetItem(itemSO);
        AudioManager.Instance.PlaySystemAudio(5, 0.1f);

        if (itemSO.ID == 11)
        {
            ghost.PlayJumpScare();
        }

        Destroy(gameObject);
    }
}
