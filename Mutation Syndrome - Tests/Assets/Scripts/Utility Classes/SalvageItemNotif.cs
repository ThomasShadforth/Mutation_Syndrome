using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SalvageItemNotif : MonoBehaviour
{
    public TextMeshProUGUI itemText;
    public float moveSpeed;
    public float lifeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);

        transform.position += new Vector3(0, moveSpeed * GamePause.deltaTime, 0);
    }

    public void SetNotifText(string[] itemsGiven, int[] itemNumbers)
    {
        itemText.text = "";
        for(int i = 0; i < itemsGiven.Length; i++)
        {
            itemText.text = itemText.text + itemsGiven[i] + " x " + itemNumbers[i].ToString();

            if(i < itemsGiven.Length - 1)
            {
                itemText.text = itemText.text + "\n";
            }
        }
    }
}
