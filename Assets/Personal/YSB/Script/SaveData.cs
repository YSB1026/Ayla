using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using static LightColorController;

[System.Serializable]
public class SaveData
{
    public Vector3 savePoint;
    public ColorOption pendantColor;
    public string currentScene;
    public HashSet<GameObject> clearedTimelines = new HashSet<GameObject>();
}
