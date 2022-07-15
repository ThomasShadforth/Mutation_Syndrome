using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleChar : BattleCharacter
{
    //Boolean flags for status effects:
    public StatusAilments[] ailments;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkForAilment(string ailmentToCheckFor)
    {
        foreach(StatusAilments ailment in ailments)
        {
            if(ailment.AilmentName == ailmentToCheckFor)
            {
                //
            }
        }
    }

    
}
