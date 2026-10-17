using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryUIManager : MonoBehaviour
{
    private static InventoryUIManager instance;
    public static InventoryUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<InventoryUIManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("InventoryUIManager");
                    instance = obj.AddComponent<InventoryUIManager>();
                }
            }

            return instance;
        }
    }

    [Header("UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [Header("Items")]
    [SerializeField] private List<ItemSO> allItems = new List<ItemSO>();

    private Dictionary<int, ItemSO> itemDatabase = new Dictionary<int, ItemSO>();

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (inventoryPanel != null) inventoryPanel.SetActive(false);

        foreach (ItemSO item in allItems)
        {
            if (item != null && !itemDatabase.ContainsKey(item.ID))
            {
                itemDatabase.Add(item.ID, item);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)
            && SceneManager.GetActiveScene().name == "GameScene"
            && !GameManager.Instance.IsESC)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        IsOpen = !IsOpen;
        inventoryPanel.SetActive(IsOpen);
        GameManager.Instance.SetMoveAble(!GameManager.Instance.CanMove);

        Cursor.visible = IsOpen;
        Cursor.lockState = IsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = IsOpen ? 0f : 1f;

        if (IsOpen)
        {
            itemDescriptionText.text = "";
            RefreshInventory();
        }
    }

    private void RefreshInventory()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        Dictionary<int, int> inventory = PlayerInventoryManager.Instance.CurrentInventory;

        foreach (var entry in inventory)
        {
            if (!itemDatabase.TryGetValue(entry.Key, out ItemSO item))
            {
                Debug.LogWarning($"아이템 ID {entry.Key}를 찾을 수 없습니다.");
                continue;
            }

            GameObject slot = Instantiate(itemSlotPrefab, content);
            Image icon = slot.transform.Find("Icon").GetComponent<Image>();
            TMP_Text nameText = slot.transform.Find("NameT").GetComponent<TMP_Text>();
            TMP_Text countText = slot.transform.Find("CountT").GetComponent<TMP_Text>();
            InvenIconButtonClick button = slot.GetComponentInChildren<InvenIconButtonClick>();

            icon.sprite = item.Icon;
            nameText.text = item.ItemName;
            countText.text = $"x{entry.Value}";
            button.SetItemSO(item);
        }
    }

    public void DisplayItemDescription(ItemSO item)
    {
        itemDescriptionText.text = item.Description;
    }

    public void Bind(GameObject invenPanel, RectTransform content, TextMeshProUGUI itemDescriptionT)
    {
        inventoryPanel = invenPanel;
        this.content = content;
        itemDescriptionText = itemDescriptionT;

        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        IsOpen = false;
    }
}