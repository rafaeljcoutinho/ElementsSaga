using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class InventoryObject : MonoBehaviour
{

    [SerializeField] private Dictionary<ItemObject,int> inventory = new();
    [SerializeField] public PlayerStats player;
    [SerializeField] public Dictionary<string, EquipmentObject> equipedItems = new();

    public Dictionary<ItemObject,int> Inventory => inventory;

    private void Awake() {
        equipedItems.Add("HeadSlot", null);
        equipedItems.Add("HandSlot", null);
    }

    public void LoadInventory(Dictionary<ItemObject,int> inventory, Dictionary<string, EquipmentObject> equipedItems){
        this.inventory = inventory;
        this.equipedItems = equipedItems;
    }
    public void AddItem(ItemObject _item, int _amount){

        if(inventory.ContainsKey(_item)){
            inventory[_item] += _amount;
        }else{
            inventory[_item] = _amount;
        }
    }

    public int GetItemQuantity(ItemObject _item){
        if(inventory.ContainsKey(_item)){
            return inventory.GetValueOrDefault(_item);
        }else{
            return 0;
        }
    }

    public bool TryRemoveItem(ItemObject _item, int _amount){
        if(inventory.ContainsKey(_item)){
            if(inventory[_item] - _amount < 0){
                return false;
            }else{
                inventory[_item] -= _amount;
                if(inventory[_item] == 0){
                        inventory.Remove(_item);
                }
                return true;
            }
        }
        return false;
    }

    public bool InventoryHasItem(ItemObject _item){
        if(inventory.ContainsKey(_item)){
            return true;
        }
        return false;
    }

    public void TryBuyAbilityPlayer(Ability ability){
        player.TryBuyAbilityPlayer(ability);

    }

    public void TryBuyPlayer(ItemObject item){

        if(inventory.ContainsKey(item)){
            if(player.gold >= item.cost){
                inventory[item] += 1;
                player.UseGold(item.cost);
                player.Save();
                Save();
                FindObjectOfType<NotifySystem>().Notify("Item purchased");
            }else{
                FindObjectOfType<NotifySystem>().Notify("More gold needed");
            }
            return;
        }

        if(player.gold >= item.cost){
            inventory.Add(item, 1);
            player.UseGold(item.cost);
            player.Save();
            Save();
            FindObjectOfType<NotifySystem>().Notify("Item purchased");
        }else{
            FindObjectOfType<NotifySystem>().Notify("More gold needed");
        }
        
    }

    public void Save(){
        Dictionary<string, string> nameToItemObject = new();
        foreach(KeyValuePair<string, EquipmentObject> keyValuePair in equipedItems){
            if(keyValuePair.Value == null){
                nameToItemObject.Add(keyValuePair.Key, null);
            }else{
                nameToItemObject.Add(keyValuePair.Key, keyValuePair.Value.name);
            }
        }
        SaveSystem.SaveInventory(inventory, nameToItemObject);
    }

}