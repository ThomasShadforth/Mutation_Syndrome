using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NeedsObjectNotif : MonoBehaviour
{
    public TextMeshProUGUI notifText;
    public float moveSpeed;
    public float destroyTime;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, destroyTime);
        transform.position += new Vector3(0, moveSpeed * GamePause.deltaTime, 0);
    }

    public void setNotifText(string posNeed, string negNeed, float posNeedVal, float negNeedVal)
    {
        notifText.text = "";
        notifText.text = posNeed + ": +" + posNeedVal + "\n";
        notifText.text = notifText.text + negNeed + ": -" + negNeedVal;
    }
}
