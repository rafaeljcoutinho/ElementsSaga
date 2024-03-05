using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
[System.Serializable]
public class EquipmentObject : ItemObject
{
    [SerializeField] public int defenseBonus;
    [SerializeField] public int dodgeBonus;
    [SerializeField] public int criticalBonus;
    [SerializeField] public int atkBonus;
    [SerializeField] public int hpRegenPerTurn;
    [SerializeField] public int cpRegenPerTurn;


}

