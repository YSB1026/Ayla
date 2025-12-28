using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using static LightColorController;

[Serializable]
public class SaveData
{
    public Vector3 savePoint;
    public ColorOption pendantColor;
    // 플래그 상태만 저장
    public Dictionary<string, bool> flags = new Dictionary<string, bool>();

    public int currentChapter = 0;
}