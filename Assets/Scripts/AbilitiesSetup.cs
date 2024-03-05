using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesSetup : MonoBehaviour
{
    [SerializeField] private List<Ability> abilitiesPlayer = new();
    [SerializeField] private GameObject canvasAbilitiesPlayer;
    [SerializeField] private List<Ability> selectedAbilities = new();
    [SerializeField] private GameObject canvasSelectedAbilities;
    [SerializeField] private GameObject canvasIcon;
    //[SerializeField] private List<Ability> allAbilities;
    private Ability selectedAbility;
    private Button selectedButton;
    //private int playerLevel;
    private int abilityNumbers = 5;
    PlayerStats playerStats;

    private Dictionary<Button, Ability> buttonAbility = new();




    private void OnEnable() {
        playerStats = FindObjectOfType<PlayerStatsController>().GetComponent<PlayerStats>();
        LoadAbilities();
        Setup();
    }



    private void Setup(){
        int i = 0;
        foreach(Ability ability in abilitiesPlayer){
            var go = Instantiate(canvasIcon, canvasAbilitiesPlayer.transform, true);
            go.GetComponentInChildren<Image>().sprite = ability.abilitySprite;
            var currentIndex = i;
            go.GetComponentInChildren<Button>().onClick.AddListener(()=> AddAbility(currentIndex));

            i++;
        }
        
        for(i = 0 ; i<abilityNumbers ; i++){
            var go = Instantiate(canvasIcon, canvasSelectedAbilities.transform, true);
            if(i < selectedAbilities.Count){
                go.GetComponentInChildren<Image>().sprite = selectedAbilities[i].abilitySprite;
                buttonAbility.Add(go.GetComponentInChildren<Button>(), selectedAbilities[i]);
            }

            var currentButton = go.GetComponentInChildren<Button>();
            go.GetComponentInChildren<Button>().onClick.AddListener(()=> AddButton(currentButton));
        }
    }

    private void Update() {
        if(selectedAbility != null && selectedButton != null){
            SetAbilityButton(selectedAbility, selectedButton);
            selectedAbility = null;
            selectedButton = null;
        }
    }

    private void SetAbilityButton(Ability selectedAbility, Button selectedButton){
        RemoveAbilityButton(selectedButton);
        AddAbilityButton(selectedAbility, selectedButton);
    }

    private void AddAbility(int abilityIdx){
        selectedAbility = abilitiesPlayer[abilityIdx];
        FindObjectOfType<NotifySystem>().Notify("Ability selected");
    }

    private void AddButton(Button button){
        selectedButton = button;
        FindObjectOfType<NotifySystem>().Notify("Button selected");
    }

    public void Save(){
        if(playerStats != null)
            SaveSystem.SaveAbilities(playerStats.abilities, playerStats.selectedAbilities);
    }

    public void Delete(){
        if(selectedButton != null){
            RemoveAbilityButton(selectedButton);
            selectedButton = null;
        }
    }

    public void SendPlayerSelectedAbilities(){
        playerStats.selectedAbilities = selectedAbilities;
    }


    private void LoadAbilities(){
        //initialMenuController menuController = FindObjectOfType<initialMenuController>();
        //playerLevel = playerStats.level;
        //allAbilities = menuController.allAbilitiesGame;
        abilitiesPlayer = playerStats.abilities;
        selectedAbilities = playerStats.selectedAbilities;
    }

    public void AddAbilityButton(Ability ability, Button button){
        button.image.sprite = ability.abilitySprite;
        buttonAbility.Add(button, ability);
        selectedAbilities.Add(ability);
    }

    public void RemoveAbilityButton(Button button){
        button.image.sprite = null;
        buttonAbility.TryGetValue(button, out Ability abilityInButton);
        if(abilityInButton)
            selectedAbilities.Remove(abilityInButton);

        buttonAbility.Remove(button);
    
    }
}
