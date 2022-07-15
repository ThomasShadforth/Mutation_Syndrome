using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public string[] itemsHeld;
    public int[] itemNumbers;

    public Item[] itemDatabase;

    public static ItemManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item GetItemDetails(string itemToReference)
    {
        for(int i = 0; i < itemDatabase.Length; i++)
        {
            if(itemDatabase[i].itemName == itemToReference)
            {
                return itemDatabase[i];
            }
        }
        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            for(int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if(itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    itemNumbers[i] = itemNumbers[i + 1];
                    itemNumbers[i + 1] = 0;

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string nameOfItem)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == "" || itemsHeld[i] == nameOfItem)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;

            for(int i = 0; i < itemDatabase.Length; i++)
            {
                if(itemDatabase[i].itemName == nameOfItem)
                {
                    itemExists = true;
                    i = itemDatabase.Length;
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = nameOfItem;
                itemNumbers[newItemPosition]++;
            }
            else
            {

            }
        }
    }
    
    public void RemoveItem(string nameOfItem)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == nameOfItem)
            {
                foundItem = true;
                itemPosition = i;
                i = itemsHeld.Length;
            }
        }

        if (foundItem)
        {
            itemNumbers[itemPosition]--;

            if (itemNumbers[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }
        }
        else
        {

        }
    }
}
