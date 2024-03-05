using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private GameObject itemCanvasGrid;
    [SerializeField] private Sprite emptyIcon;
    [SerializeField] private GameObject headSlot;
    [SerializeField] private GameObject handSlot;
    
    private InventoryObject playerInventory;
    private void OnEnable() {
        playerInventory = FindObjectOfType<PlayerStatsController>().GetComponent<InventoryObject>();
        foreach(KeyValuePair<ItemObject, int> keyValuePairs in playerInventory.Inventory){
            var go = Instantiate(itemIcon, itemCanvasGrid.transform);
            var image = go.GetComponentInChildren<Image>();
            image.sprite = keyValuePairs.Key.itemSprite;
            var description = go.GetComponentInChildren<TextMeshProUGUI>();
            description.text = "Name: " + keyValuePairs.Key.name + "\nQuantidade: " + keyValuePairs.Value + "\nDescrição: " + keyValuePairs.Key.description;
          
            if (keyValuePairs.Key is EquipmentObject equipmentObject)
            {
                go.GetComponentInChildren<Button>().onClick.AddListener(() => UseEquipment(keyValuePairs.Key));
            }
        }
        SetupEquipment();
    }

    private void OnDisable() {
        playerInventory.Save();
    }

    private void SetupEquipment(){
        if(playerInventory.equipedItems["HeadSlot"] != null){
            headSlot.GetComponentInChildren<Image>().sprite = playerInventory.equipedItems["HeadSlot"].itemSprite;
        }else{
            headSlot.GetComponentInChildren<Image>().sprite = emptyIcon;
        }
        if(playerInventory.equipedItems["HandSlot"] != null){
            handSlot.GetComponentInChildren<Image>().sprite = playerInventory.equipedItems["HandSlot"].itemSprite;
        }else{
            handSlot.GetComponentInChildren<Image>().sprite = emptyIcon;
        }
    }

    private void Awake() {
        var equipment = headSlot.GetComponent<EquipmentPlaceHolder>().gameObject;
        var button = headSlot.GetComponentInChildren<Button>();
        button.onClick.AddListener(()=> RemoveEquipment(equipment));

        var equipment1 = handSlot.GetComponent<EquipmentPlaceHolder>().gameObject;
        var button1 = handSlot.GetComponentInChildren<Button>();
        button1.onClick.AddListener(()=> RemoveEquipment(equipment1));
    }


    private void UseEquipment(ItemObject equipment){
        EquipmentObject test = (EquipmentObject)equipment;
        var player = FindObjectOfType<PlayerStatsController>().GetComponent<PlayerStats>();
        if(equipment.equipmentType == EquipmentType.None){
            FindObjectOfType<NotifySystem>().Notify("Type is null " + equipment.equipmentType);
            return;
        }
        if(equipment.equipmentType == EquipmentType.Head){
            headSlot.GetComponentInChildren<Image>().sprite = equipment.itemSprite;
            playerInventory.equipedItems["HeadSlot"] = test;
            player.equipedItems["HeadSlot"] = test;
            Debug.Log(player.equipedItems["HeadSlot"].atkBonus);
            FindObjectOfType<NotifySystem>().Notify("Item equiped");
        }
        else if(equipment.equipmentType == EquipmentType.Hand){
            handSlot.GetComponentInChildren<Image>().sprite = equipment.itemSprite;
            playerInventory.equipedItems["HandSlot"] = test;
            player.equipedItems["HandSlot"] = test;
            Debug.Log(player.equipedItems["HandSlot"].atkBonus);
            FindObjectOfType<NotifySystem>().Notify("Item equiped");
        }else{
            FindObjectOfType<NotifySystem>().Notify("Fail on equip item");
        }
    }

    private void RemoveEquipment(GameObject go){
        var player = FindObjectOfType<PlayerStatsController>().GetComponent<PlayerStats>();
        if(go.name == "HeadSlot"){
            headSlot.GetComponentInChildren<Image>().sprite = emptyIcon;
            playerInventory.equipedItems["HeadSlot"] = null;
            player.equipedItems["HeadSlot"] = null;
            FindObjectOfType<NotifySystem>().Notify("Item removed");
        }else if(go.name == "HandSlot"){
            handSlot.GetComponentInChildren<Image>().sprite = emptyIcon;
            playerInventory.equipedItems["HandSlot"] = null;
            player.equipedItems["HandSlot"] = null;
            FindObjectOfType<NotifySystem>().Notify("Item removed");
        }else{
            FindObjectOfType<NotifySystem>().Notify("Fail to remove item");
        }
    }
}
