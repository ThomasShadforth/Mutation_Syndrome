using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalvageObjects : MonoBehaviour
{

    public bool isInteractedWith;
    public string[] itemsGiven;
    public int[] itemNumbers;

    public float salvageTimer;
    float salvageTime;
    public bool countdownActive;

    public bool inArea;

    public Image timerImage;
    public GameObject itemDropNotif;

    ObjectID objectID;

    // Start is called before the first frame update
    void Start()
    {
        /*int arrayLength = Random.Range(1, 6);
        itemsGiven = new string[arrayLength];
        itemNumbers = new int[arrayLength];*/

        for(int i = 0; i < itemsGiven.Length; i++)
        {
            RandomiseItemNames(i);
            RandomiseItemNumbers(i);
        }

        salvageTime = salvageTimer;
        objectID = GetComponent<ObjectID>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (inArea)
        {
            if(Input.GetKeyDown(KeyCode.Q) && !countdownActive && !isInteractedWith)
            {
                ActivateSalvage();
            }
        }


        if (countdownActive)
        {
            //if pressing any key while the countdown is active
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                countdownActive = false;
                salvageTime = salvageTimer;
                timerImage.gameObject.SetActive(false);
            }

            if(salvageTime > 0)
            {
                //Note: Will only require slight tweaks if adding the ability to make salvage faster
                salvageTime -= 1 * GamePause.deltaTime;


                if(salvageTime <= 0)
                {
                    salvageTime = 0;
                    SalvageRewards();
                }

                SetTimerImage();
            }
        }
    }

    public void LoadSalvage()
    {
        isInteractedWith = true;
        timerImage.gameObject.SetActive(false);
    }

    void ActivateSalvage()
    {
        countdownActive = true;
        timerImage.gameObject.SetActive(true);
        
    }

    void SetTimerImage()
    {
        timerImage.fillAmount = salvageTime / salvageTimer;
    }

    void SalvageRewards()
    {
        for(int i = 0; i < itemsGiven.Length; i++)
        {
            for(int j = itemNumbers[i]; j > 0; j--)
            {
                ItemManager.instance.AddItem(itemsGiven[i]);
            }
        }

        GameObject notif = Instantiate(itemDropNotif, transform.position, transform.rotation);
        notif.GetComponent<SalvageItemNotif>().SetNotifText(itemsGiven, itemNumbers);

        isInteractedWith = true;
        objectID.data.interactedWith = isInteractedWith;
        ObjectManager.instance.UpdateObjectStates(objectID);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            inArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            inArea = false;
        }
    }

    void RandomiseItemNames(int indexToCheck)
    {
        bool hasSelectedItem = false;

        while (!hasSelectedItem)
        {
            string nameToUse = ItemManager.instance.itemDatabase[Random.Range(0, ItemManager.instance.itemDatabase.Length)].itemName;

            bool noMatchingItem = true;

            for(int i = 0; i < itemsGiven.Length; i++)
            {
                if(itemsGiven[i] == nameToUse)
                {
                    noMatchingItem = false;
                }
            }

            if (noMatchingItem)
            {
                itemsGiven[indexToCheck] = nameToUse;
                hasSelectedItem = true;
            }
        }
    }

    private void RandomiseItemNumbers(int indexToCheck)
    {
        itemNumbers[indexToCheck] = Random.Range(0, 4);
    }
}
