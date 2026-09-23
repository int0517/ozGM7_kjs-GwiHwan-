using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private ItemSO keySO;
    private Renderer doorRenderer;

    private void Awake()
    {
        doorRenderer = GetComponent<Renderer>();
        doorRenderer.material.color = Color.green;
    }

    public void DoorOpen()
    {
        if (PlayerInventoryManager.Instance.CurrentInventory.ContainsKey(keySO.ID))
        {
            PlayerInventoryManager.Instance.UseItem(keySO);
            // 움직이는 걸로 후에 변경
            Destroy(gameObject);
        }
    }
}
