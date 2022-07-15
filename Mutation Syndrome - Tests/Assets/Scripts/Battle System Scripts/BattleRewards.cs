using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewards : MonoBehaviour
{
    public static BattleRewards instance;

    public Text xpText, itemText;
    public GameObject screenObject;

    public string[] itemsToGive;
    public int xpEarned;

    public bool completesQuest;
    public string questToComplete;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRewardsScreen(int xpGained,string[] rewards)
    {
        xpEarned = xpGained;
        itemsToGive = rewards;

        xpText.text = "Everyone earned " + xpEarned + " EXP!";
        itemText.text = "";

        for(int i = 0; 9 < itemsToGive.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        screenObject.SetActive(true);
    }

    public void CloseRewardsScreen()
    {
        for(int i = 0; i < GameManager.instance.activePartyMembers.Length; i++)
        {
            if(GameManager.instance.activePartyMembers[i] != null)
            {
                //Add EXP to player object here
            }
        }

        for(int i = 0; i < itemsToGive.Length; i++)
        {
            ItemManager.instance.AddItem(itemsToGive[i]);
            //Call the itemManager's add item method
        }

        screenObject.SetActive(false);
        GameManager.instance.isBattleActive = false;

        if (completesQuest)
        {
            //Mark a quest as complete (Will implement quests at a later point if possible)
        }
    }
}
