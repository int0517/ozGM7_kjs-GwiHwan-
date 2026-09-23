using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;

    public void GetItem()
    {
        PlayerInventoryManager.Instance.GetItem(itemSO);
        Destroy(gameObject);
    }
}
