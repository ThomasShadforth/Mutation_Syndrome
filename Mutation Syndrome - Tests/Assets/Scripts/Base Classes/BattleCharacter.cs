using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    //Main Stats: HP, STR, DEX
    public string charName;
    public float maxHP, HP;
    public float damage;
    public int speed;
    public int wisdom;
    public int charisma;

    //Battle Values: Selected Move, Selected Target
    public string selectedMov;
    public int selectedTarget;

    //bool flags
    public bool shouldFade;
    public bool isPlayer;

    //Approach - have the known attacks set by the character status scripts (This will hold the moves the player characters currently know)
    public BattleMove[] knownAttacks;

    //Ailments - what the character is being afflicted by and if they are strong against any (Applies to the enemies themselves for now, but will likely be applied later to attacks and so on)
    public string afflictedBy = "";
    public string strongAgainst = "";

    public int ailmentTick;

    //Components
    SpriteRenderer spriteRender;


    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)
        {
            spriteRender.color = new Color(Mathf.MoveTowards(spriteRender.color.r, 1f, 1f), Mathf.MoveTowards(spriteRender.color.g, 0f, 1f), Mathf.MoveTowards(spriteRender.color.b, 0f, 1f), Mathf.MoveTowards(spriteRender.color.a, 0, 1f));

        }
    }

    public void SetFade()
    {
        shouldFade = true;
    }
}
