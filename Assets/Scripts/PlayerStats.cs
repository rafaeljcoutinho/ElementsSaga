using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour , IDamage
{
    [SerializeField] public GameObject textDamage;
    [SerializeField] private PlayerStats target;
    [SerializeField] private GameObject targetIndicator;
    [SerializeField] private healthBarController healthBar;
    [SerializeField] private battleController battleController;
    [SerializeField] public Dictionary<string, EquipmentObject> equipedItems = new();
    [SerializeField] public GameObject visual;
    [SerializeField] private GameObject actionUI;
    [SerializeField] public List<Ability> abilities = new();
    [SerializeField] public List<Ability> selectedAbilities = new();
    [SerializeField] private List<Button> abilitiesButtons = new();
    [SerializeField] public int hp;
    [SerializeField] public int maxHp;
    [SerializeField] public string nickname;
    [SerializeField] public int cp;
    [SerializeField] public int maxCp;
    [SerializeField] public int level;
    [SerializeField] public int xp;
    [SerializeField] public int maxXp;
    [SerializeField] public int gold;
    [SerializeField] public int defBonus;
    [SerializeField] public int dodge;
    [SerializeField] public int critical;
    [SerializeField] public int atkBonus;
    [SerializeField] public bool isDead;

    private string targetIndicatorObjectName = "TargetIndicator";
    private string initialGameSceneName = "initial scene";

    private bool isAction;
    private int stunTurns;



    private void Awake() {
        isAction = false;
        isDead = false;
        stunTurns = 0;
        hp = maxHp;
        cp = maxCp;


    }

    

    private void Start() {
        GetTarget();
        var x = SceneManager.GetActiveScene();
        if(x.name == initialGameSceneName){
            equipedItems.Add("HeadSlot", null);
            equipedItems.Add("HandSlot", null);
            LoadInventory();
        }
        if(battleController != null && this == battleController.Player){
            SetupAbilities();
            TargetEventHandler.OnTargetClick += SetTarget;
            targetIndicator = GameObject.Find(targetIndicatorObjectName);
            targetIndicator.SetActive(false);
            SetTargetIndicatorPosition();
        }
    }
    private void Update() {
        if(isDead){
            return;
        }

        if(target != null && target.isDead){
            GetTarget();
            if(targetIndicator != null){
                SetTargetIndicatorPosition();
            }
        }

        if(isAction){
            if(this != battleController.Player){
                PlayerAction();
            }
        }
    }

    private void LoadInventory(){
        var inventory = gameObject.AddComponent<InventoryObject>();
        inventory.player = this;
        var inventoryDataComplete = SaveSystem.LoadInventory();
        var inventoryData = inventoryDataComplete.inventory;
        var equipedItemsData = inventoryDataComplete.equipedItems;
        var initialMenuController = FindObjectOfType<initialMenuController>();
        foreach(ItemObject item in initialMenuController.allItemsGame){
            if(inventoryData.ContainsKey(item.name)){
                inventory.AddItem(item, inventoryData[item.name]);
            }
        }


        
        if(equipedItemsData["HeadSlot"] != null){
            EquipmentObject item1 = (EquipmentObject)initialMenuController.stringToItemObject[equipedItemsData["HeadSlot"]];
            inventory.equipedItems["HeadSlot"] = item1;
            equipedItems["HeadSlot"] = item1;
        }
        if(equipedItemsData["HandSlot"] != null){
            EquipmentObject item2 = (EquipmentObject)initialMenuController.stringToItemObject[equipedItemsData["HandSlot"]];
            inventory.equipedItems["HandSlot"] = item2;
            equipedItems["HandSlot"] = item2;
        }
    }

    private void OnDestroy() {
        TargetEventHandler.OnTargetClick -= SetTarget;
    }

    private void SetupAbilities(){
        int i = 0;
        foreach(Ability ability in selectedAbilities){
            if(i>=5)
                break;
            abilitiesButtons[i].image.sprite = ability.abilitySprite;

            int currentIndex = i;
            abilitiesButtons[i].onClick.AddListener(() => CastAbility(currentIndex));

            i++;
        }
    }

    private void SetTargetIndicatorPosition(){
        if(target == null){
            return;
        }
        targetIndicator.transform.position = Camera.main.WorldToScreenPoint(target.transform.position + Vector3.up * 3);
    }

    private void CastAbility(int abilityIndex) {
        HideUI();
        if(battleController.Player != this)
            GetTarget();
        battleController.PerformAbility(this, target, selectedAbilities[abilityIndex]);
    }


    private void SetTarget(object sender, TargetEventArgs e){
        target = e.PlayerStats;
        SetTargetIndicatorPosition();
    }

    private void PlayerAction(){
        isAction = false;
        StartCoroutine(PerformAction());

    }

    private IEnumerator PerformAction(){
        var timeToAct = Random.Range(1.5f,2.5f);
        timeToAct = 0f;
        yield return new WaitForSeconds(timeToAct);
        GetTarget();
        var randomAbility = Random.Range(0,abilities.Count);
        battleController.PerformAbility(this, target, abilities[randomAbility]);
        yield return null;
    }
    

    private void ShowUI(){
        if(actionUI != null){
            actionUI.SetActive(true);
            targetIndicator.SetActive(true);
        }
        
    }


    private void HideUI(){
        if(actionUI != null){
            actionUI.SetActive(false);
            targetIndicator.SetActive(false);
        }
    }

    public void TryBuyAbilityPlayer(Ability ability){
        if(abilities.Contains(ability)){
            FindObjectOfType<NotifySystem>().Notify("You already have this");
            return;
        }
        if(ability.cost <= gold && ability.levelRequired <= level){
            gold -= ability.cost;
            abilities.Add(ability);
            SaveSystem.SaveAbilities(abilities, selectedAbilities);
            FindObjectOfType<NotifySystem>().Notify("Ability purchased");
        }
    }


    public void Save(){
        SaveSystem.SavePlayer(this);
    }
    public bool PlayerGetAction(){
        if(isDead){
            return false;
        }

        if(stunTurns > 0){
            stunTurns--;
            ShowStunnedPopup();
            return false;
        }else{
            isAction = true;
            ShowUI();
            return true;
        }
        
    }

    public void SetTarget(PlayerStats playerStats){
        target = playerStats;
    }

    private void GetTarget(){
        if(battleController == null){
            return;
        }
        if(this == battleController.Player){
            target = battleController.GetPlayerTarget();
        }else{
            target = battleController.GetNpcTarget(this);
        }
    }

    public void PlayerGiveAction(){
        isAction = false;
        HideUI();
    }


    public void GiveXP(int amount){
        xp += amount;
        if(xp >= maxXp){
            xp -= maxXp;
            levelUp();
        }
    }

    private void levelUp(){
        maxHp += 40;
        maxCp += 40;
        maxXp += 10;
        level++;

    }
    public void GiveGold(int amount){
        gold += amount;
    }
    public void UseGold(int amount){
        gold -= amount;
        var initialMenuController = FindObjectOfType<initialMenuController>().GetComponent<initialMenuController>();
        initialMenuController.UpdateGoldView(gold);
    }

    public void GiveHp(int amount){
        ShowPopupHealing(amount);
        if(hp + amount >= maxHp){
            hp = maxHp;
        }else{
            hp += amount;
        }
        float fillAmount = (float)hp/maxHp;
        healthBar.setHealthbar(fillAmount);
    }
    public void TakeDamage(int amount, bool isCritical, bool isDodge){
        ShowPopupDamage(amount, isCritical, isDodge);
        if(isDodge){
            return;
        }
        int aux = hp - amount;
        if(aux <= 0){
            isDead = true;
            Die();
            hp = 0;
        }else{
            hp -= amount;
        }
        float fillAmount = (float)hp/maxHp;
        healthBar.setHealthbar(fillAmount);
    }

    private void Die(){
        visual.transform.Rotate(Vector3.right, 90);
        battleController.DeadPlayer(this); 
    }

    private void ShowPopupDamage(int amount, bool isCritical, bool isDodge){
        var go = Instantiate(textDamage, transform.position + Vector3.up * 3.5f, Quaternion.identity);
        go.GetComponent<TextMeshPro>().color = Color.red;
        go.GetComponent<TextMeshPro>().text = amount.ToString();
        if(isDodge){
            go.GetComponent<TextMeshPro>().text = "Dodge";
            go.GetComponent<TextMeshPro>().color = Color.white;
        }else if(isCritical){
            go.GetComponent<TextMeshPro>().color = Color.yellow;
            go.GetComponent<TextMeshPro>().fontSize = 48;
        }
    }
    private void ShowPopupHealing(int amount){
        var go = Instantiate(textDamage, transform.position + Vector3.up * 3.5f, Quaternion.identity);
        go.GetComponent<TextMeshPro>().color = Color.green;
        go.GetComponent<TextMeshPro>().text = amount.ToString();
    }
    private void ShowStunnedPopup(){
        var go = Instantiate(textDamage, transform.position + Vector3.up * 3.5f, Quaternion.identity);
        go.GetComponent<TextMeshPro>().color = Color.blue;
        go.GetComponent<TextMeshPro>().text = "Stunned " + stunTurns + " turns";
    }

}

public interface IDamage{
    public void TakeDamage(int amount, bool isCritical, bool isDodge);
}
