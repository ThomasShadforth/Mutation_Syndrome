using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    public IDictionary<string, IDictionary<string, ObjectData>> objectsRegistry = new Dictionary<string, IDictionary<string, ObjectData>>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            /*foreach(var(key, value) in objectsRegistry.Select(x => (x.Key, x.Value)))
            {
                foreach(var(ID, ObjectVal) in value.Select(y => (y.Key, y.Value)))
                {
                    if(ObjectVal != null)
                    {
                        Debug.Log(ObjectVal.name);
                    }
                    else
                    {
                        Debug.Log("IT EXISTS, IT JUST ISN'T AN OBJECT");
                    }
                    
                }
            }*/
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RegisterObj(ObjectID objectID, ObjectData objectEntity)
    {
        IDictionary<string, ObjectData> objList;
        if (!objectsRegistry.ContainsKey(SceneManager.GetActiveScene().name))
        {
            objectsRegistry[SceneManager.GetActiveScene().name] = new Dictionary<string, ObjectData>();
            objList = new Dictionary<string, ObjectData>();
            objectsRegistry[SceneManager.GetActiveScene().name] = objList;
        }
        else
        {
            objList = objectsRegistry[SceneManager.GetActiveScene().name];
        }

        if(objList.Count != 0)
        {
            if (!objList.ContainsKey(objectID.objectID))
            {
                Debug.Log("DOES NOT EXIST");
                objList.Add(new KeyValuePair<string, ObjectData>(objectID.objectID, objectEntity));
            }
            else
            {
                LoadObjectStates(objectID);
                Debug.Log("ALREADY EXISTS");
            }
        }
        else
        {
            Debug.Log("FIRST ENTRY");
            objList.Add(new KeyValuePair<string, ObjectData>(objectID.objectID, objectEntity));
        }
    }

    public void SaveObjectStates()
    {
        foreach(var(Key, Value) in objectsRegistry.Select(x => (x.Key, x.Value))){
            if(SceneManager.GetActiveScene().name == Key)
            {
                foreach(var(IDKey, ObjVal) in Value.Select(y => (y.Key, y.Value)))
                {
                    
                }
            }
        }
    }

    public void LoadObjectStates(ObjectID objID)
    {
        ObjectData theData = objectsRegistry[SceneManager.GetActiveScene().name][objID.objectID];
        objID.LoadInformation(theData);
    }

    public void UpdateObjectStates(ObjectID objID)
    {
        objectsRegistry[SceneManager.GetActiveScene().name][objID.objectID] = objID.data;
    }

}
