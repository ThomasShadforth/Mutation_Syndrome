using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetMenuButton : MonoBehaviour
{
    public string actionName;
    public int enemyBattlerTarget;
    public TextMeshProUGUI targetName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        
        BattleManager.instance.SelectTarget(enemyBattlerTarget);
    }
}
