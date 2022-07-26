using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeedsStatus
{
    public string needName;
    public float needNumber;
    public float maxNeedMeter = 100f;
    public float depleteRate = 1f;
}
