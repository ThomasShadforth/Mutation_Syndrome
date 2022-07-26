using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterStatus[] allCharacters;
    public CharacterStatus[] activePartyMembers = new CharacterStatus[4];

    public bool isBattleActive;
    public bool isGameMenuOpen;
    public bool isShopMenuOpen;

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
        CharacterNeedsTick();
    }

    void CharacterNeedsTick()
    {
        for(int i = 0; i < activePartyMembers.Length; i++)
        {
            if(activePartyMembers[i] != null)
            {
                activePartyMembers[i].DeductNeeds();
            }
        }
    }
}
