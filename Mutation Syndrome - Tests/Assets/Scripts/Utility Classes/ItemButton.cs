using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public Text amountText;
    public int buttonIndex;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void Press()
    {
        if (GameMenu.instance.menu.activeInHierarchy)
        {
            if(ItemManager.instance.itemsHeld[buttonIndex] != "")
            {
                //Open the details panel with item information, value, and the use/discard buttons
                GameMenu.instance.SelectInventoryItem(ItemManager.instance.GetItemDetails(ItemManager.instance.itemsHeld[buttonIndex]));
            }
        }

        if (BattleManager.instance.battleActive)
        {
            //Update the item details panel in the battle menu
            BattleManager.instance.SetSelectedItem(ItemManager.instance.GetItemDetails(ItemManager.instance.itemsHeld[buttonIndex]));
        }
    }
}
