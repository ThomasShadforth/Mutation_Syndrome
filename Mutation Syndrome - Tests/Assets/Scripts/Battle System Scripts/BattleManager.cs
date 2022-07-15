using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public GameObject battleScene;

    public bool battleActive;
    public bool cannotFlee;

    public BattleCharacter[] playerPartyPrefabs;
    public BattleCharacter[] enemyPrefabs;

    [SerializeField] Transform[] playerPositions;
    [SerializeField] Transform[] enemyPositions;

    public List<BattleCharacter> battlers = new List<BattleCharacter>();
    public BattleNumberUI damageNum;


    public Text[] PlayerPartyName;
    public Text[] PlayerPartyHP;

    List<BattleCharacter> PartyMembers = new List<BattleCharacter>();
    int partyIndex;
    

    public int chanceOfFleeing;

    public int currentTurn = 0;
    public bool turnWaiting;
    public bool fleeing;
    bool actionExecuting;

    public GameObject UIButtonHolder;
    public GameObject actionMenu;
    public GameObject targetSelectMenu;
    public GameObject turnIndicator;

    public TargetMenuButton[] targetButtons;
    public BattleMove[] moveList;

    //Battle Item Window
    public ItemButton[] battleItemButtons;
    public GameObject itemWindow;
    public Item selectedItem;
    public Text itemNameText, itemDescriptionText;


    //Character choice window
    public GameObject characterChoiceWindow;
    public Text[] characterItemChoiceNames;

    int turnIndicatorOffset = -500;

    public string[] rewardItems;
    public int rewardEXP;
    //Note: Organising characters by speed works.

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartBattle(false, new string[] { "Lurker", "Zombie", "Lurker"});
        }

        if (battleActive)
        {
            
        }
    }

    public void OrganiseTurns()
    {
        //testValues.Sort();
        //testValues.Reverse();
        battlers.Sort(delegate (BattleCharacter x, BattleCharacter y)
        {
            return x.speed.CompareTo(y.speed);
        });

        battlers.Reverse();
    }

    public void StartBattle(bool setCannotFlee, string[] enemiesToSpawn)
    {
        if (!battleActive)
        {
            battleActive = true;
            cannotFlee = setCannotFlee;

            GameManager.instance.isBattleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

            battleScene.SetActive(true);

            //Set battle music here based on what type (Normal, boss, etc)

            //array for player positions (Spawns the player and their party in the positions)
            for (int i = 0; i < playerPositions.Length; i++) { 
                if(GameManager.instance.activePartyMembers.Length != 0)
                {
                    if (GameManager.instance.activePartyMembers[i] != null)
                    {
                        for (int j = 0; j < playerPartyPrefabs.Length; j++)
                        {

                            if (playerPartyPrefabs[j].charName == GameManager.instance.activePartyMembers[i].characterName)
                            {
                                //Instantiate new player battle character, assign name, stats, etc.
                                BattleCharacter newPlayerChar = Instantiate(playerPartyPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                                
                                newPlayerChar.transform.parent = playerPositions[i];
                                battlers.Add(newPlayerChar);
                                PartyMembers.Add(newPlayerChar);

                                //Set character stats
                                CharacterStatus partyMember = GameManager.instance.activePartyMembers[i];

                                battlers[i].charName = partyMember.characterName;
                                //Calculate rest of stats, assign name and health to UI
                            }
                        }
                    }
                }
            }

            //Array for enemy positions
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                //Similarly, cycle through enemies to spawn
                //check if the name is blank. If not
                //cycle through enemy prefabs, if the names match, instantiate then assign to battle pos
                if (enemiesToSpawn[i] != "")
                {
                    for(int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            //Spawn the enemy
                            BattleCharacter newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            battlers.Add(newEnemy);
                            
                        }
                    }
                }
                
            }

            turnWaiting = true;
            OrganiseTurns();
            UpdateUIStats();
            PlayerTurn();
        }
    }

    #region Battle End Coroutines
    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        UIButtonHolder.SetActive(false);
        targetSelectMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < battlers.Count; i++)
        {
            if (battlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.activePartyMembers.Length; j++)
                {
                    if (GameManager.instance.activePartyMembers[j] != null)
                    {
                        if (GameManager.instance.activePartyMembers[j].characterName == battlers[i].charName)
                        {
                            CharacterStatus character = GameManager.instance.activePartyMembers[j];
                            character.HP = battlers[i].HP;
                        }
                    }
                    
                }
            }

            Destroy(battlers[i]);
        }

        UIFade.instance.FadeFromBlack();

        battleScene.SetActive(false);

        //Stop music
        PartyMembers.Clear();
        battlers.Clear();
        currentTurn = 0;

        if (fleeing)
        {
            battleScene.SetActive(false);
            //Just end the battle without rewards
        }
        else
        {
            //Open a battle reward screen
        }
    }

    public IEnumerator GameOverCo()
    {
        
        battleActive = false;

        yield return new WaitForSeconds(1.5f);

        battleScene.SetActive(false);

        //Load game over scene
    }

    #endregion

    #region General Battle Methods
    public void UpdateBattle()
    {
        bool allPlayersDead = true;
        bool allEnemiesDead = true;

        foreach(BattleCharacter battler in battlers)
        {
            if(battler.HP < 0)
            {
                battler.HP = 0;
            }

            if(battler.HP == 0)
            {
                if (battler.isPlayer)
                {
                    //change player sprite to dead sprite
                }
                else
                {
                    //Have enemy sprite fade away
                }
            }
            else
            {
                if (battler.isPlayer)
                {
                    allPlayersDead = false;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }

            if(allEnemiesDead || allPlayersDead)
            {
                if (allEnemiesDead)
                {
                    StartCoroutine(EndBattleCo());
                } else if (allPlayersDead)
                {
                    StartCoroutine(GameOverCo());
                }
            }
            else
            {
                while(battler.HP == 0)
                {
                    currentTurn++;

                    if (currentTurn > battlers.Count - 1)
                    {
                        currentTurn = 0;
                    }
                }
            }
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        UpdateBattle();
    }

    public void UpdateUIStats()
    {
        for(int i = 0; i < PlayerPartyName.Length; i++)
        {
            if (PartyMembers.Count > i)
            {
                //Update party member hp, name and stats
                BattleCharacter partyMember = PartyMembers[i];
                PlayerPartyName[i].gameObject.SetActive(true);
                PlayerPartyName[i].text = partyMember.charName;

                PlayerPartyHP[i].text = Mathf.Clamp(partyMember.HP, 0, int.MaxValue) + "/" + partyMember.maxHP;
            }
            else
            {
                PlayerPartyName[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if (cannotFlee)
        {
            //state the the player cannot run away from this fight
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);
            if(fleeSuccess <= chanceOfFleeing)
            {
                //flee battle
                fleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //Tell the player they were unsuccessful in running away
                //Note: Will need to tweak it so that it skips over all player turns (Use a boolean for this)

            }
        }
    }

    public IEnumerator TurnStart()
    {
        
        //Note replace yield return's values with animation values (Makes it flexible instead of cutting animations to a single second)
        for(currentTurn = 0; currentTurn < battlers.Count; currentTurn++)
        {
            if(battlers[currentTurn].HP <= 0)
            {
                continue;
            }
            else
            {
                if (battlers[currentTurn].isPlayer)
                {
                    if(battlers[currentTurn].selectedMov != "")
                    {
                        //Attack here
                        
                        
                        StartCoroutine(PlayerAttack(battlers[currentTurn].selectedMov, battlers[currentTurn].selectedTarget));
                        yield return new WaitForSeconds(1f);

                    }
                }
                else
                {
                    
                    
                    StartCoroutine(EnemyAttack());
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        PlayerTurn();
    }
    #endregion
    public void PlayerTurn()
    {
        turnIndicator.GetComponent<RectTransform>().anchoredPosition = PlayerPartyName[partyIndex].GetComponent<RectTransform>().anchorMin + new Vector2(turnIndicatorOffset, 55);
        partyIndex = 0;
        UIButtonHolder.SetActive(true);
    }

    #region Attack Methods/Coroutines
    public IEnumerator PlayerAttack(string attackName, int selectedTarget)
    {
        int movePower = 10;
        
        /*for (int i = 0; i < moveList.Length; i++) {
            if (attackName == moveList[i].moveName) {
                //instantiate move effect
                movePower = moveList[i].attackPower;
            } 
        }*/
        yield return null;
        DealDamage(selectedTarget, movePower);

        
    }

    public IEnumerator EnemyAttack()
    {
        

        int target = 0;

        List<BattleCharacter> playerCharacters = new List<BattleCharacter>();
        for(int i = 0; i < battlers.Count; i++)
        {
            if(battlers[i].isPlayer && battlers[i].HP > 0)
            {
                playerCharacters.Add(battlers[i]);
            }
        }

        playerCharacters.Sort(delegate (BattleCharacter x, BattleCharacter y)
        {
            return x.HP.CompareTo(y.HP);
        });

        playerCharacters.Reverse();

        int lastIndex = playerCharacters.Count - 1;
        if(playerCharacters[lastIndex].HP < playerCharacters[lastIndex - 1].HP)
        {
            for(int i = 0; i < battlers.Count; i++)
            {
                if(battlers[i].charName == playerCharacters[lastIndex].charName)
                {
                    target = i;
                    battlers[currentTurn].selectedTarget = target;
                }
            }
        }
        else
        {
            List<BattleCharacter> twoLeastHealth = new List<BattleCharacter>();
            twoLeastHealth.Add(playerCharacters[lastIndex - 1]);
            twoLeastHealth.Add(playerCharacters[lastIndex]);
            BattleCharacter targetChar = twoLeastHealth[Random.Range(0, twoLeastHealth.Count)];

            for(int i = 0; i < battlers.Count; i++)
            {
                if(battlers[i].charName == targetChar.charName)
                {
                    target = i;
                    battlers[currentTurn].selectedTarget = target;
                }
            }
        }

        /*
        BattleMove selectedMove = battlers[currentTurn].knownAttacks[Random.Range(0, battlers[currentTurn].knownAttacks.Length)];

        float totalDamage = selectedMove.attackPower;

        if(selectedMove.effectiveAgainst != "")
        {
            //Check effectivness here
            if(selectedMove.effectiveAgainst == battlers[battlers[currentTurn].selectedTarget].afflictedBy)
            {
                //Multiply damage
                totalDamage *= 1.5f;
            }
        }

        yield return null;
        DealDamage(battlers[currentTurn].selectedTarget, totalDamage);*/

        yield return null;
        DealDamage(battlers[currentTurn].selectedTarget, 10);
    }

    void DealDamage(int target, float movePower)
    {
        float atkPwr = 1f;
        float defPwr = 2f;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToDeal = Mathf.RoundToInt(damageCalc);

        

        battlers[target].HP = battlers[target].HP - damageToDeal;

        Debug.Log(damageToDeal);

        Instantiate(damageNum, battlers[target].transform.position, battlers[target].transform.rotation).setInitialDamage(damageToDeal);

        //Instantiate damage number effect

        UpdateUIStats();
    }
    #endregion

    #region Action Methods
    public void OpenActionMenu()
    {
        actionMenu.SetActive(true);
    }

    public void CloseActionMenu()
    {
        actionMenu.SetActive(false);
    }


    public void OpenTargetSelect(string actionName)
    {
        actionMenu.SetActive(false);
        PartyMembers[partyIndex].selectedMov = actionName;
        targetSelectMenu.SetActive(true);

        List<int> enemies = new List<int>();
        for(int i = 0; i < battlers.Count; i++)
        {
            if (!battlers[i].isPlayer)
            {
                enemies.Add(i);
            }
        }

        

        for(int i = 0; i < targetButtons.Length; i++)
        {
            if(enemies.Count > i && battlers[enemies[i]].HP > 0)
            {
                //Set up the target button with the name
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].actionName = actionName;
                targetButtons[i].enemyBattlerTarget = enemies[i];
                targetButtons[i].targetName.text = battlers[enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseTargetSelectMenu()
    {
        PartyMembers[partyIndex].selectedMov = "";
        targetSelectMenu.SetActive(false);
        actionMenu.SetActive(true);
    }

    public void SelectTarget(int targetIndex)
    {
        PartyMembers[partyIndex].selectedTarget = targetIndex;

        targetSelectMenu.SetActive(false);
        NextPlayerTurn();
    }

    private void NextPlayerTurn()
    {
        partyIndex++;

        //Set a variable called target index to the chosen value (Depending on the enemies present)
        //Set a variable called actionName to the chosen action.
        if (partyIndex < PartyMembers.Count)
        {
            turnIndicator.GetComponent<RectTransform>().anchoredPosition = PlayerPartyName[partyIndex].GetComponent<RectTransform>().anchorMin + new Vector2(turnIndicatorOffset + (250 * partyIndex), 55);
            return;
        }
        else
        {
            UIButtonHolder.SetActive(false);
            //StartTurn();
            StartCoroutine(TurnStart());
        }
    }
    #endregion

    #region Battle Effects

    public void ApplyEffect(int target, string effectApplied)
    {
        if(battlers[target].afflictedBy != "")
        {
            //Decide whether to overwrite or to replace
        }
        else
        {
            battlers[target].afflictedBy = effectApplied;
        }
    }

    public void BleedDamage()
    {
        
    }

    public void PainDamage()
    {
        if(battlers[currentTurn].isPlayer && battlers[currentTurn].afflictedBy == "Pain")
        {
            battlers[currentTurn].HP -= 2;
            UpdateUIStats();
            UpdateBattle();
        }
    }

    #endregion

    #region Item Menu

    public void OpenItemSelect()
    {
        ItemManager.instance.SortItems();
        battleItemButtons[0].Press();
        itemWindow.SetActive(true);

        for(int i = 0; i < battleItemButtons.Length; i++)
        {
            battleItemButtons[i].buttonIndex = i;
            if(ItemManager.instance.itemsHeld[i] != "")
            {
                battleItemButtons[i].buttonImage.gameObject.SetActive(true);
                battleItemButtons[i].buttonImage.sprite = ItemManager.instance.GetItemDetails(ItemManager.instance.itemsHeld[i]).itemSprite;
                battleItemButtons[i].amountText.text = ItemManager.instance.itemNumbers[i].ToString();
            }
            else
            {
                battleItemButtons[i].buttonImage.gameObject.SetActive(false);
                battleItemButtons[i].amountText.text = "";
            }
        }
    }

    public void CloseItemSelect()
    {
        itemWindow.SetActive(false);
    }

    public void SetSelectedItem(Item itemToUse)
    {
        selectedItem = itemToUse;
        itemNameText.text = selectedItem.itemName;
        itemDescriptionText.text = selectedItem.itemDescription;
        //Set the panel name and description to the item's name and description
    }

    public void UseItem()
    {
        if(selectedItem.isArmour || selectedItem.isWeapon)
        {
            //Notify the player that they cannot equip this during battle
        }
        else
        {
            OpenItemCharacterChoice();
            //Open the window for selecting which player to use an item on.
        }
    }

    public void OpenItemCharacterChoice()
    {
        characterChoiceWindow.SetActive(true);
        //Open the window
        for(int i = 0; i < characterItemChoiceNames.Length; i++)
        {
            if(GameManager.instance.activePartyMembers[i] != null)
            {
                characterItemChoiceNames[i].text = GameManager.instance.activePartyMembers[i].characterName;
                characterItemChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.activePartyMembers[i].gameObject.activeInHierarchy);
            }
            else
            {
                characterItemChoiceNames[i].text = "";
                characterItemChoiceNames[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    //Either when an character is selected, or the player backs out of this menu
    public void CloseItemCharacterChoice()
    {
        characterChoiceWindow.SetActive(false);
    }

    public void ConsumeItem(int selectedCharacter)
    {
        selectedItem.UseItem(selectedCharacter);
        PartyMembers[partyIndex].HP = GameManager.instance.activePartyMembers[partyIndex].HP;
        UpdateUIStats();
        CloseItemCharacterChoice();
        itemWindow.SetActive(false);
        NextPlayerTurn();
    }

    #endregion
}
