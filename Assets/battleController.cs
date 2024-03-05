using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class battleController : MonoBehaviour
{
    //test
    private float timer;
    [SerializeField] private float timeToAct;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private PlayerStats player;
    [SerializeField] private List<PlayerStats> playerFriends = new List<PlayerStats>();
    [SerializeField] private List<PlayerStats> enemies = new List<PlayerStats>();
    [Header("Recompensas da missao")]
    [SerializeField] private int gold;
    [SerializeField] private int xp;
    [SerializeField] private GameObject endBattleUI;
    [SerializeField] private TextMeshProUGUI rewardText;
    private PlayerStats playerStatsDb;
    private bool actionOpen;

    private List<PlayerStats> allPlayers = new List<PlayerStats>();

    private bool isEnd;

    private int playerIdx;

    private PlayerStats actionPlayer;

    public PlayerStats Player => player;
    private bool timerStop;
    private bool isCritical;
    private bool isDodge;

    private void Awake() {
        playerIdx = 0;
        actionOpen = true;
        actionPlayer = null;
        isEnd = false;
        rewardText.text = "+ " + gold + " gold\n+ " + xp + " xp";
        timer = timeToAct;
        timerStop = false;
        isCritical = false;
        LoadPlayer();


        allPlayers.Add(player);
        if(enemies.Count > 0){
            foreach (PlayerStats enemy in enemies){
                allPlayers.Add(enemy);
            }
        }
        if(playerFriends.Count > 0){
            foreach (PlayerStats friend in playerFriends){
                allPlayers.Add(friend);
            }
        }
    }


    private void Start() {

    }

    void Update()
    {
        if(isEnd){
            return;
        }
        
        //Controlar mortes e dar o resultado da partida
        //verificar se o player morreu
        if(actionOpen){
            PassAction();
        }else{
            if(!timerStop){
                timer -= Time.deltaTime;
                timerText.text = timer.ToString("0");
            }
            if(timer <= 0){
                PassAction();
            }
        }

        CheckEnemiesHP();
        CheckPlayerHP();
        
    }

    public void DeadPlayer(PlayerStats deadPlayer){
        foreach(PlayerStats player in allPlayers){
            if(player == deadPlayer){
                allPlayers.Remove(player);
                break;
            }
        }
        foreach(PlayerStats player in enemies){
            if(player == deadPlayer){
                enemies.Remove(player);
                break;
            }
        }
        foreach(PlayerStats player in playerFriends){
            if(player == deadPlayer){
                playerFriends.Remove(player);
                break;
            }
        }
    }

    public PlayerStats GetPlayerTarget(){
        if(isEnd){
            return null;
        }
        int defensorIdx = Random.Range(0, enemies.Count);
        return enemies[defensorIdx];
    }

    public PlayerStats GetNpcTarget(PlayerStats player){
        foreach(PlayerStats playerStats in playerFriends){
            if(player == playerStats){
                return GetPlayerTarget();
            }
        }

        int defensorIdx = Random.Range(0, playerFriends.Count+1);
        if(defensorIdx == 0){
            return this.player;
        }else{
            return playerFriends[defensorIdx - 1];
        }
    }

    private void CheckEnemiesHP(){
        int isDead = 0;
        foreach(PlayerStats enemy in enemies){
            if(enemy.isDead){
                isDead++;
            }
        }
        if(isDead == enemies.Count){
            EndBattle(true);
        }
    }

    private void CheckPlayerHP(){
        if(player.isDead){
            EndBattle(false);
        }
    }

    private void PassAction(){
        if(isEnd){
            return;
        }
        GiveAction();
        timer = timeToAct;
        int j = 0;
        while(actionOpen){

            if(j >= 50){
                Debug.Log("passou + " + j);
                break;
            }
            
            if(playerIdx >= allPlayers.Count){
                playerIdx = 0;
            }

            for(int i = playerIdx ; i<allPlayers.Count ; i++){
                var playerCanAct = allPlayers[i].PlayerGetAction();
                if(playerCanAct == true){
                    GetAction(allPlayers[i]);
                    playerIdx ++;
                    break;
                }
            }
            j++;
        }
    }


    public void GiveAction(){
        if(actionPlayer != null){
            actionPlayer.PlayerGiveAction();
        }
        actionOpen = true;
        actionPlayer = null;
    }

    public void GetAction(PlayerStats player){
        actionPlayer = player;
        actionOpen = false;
    }


    private void LoadPlayer(){
        playerStatsDb = FindObjectOfType<PlayerStatsController>().gameObject.GetComponent<PlayerStats>();
        var inventory = FindObjectOfType<PlayerStatsController>().gameObject.GetComponent<InventoryObject>();
            player.equipedItems = inventory.equipedItems;
            player.selectedAbilities = playerStatsDb.selectedAbilities;
            player.maxHp = playerStatsDb.maxHp;
            player.hp = player.maxHp;
            player.maxXp = playerStatsDb.maxXp;
            player.xp = playerStatsDb.xp;
            player.level = playerStatsDb.level;
            player.gold = playerStatsDb.gold;
            player.dodge = playerStatsDb.dodge;
            player.atkBonus = playerStatsDb.atkBonus;
            player.defBonus = playerStatsDb.defBonus;
            player.critical = playerStatsDb.critical;
    }

    public void PerformAbility(PlayerStats atacker, PlayerStats defensor, Ability ability){
        if(isEnd){
            return;
        }

        //atacker executar animação da ability
        //defensor executar animação de tomar a ability
        if(ability.heal > 0){
            atacker.GiveHp(ability.heal);
        }
        if(ability.damage > 0){
            defensor.TakeDamage((int)CalculateDamage(atacker, defensor, ability.damage), isCritical, isDodge);
        }
        isCritical = false;
        isDodge = false;
        StartCoroutine(WaitForAbilityCast(.2f)); //voltar para tempo da abilidade

        //no momento certo da animação, executar os eventos tanto de healing quanto de take damage.
    }

    private float CalculateDamage(PlayerStats atacker, PlayerStats defensor, int damage){

        int equipmentCritical = 0;
        int equipmentDodge = 0;
        foreach (KeyValuePair<string, EquipmentObject> item in atacker.equipedItems)
        {
            if(item.Value != null){
                damage += item.Value.atkBonus;
                equipmentCritical += item.Value.criticalBonus;
            }
            
            
        }
        foreach (KeyValuePair<string, EquipmentObject> item in defensor.equipedItems)
        {
            if(item.Value != null){
                damage -= item.Value.atkBonus;
                if(damage <= 0){
                    damage = 0;
                }
                equipmentDodge += item.Value.dodgeBonus;
            }
        }
        var random = Random.Range(1,3);
        var percentage = Random.Range(1,101);
        var damageVariation = 1;
        float maxVariation = 0.15f;

        var randomCritChance = Random.Range(1,101);
        var randomDodgeChance = Random.Range(1,101);

        if(randomDodgeChance <= defensor.dodge + equipmentDodge){
            isDodge = true;
        }

        if(random % 2 == 0){
            damageVariation = -1;
        }
        float newDamage = maxVariation * damage * percentage/100 * damageVariation ;
        newDamage += damage;
        if(randomCritChance <= atacker.critical + equipmentCritical){
            isCritical = true;
            newDamage *= 1.7f;
        }


        return newDamage;
    }

    private IEnumerator WaitForAbilityCast(float abilityTime){
        timerStop = true;
        timerText.text = "Action";
        timerText.fontSize = 30;
        yield return new WaitForSeconds(abilityTime);
        timerText.text = "";
        timerText.fontSize = 67;
        timerStop = false;
        PassAction();
        yield return null;
    }

    public void OpenEndBattleUI(){
        endBattleUI.SetActive(true);
    }

    public void BackToMenu(){
        SceneManager.LoadScene("initial scene");
    }
    private void EndBattle(bool completed){
        isEnd = true;
        StopAllCoroutines();
        StartCoroutine(WaitEndBattleTime(completed));
    }

    private IEnumerator WaitEndBattleTime(bool completed){
        yield return new WaitForSeconds(1);
        if(completed){
            GiveReward();
        }else{
            rewardText.text = "Defeated";
        }
        OpenEndBattleUI();
        SaveProgress();
    }

    private void GiveReward(){
        player.GiveGold(gold);
        player.GiveXP(xp);
    }

    private void SaveProgress(){
        SaveSystem.SavePlayer(player);
    }
}
