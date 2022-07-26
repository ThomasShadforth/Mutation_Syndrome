using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsObject : MonoBehaviour
{
    public string needIncreased;
    public string needDecreased;
    public float needValuePos;
    public float needValueNegative;

    [SerializeField]
    bool inRange;
    [SerializeField]
    bool interactedWith;

    [SerializeField]
    GameObject needNotif;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactedWith) {
            if (inRange && Input.GetMouseButtonDown(1))
            {
                ApplyNeedOutcome();
            }
        }
    }

    public void ApplyNeedOutcome()
    {
        for(int i = 0; i < GameManager.instance.activePartyMembers.Length; i++)
        {
            if(GameManager.instance.activePartyMembers[i] != null)
            {
                GameManager.instance.activePartyMembers[i].AffectNeed(needIncreased, needDecreased, needValuePos, needValueNegative);
            }
        }

        interactedWith = true;
        GameObject notif = Instantiate(needNotif, transform.position, transform.rotation);
        notif.GetComponent<NeedsObjectNotif>().setNotifText(needIncreased, needDecreased, needValuePos, needValueNegative);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            inRange = false;
        }
    }
}
