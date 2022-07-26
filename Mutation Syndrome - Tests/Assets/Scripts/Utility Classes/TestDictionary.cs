using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDictionary : MonoBehaviour
{
    [SerializeField]
    List<string> keys = new List<string>();
    [SerializeField]
    List<int> values = new List<int>();

    Dictionary<string, int> myDictionary = new Dictionary<string, int>();
}
