using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Potion")]
[System.Serializable]
public class PotionObject : ItemObject
{
    [SerializeField] private int amount;
    void Awake()
    {
        type = ItemType.Potion;
    }
}
