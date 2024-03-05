using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initialMenuController : MonoBehaviour
{
    [SerializeField] public List<Ability> allAbilitiesGame;
    [SerializeField] private TextMeshProUGUI goldStatus;
    [SerializeField] public List<ItemObject> allItemsGame;
    [SerializeField] public Dictionary<string, ItemObject> stringToItemObject = new();
    [SerializeField] private GameObject missionOverlay;
    [SerializeField] private GameObject profileOverlay;
    [SerializeField] private GameObject storeOverlay;
    [SerializeField] private GameObject abilitiesOverlay;
    [SerializeField] private GameObject inventoryOverlay;
    private PlayerStats playerStats;
    private AbilitiesData abilitiesData;
    private List<string> abilities = new();
    private List<string> selectedAbilities = new();



    public void OpenMissionMenu(){
        if(missionOverlay.activeSelf)
            missionOverlay.SetActive(false);
        else
            missionOverlay.SetActive(true);
    }
    public void OpenProfileMenu(){
        if(profileOverlay.activeSelf)
            profileOverlay.SetActive(false);
        else
            profileOverlay.SetActive(true);
    }

    public void OpenStoreMenu(){
        if(storeOverlay.activeSelf)
            storeOverlay.SetActive(false);
        else
            storeOverlay.SetActive(true);
    }

    public void OpenInventoryMenu(){
        if(inventoryOverlay.activeSelf)
            inventoryOverlay.SetActive(false);
        else
            inventoryOverlay.SetActive(true);
    }

    public void OpenAbilitiesMenu(){
        if(abilitiesOverlay.activeSelf)
            abilitiesOverlay.SetActive(false);
        else
            abilitiesOverlay.SetActive(true);
    }

    public void OpenLevel1(){
        SceneManager.LoadScene("Level 1");
    }

    public void OpenLevel2(){
        if(playerStats.level >= 2)
            SceneManager.LoadScene("Level 2");

        //popup notifier
    }

    public void OpenLevel3(){
        if(playerStats.level >= 3)
            SceneManager.LoadScene("Level 3");

        //popup notifier
    }

    public void UpdateGoldView(int amount){
        goldStatus. text = "Gold: " + amount.ToSafeString();
    }

    
    private void Awake() {
        //Save this dictionary
        foreach(ItemObject item in allItemsGame){
            stringToItemObject.Add(item.name, item);
        }
        playerStats = FindObjectOfType<PlayerStatsController>().GetComponent<PlayerStats>();
        if(SaveSystem.LoadPlayer() != null){
            PlayerData player = SaveSystem.LoadPlayer();
            playerStats.gold = player.currentGold;
            UpdateGoldView(playerStats.gold);
            playerStats.xp = player.currentXp;
            playerStats.level = player.level;
            playerStats.maxCp = player.maxCp;
            playerStats.maxHp = player.maxHp;
            playerStats.maxXp = player.maxXp;
        }
        if(SaveSystem.LoadAbilities() != null && playerStats.selectedAbilities.Count == 0){
            abilitiesData = SaveSystem.LoadAbilities();
            abilities = abilitiesData.abilities;
            selectedAbilities = abilitiesData.selectedAbilities;
            //use dictionary
            foreach(string abilityName in abilities){
                foreach(Ability ability in allAbilitiesGame){
                    if(abilityName == ability.name){
                        playerStats.abilities.Add(ability);
                    }
                }
            }
            foreach(string abilityName in selectedAbilities){
                foreach(Ability ability in allAbilitiesGame){
                    if(abilityName == ability.name){
                        playerStats.selectedAbilities.Add(ability);
                    }
                }
            }
        }
    }


}
