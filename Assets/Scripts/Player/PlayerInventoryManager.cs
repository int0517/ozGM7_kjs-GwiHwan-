using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerInventoryManager : MonoBehaviour
{
    private static PlayerInventoryManager instance;
    public static PlayerInventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<PlayerInventoryManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerInventoryManager");
                    instance = obj.AddComponent<PlayerInventoryManager>();
                }
            }

            return instance;
        }
    }

    private Dictionary<int, int> currentInventory;
    public Dictionary<int, int> CurrentInventory => currentInventory;

    [SerializeField] private TextMeshProUGUI invenT;

    private void Awake()
    {
        GameObject invenTgameObject = GameObject.Find("TempInventoryT");
        invenT = invenTgameObject.GetComponent<TextMeshProUGUI>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            currentInventory = new Dictionary<int, int>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetItem(ItemSO item)
    {
        if (currentInventory.ContainsKey(item.ID))
        {
            currentInventory[item.ID]++;
        }
        else
        {
            currentInventory.Add(item.ID, 1);
        }

        invenT.text += $"\n{item.name}";
    }

    public void UseItem(ItemSO item)
    {
        if (!currentInventory.ContainsKey(item.ID) || currentInventory[item.ID] <= 0) return;

        currentInventory[item.ID]--;

        // 임시 텍스트 인벤
        string[] itemNames = invenT.text.Split('\n');
        List<string> newItemNames = new List<string>();
        bool removed = false;

        foreach (string itemName in itemNames)
        {
            if (!removed && itemName == item.name)
            {
                removed = true;
                continue;
            } 
            if (!string.IsNullOrEmpty(itemName))
            {
                newItemNames.Add(itemName);
            }
        }
        invenT.text = string.Join("\n", newItemNames);


        if (currentInventory[item.ID] <= 0)
        {
            currentInventory.Remove(item.ID);
        }
    }
}
