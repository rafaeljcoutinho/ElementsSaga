using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileSetup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI profileStats;
    private PlayerStats playerStats;

    private void OnEnable() {
        playerStats = FindObjectOfType<PlayerStatsController>().GetComponent<PlayerStats>();
        int equipmentDefense = 0;
        int equipmentDamage = 0;
        int equipmentDodge = 0;
        int equipmentCritical = 0;
        foreach (KeyValuePair<string, EquipmentObject> keyValuePair in playerStats.equipedItems)
        {
            if(keyValuePair.Value == null){
                continue;
            }
            equipmentCritical += keyValuePair.Value.criticalBonus;
            equipmentDodge += keyValuePair.Value.dodgeBonus;
            equipmentDamage += keyValuePair.Value.atkBonus;
            equipmentDefense += keyValuePair.Value.defenseBonus;
        }

        equipmentCritical += playerStats.critical;
        equipmentDodge += playerStats.dodge;
        equipmentDamage += playerStats.atkBonus;
        equipmentDefense += playerStats.defBonus;
        profileStats.text = 
            "\nName: " + playerStats.nickname 
            + "\nHP: " + playerStats.maxHp
            + "\nLevel: " + playerStats.level 
            + "\nGold: " + playerStats.gold 
            + "\nXp: " + playerStats.xp + " / " + playerStats.maxXp
            + "\nCritical chance: " +  equipmentCritical
            + "\nDodge chance: " + equipmentDodge
            + "\nBonus atk: " + equipmentDamage
            + "\nBonus def: " + equipmentDefense;
    }
}
