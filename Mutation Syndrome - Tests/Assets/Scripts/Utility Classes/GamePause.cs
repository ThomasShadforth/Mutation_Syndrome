using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePause
{
    public static bool GamePaused;
    public static float deltaTime { get { return GamePaused ? 0 : Time.deltaTime; } }
}
