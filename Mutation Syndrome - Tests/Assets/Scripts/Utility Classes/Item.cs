using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //Boolean flags - what type of item it is
    public bool isItem;
    public bool isArmour;
    public bool isWeapon;

    //General values - Name, desc, etc.
    [Header("General Values")]
    public string itemName;
    public string itemDescription;
    public int itemValue;
    public Sprite itemSprite;

    //Item details
    [Header("Item Details")]
    public int valueChange;
    public bool affectHP;
    public bool affectNeed;
    public CharacterNeeds[] needs;
    public bool doesTreatEffect;
    public string effectTreated;

    //Weapon/Armour Values
    [Header("Weapon and Armour Values")]
    public string armourType;
    public string weaponType;

    public int armourStrength;
    public int weaponStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseItem(int indexToUseOn)
    {
        CharacterStatus selectedChar = GameManager.instance.allCharacters[indexToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.HP += valueChange;
                if(selectedChar.HP > selectedChar.MaxHP)
                {
                    selectedChar.HP = selectedChar.MaxHP;
                }
            }
            if (doesTreatEffect)
            {
                if(effectTreated == selectedChar.afflictedBy)
                {
                    selectedChar.afflictedBy = "";
                }
            }
        }

        if (isWeapon)
        {
            //Check to see if there is no weapon equipped. If a weapon is equipped, add it to the inventory
            if(selectedChar.equippedWeapon != "")
            {
                ItemManager.instance.AddItem(selectedChar.equippedWeapon);
            }

            selectedChar.equippedWeapon = itemName;
            
            //And set equipped weapon
            //Set character's weapon power accordingly
        }

        if (isArmour)
        {
            for(int i = 0; i < selectedChar.equippedArmour.Length; i++)
            {
                if(selectedChar.equippedArmour[i].ArmourType == armourType)
                {
                    if(selectedChar.equippedArmour[i].ArmourName != "")
                    {
                        ItemManager.instance.AddItem(selectedChar.equippedArmour[i].ArmourName);
                    }

                    selectedChar.equippedArmour[i].ArmourName = itemName;
                    selectedChar.equippedArmour[i].ArmourStrength = armourStrength;
                    //Note: create a method to recalculate total defense whenever a new piece of armour is equipped
                }
            }
            //Same applies to armour depending on slot
            //Set armour power accordingly
        }

        ItemManager.instance.RemoveItem(itemName);
    }

    void AffectNeed(CharacterStatus selectedChar)
    {
        for(int i = 0; i < needs.Length; i++)
        {
            if (needs[i].affected)
            {
                if (needs[i].affectedPos)
                {
                    for(int j = 0; j < selectedChar.needs.Length; j++) {
                        if (needs[i].needName == selectedChar.needs[j].needName)
                        {
                            selectedChar.needs[j].needNumber += valueChange;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < selectedChar.needs.Length; j++)
                    {
                        if (needs[i].needName == selectedChar.needs[j].needName)
                        {
                            selectedChar.needs[j].needNumber -= valueChange;
                        }
                    }
                }
            }
        }
    }
}
