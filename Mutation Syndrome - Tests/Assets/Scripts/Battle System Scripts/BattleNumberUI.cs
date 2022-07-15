using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleNumberUI : MonoBehaviour
{
    public float lifeTime, moveSpeed;
    public int tickSpeed;
    float remainingDamage, initialDamage, damageVal;
    public Text damageNumberText;

    public float placementJitter = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        if (damageVal < initialDamage)
        {
            setDamageText();
        }
    }

    public void setInitialDamage(int damageDealt)
    {
        remainingDamage = damageDealt;
        initialDamage = remainingDamage;
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
        tickSpeed = damageDealt / (int)lifeTime;
    }

    void setDamageText()
    {
        damageNumberText.text = damageVal.ToString("0");
        damageVal += 1 * tickSpeed * Time.deltaTime;
        
    }
}
