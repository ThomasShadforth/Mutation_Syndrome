using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public string characterName;

    public int Strength, Constitution, Wisdom, Intelligence, Dexterity, Charisma;
    public float HP, MaxHP;

    public NeedsStatus[] needs = new NeedsStatus[6];
    public string[] needNames = new string[6] {"Hunger", "Energy", "Thirst", "Happiness", "Social", "Hygiene" };
    
    public int Infection;

    public string afflictedBy;

    public string equippedWeapon;
    public int wepPow;
    public int totalArmourDef;
    public ArmourSlot[] equippedArmour;
    void Start()
    {
        for(int i = 0; i < needs.Length; i++)
        {
            InitialiseNeeds(i, needNames[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateDefense()
    {
        totalArmourDef = 0;
        for(int i = 0; i < equippedArmour.Length; i++)
        {
            if(equippedArmour[i].ArmourName != "")
            {
                totalArmourDef += equippedArmour[i].ArmourStrength;
            }
        }
    }

    void InitialiseNeeds(int index, string needName)
    {
        needs[index].needName = needName;
        needs[index].needNumber = 100f;
    }
}
