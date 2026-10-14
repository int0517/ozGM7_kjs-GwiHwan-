using UnityEngine;

public class InvenIconButtonClick : MonoBehaviour
{
    [SerializeField] private ItemSO item;

    public void SetItemSO(ItemSO item)
    {
        this.item = item;
    }

    public void OnClick()
    {
        InventoryUIManager.Instance.DisplayItemDescription(item);
    }
}
