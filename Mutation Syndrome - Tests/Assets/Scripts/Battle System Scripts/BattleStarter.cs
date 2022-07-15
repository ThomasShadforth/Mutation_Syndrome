using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleTypes[] battleTypes;

    public bool activateOnEnter, activateOnStay, activateOnLeave;

    bool inArea;

    public float battleTimer = 10f;
    float battleCounter;

    public bool deactivateAfterStarting;

    public bool cannotFlee;

    public bool shouldCompleteQuest;
    public string questToComplete;

    void Start()
    {
        battleCounter = Random.Range(battleTimer * .5f, battleTimer * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (inArea && PlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                battleCounter -= Time.deltaTime;
            }

            if(battleTimer <= 0)
            {
                battleTimer = Random.Range(battleTimer * .5f, battleTimer * 1.5f);
                StartCoroutine(StartBattleCo());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (activateOnEnter)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (activateOnLeave)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattleCo()
    {
        //UI Fade here
        UIFade.instance.FadeToBlack();
        GameManager.instance.isBattleActive = true;

        int selectedBattle = Random.Range(0, battleTypes.Length);

        BattleManager.instance.rewardItems = battleTypes[selectedBattle].rewardItems;
        BattleManager.instance.rewardEXP = battleTypes[selectedBattle].XPEarned;

        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.StartBattle(cannotFlee, battleTypes[selectedBattle].enemies);
        UIFade.instance.FadeFromBlack();


        if (deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }

        BattleRewards.instance.completesQuest = shouldCompleteQuest;
        BattleRewards.instance.questToComplete = questToComplete;
        
    }
}
