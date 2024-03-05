using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private List<Ability> abilities = new();
    [SerializeField] private List<ItemObject> items = new();

    [SerializeField] private Canvas abilitiesCanvasGrid;
    [SerializeField] private GameObject equipmentGrid;
    [SerializeField] private Canvas itemsCanvasGrid;

    [SerializeField] private GameObject itemIconCanvas;
    [SerializeField] private Sprite blockedIcon;
    [SerializeField] private InventoryObject inventory;

    private bool menuLoaded;
    

    private void Awake() {
        inventory = FindObjectOfType<PlayerStatsController>().GetComponent<InventoryObject>();
        
    }

    private void LoadMenu(){
        LoadAbilities();
        LoadItems();
    }

    private void OnEnable() {
        if(!menuLoaded)
            LoadMenu();
        menuLoaded = true;
    }



    private void LoadAbilities(){
        var go = FindObjectOfType<initialMenuController>().GetComponent<initialMenuController>();
        abilities = go.allAbilitiesGame;
        foreach(Ability ability in abilities){
            
            var go2 = Instantiate(itemIconCanvas,abilitiesCanvasGrid.gameObject.transform);
            var image = go2.GetComponentInChildren<Image>();
            image.sprite = ability.abilitySprite;
            var description = go2.GetComponentInChildren<TextMeshProUGUI>();
            var button = go2.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => inventory.TryBuyAbilityPlayer(ability));

            description.text = "Name: " + ability.name + "\nLevel: " + ability.levelRequired + "\nPrice: " + ability.cost + "\n" + ability.description; 

        }
        
    }

    private void LoadItems(){
        var initialMenuController = FindObjectOfType<initialMenuController>().GetComponent<initialMenuController>();
        items = initialMenuController.allItemsGame;
        foreach(ItemObject item in items){
            GameObject go = new();
            if(item.type != ItemType.Equipment){
                go = Instantiate(itemIconCanvas,itemsCanvasGrid.gameObject.transform);
            }else{
                go = Instantiate(itemIconCanvas,equipmentGrid.gameObject.transform);
            }
            
            var image = go.GetComponentInChildren<Image>();
            var description = go.GetComponentInChildren<TextMeshProUGUI>();
            var button = go.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => inventory.TryBuyPlayer(item));

            description.text = "Name: " + item.name + "\nPrice: " + item.cost + "\n" + item.description; 
            image.sprite = item.itemSprite;
        }
        
    }


}
