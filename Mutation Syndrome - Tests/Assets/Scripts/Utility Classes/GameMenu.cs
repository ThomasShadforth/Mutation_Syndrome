using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public static GameMenu instance;
    public GameObject menu;

    public GameObject[] menuWindows;

    [Header("Character Info")]
    public GameObject[] charInfoPanels;
    public Text[] nameInfoText;
    public Text[] HPInfoText;
    public Image[] charImages;

    Item selectedItem;
    [Header("Item Menu Values")]
    public ItemButton[] itemButtons;
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text useButtonText;

    public GameObject characterSelectScreen;
    public Text[] characterChoiceNames;

    [Header("Status Screen")]
    public GameObject[] statusButtons;
    public Image characterImage;
    public Text nameText;
    public Text HPText;
    public Text strText;
    public Text dexText;
    public Text conText;
    public Text intText;
    public Text wisText;
    public Text charText;
    public Text weaponText;
    public Text wepPowText;
    public Text armPowText;
    public Image[] needsMeterImages;
    public Text[] needsMeterText;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (menu.activeInHierarchy)
            {
                //close the menu (Create a method for this)
                CloseMenu();
            }
            else
            {
                menu.SetActive(true);
                //Create method to update main stats page
                GameManager.instance.isGameMenuOpen = true;
                GamePause.GamePaused = true;
                UpdateMainStats();
            }
        }
    }

    public void CloseMenu()
    {
        for(int i = 0; i < menuWindows.Length; i++)
        {
            menuWindows[i].SetActive(false);
        }

        menu.SetActive(false);
        GameManager.instance.isGameMenuOpen = false;
        GamePause.GamePaused = false;
        CloseItemChoiceMenu();
    }

    public void ToggleWindowOpen(int windowNum)
    {
        for(int i = 0; i < menuWindows.Length; i++)
        {
            if(i == windowNum)
            {
                menuWindows[windowNum].SetActive(!menuWindows[windowNum].activeInHierarchy);
            }
            else
            {
                menuWindows[i].SetActive(false);
            }
        }
    }

    public void UpdateMainStats()
    {
        for(int i = 0; i < charInfoPanels.Length; i++)
        {
            if(GameManager.instance.activePartyMembers[i] != null)
            {
                charInfoPanels[i].SetActive(true);
                nameInfoText[i].text = GameManager.instance.activePartyMembers[i].characterName;
                HPInfoText[i].text = "HP: " + GameManager.instance.activePartyMembers[i].HP + " / " + GameManager.instance.activePartyMembers[i].MaxHP;
            }
            else
            {
                charInfoPanels[i].SetActive(false);
            }
        }
    }

    public void OpenItemWindow()
    {
        itemButtons[0].Press();
    }


    public void ShowItems()
    {
        ItemManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonIndex = i;

            if(ItemManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = ItemManager.instance.GetItemDetails(ItemManager.instance.itemsHeld[i]).itemSprite;
                //Set the amount text
                itemButtons[i].amountText.text = ItemManager.instance.itemNumbers[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
                //Set amount text to blank
            }
        }
    }

    public void SelectInventoryItem(Item newItem)
    {
        selectedItem = newItem;

        if (selectedItem.isItem)
        {
            //Set use button text to use
            useButtonText.text = "Use";
        }

        if(selectedItem.isArmour || selectedItem.isWeapon)
        {
            useButtonText.text = "Equip";
        }

        itemNameText.text = selectedItem.itemName;
        itemDescriptionText.text = selectedItem.itemDescription;

        //Set the itemName text and description text
    }

    public void PressUseButton()
    {
        OpenItemChoiceMenu();
    }

    public void OpenItemChoiceMenu()
    {
        characterSelectScreen.SetActive(true);

        for(int i = 0; i < characterChoiceNames.Length; i++)
        {
            if(GameManager.instance.activePartyMembers[i] != null)
            {
                characterChoiceNames[i].text = GameManager.instance.activePartyMembers[i].characterName;
                characterChoiceNames[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                characterChoiceNames[i].text = "";
                characterChoiceNames[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void CloseItemChoiceMenu()
    {
        characterSelectScreen.SetActive(false);
    }

    public void ConsumeItem(int partyMemberIndex)
    {
        selectedItem.UseItem(partyMemberIndex);
        ShowItems();
        CloseItemChoiceMenu();
    }

    public void PressDiscardButton()
    {
        ItemManager.instance.RemoveItem(selectedItem.itemName);
        ShowItems();
    }

    public void OpenStatusMenu()
    {
        for(int i = 0; i < statusButtons.Length; i++)
        {
            if(GameManager.instance.activePartyMembers[i] != null)
            {
                statusButtons[i].SetActive(true);
            }
            else
            {
                statusButtons[i].SetActive(false);
            }
        }

        PressStatusButton(0);
    }

    public void PressStatusButton(int charIndex)
    {
        nameText.text = GameManager.instance.activePartyMembers[charIndex].characterName;
        HPText.text = GameManager.instance.activePartyMembers[charIndex].HP.ToString() + " / " + GameManager.instance.activePartyMembers[charIndex].MaxHP;
        strText.text = GameManager.instance.activePartyMembers[charIndex].Strength.ToString();
        dexText.text = GameManager.instance.activePartyMembers[charIndex].Dexterity.ToString();
        conText.text = GameManager.instance.activePartyMembers[charIndex].Constitution.ToString();
        intText.text = GameManager.instance.activePartyMembers[charIndex].Intelligence.ToString();
        wisText.text = GameManager.instance.activePartyMembers[charIndex].Wisdom.ToString();
        charText.text = GameManager.instance.activePartyMembers[charIndex].Charisma.ToString();
        weaponText.text = GameManager.instance.activePartyMembers[charIndex].equippedWeapon;
        wepPowText.text = GameManager.instance.activePartyMembers[charIndex].wepPow.ToString();
        armPowText.text = GameManager.instance.activePartyMembers[charIndex].totalArmourDef.ToString();

        for(int i = 0; i < needsMeterImages.Length; i++)
        {
            needsMeterText[i].text = GameManager.instance.activePartyMembers[charIndex].needNames[i] + ": " + GameManager.instance.activePartyMembers[charIndex].needs[i].needNumber + " / " + GameManager.instance.activePartyMembers[charIndex].needs[i].maxNeedMeter;
            needsMeterImages[i].fillAmount = GameManager.instance.activePartyMembers[charIndex].needs[i].needNumber / GameManager.instance.activePartyMembers[charIndex].needs[i].maxNeedMeter;
        }
    }
}
