using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Equipment,
    Potion,
    Default
}

public enum EquipmentType{
    Hand,
    Head,
    None
}


public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public Sprite itemSprite;
    public ItemType type;
    public EquipmentType equipmentType;
    [TextArea(15,20)]
    public string description;
    public int cost;

}

