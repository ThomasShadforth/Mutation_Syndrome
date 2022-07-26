using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct ObjectData
{
    public bool isSalvage;
    public bool interactedWith;
    public Vector3 position;
}

public class ObjectID : MonoBehaviour
{
    public string objectID;
    public ObjectData data;

    // Start is called before the first frame update

    void OnEnable()
    {
        
    }

    private void Awake()
    {
        data.position = transform.position;

        //SceneManager.sceneLoaded += OnSceneLoaded;
        if (!FindObjectOfType<ObjectManager>())
        {
            Debug.Log("DOESNT EXIST YET");
        }
        else
        {
            Debug.Log("EXISTS");
            ObjectManager.instance.RegisterObj(this, data);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadInformation(ObjectData dataToLoad)
    {
        data = dataToLoad;
        if (GetComponent<SalvageObjects>())
        {
            GetComponent<SalvageObjects>().LoadSalvage();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ObjectManager.instance.RegisterObj(this, data);
    }

}
