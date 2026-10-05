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

    private void Awake()
    {
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

        MessageManager.Instance.PlayMessage($"{item.ItemName}을(를) 획득했다.");
    }

    public void UseItem(ItemSO item)
    {
        if (!currentInventory.ContainsKey(item.ID) || currentInventory[item.ID] <= 0) return;

        currentInventory[item.ID]--;

        MessageManager.Instance.PlayMessage($"{item.ItemName}을(를) 사용했다.");

        if (currentInventory[item.ID] <= 0)
        {
            currentInventory.Remove(item.ID);
        }
    }
}
