using System;
using UnityEngine;

public enum ItemType
{
    Key,
    Consumable,
    equip
}

[CreateAssetMenu(fileName = "ItemSO", menuName = ("Item"))]
public class ItemSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string description;
    [SerializeField] private ItemType type;

    public int ID => id;
    public string Description => description;
    public ItemType Type => type;
}
