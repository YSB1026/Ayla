using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FlagDefinitions", menuName = "Game/Flag Definitions")]
public class FlagDefinitionSO : ScriptableObject
{
    public FlagEntry[] flags;
}

[Serializable]
public class FlagEntry
{
    public string flagID;
    public string description;
    public bool defaultValue;
}

[System.Serializable]
public class FlagSaveEntry
{
    public string flagID;
    public bool value;
}

[System.Serializable]
public class FlagSaveWrapper
{
    public FlagSaveEntry[] list;
}