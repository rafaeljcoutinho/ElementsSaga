using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int currentXp;
    public int currentGold;
    public int maxXp;
    public int maxCp;
    public int maxHp;


    public PlayerData (PlayerStats playerStats){
        level = playerStats.level;
        currentGold = playerStats.gold;
        currentXp = playerStats.xp;
        maxCp = playerStats.maxCp;
        maxHp = playerStats.maxHp;
        maxXp = playerStats.maxXp;

    }
}


[System.Serializable]
public class AbilitiesData
{
    public List<string> abilities = new();
    public List<string> selectedAbilities = new();

    public AbilitiesData(List<Ability> abilitiesToSave, List<Ability> selectedAbilitiesToSave){
        foreach(Ability ability in abilitiesToSave){
            abilities.Add(ability.name);
        }
        foreach(Ability ability in selectedAbilitiesToSave){
            selectedAbilities.Add(ability.name);
        }
    }
}

[System.Serializable]
public class InventoryData
{
    public Dictionary<string, int> inventory = new();
    public Dictionary<string, string> equipedItems = new();

    public InventoryData(Dictionary<ItemObject, int> _inventory, Dictionary<string, string> _equipedItems){
        foreach (KeyValuePair<ItemObject, int> keyValuePair in _inventory){
            inventory.Add(keyValuePair.Key.name, keyValuePair.Value);
        }
        foreach (KeyValuePair<string, string> keyValuePair in _equipedItems){
            equipedItems.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}