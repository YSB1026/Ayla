using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;

    public FlagDefinitionSO flagDefinitions;

    private Dictionary<string, bool> runtimeFlags = new Dictionary<string, bool>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeFlags();
    }

    void InitializeFlags()
    {
        runtimeFlags.Clear();

        foreach (var entry in flagDefinitions.flags)
        {
            if (!runtimeFlags.ContainsKey(entry.flagID))
                runtimeFlags.Add(entry.flagID, entry.defaultValue);
        }
    }

    public bool GetFlag(string flagID)
    {
        return runtimeFlags.ContainsKey(flagID) && runtimeFlags[flagID];
    }

    public void SetFlag(string flagID, bool value)
    {
        if (!runtimeFlags.ContainsKey(flagID))
        {
            Debug.LogWarning($"Flag '{flagID}' is not defined.");
            return;
        }
        runtimeFlags[flagID] = value;
    }

    public Dictionary<string, bool> GetAllFlags()
    {
        return new Dictionary<string, bool>(runtimeFlags);
    }

    public void LoadFlags(Dictionary<string, bool> saved)
    {
        foreach (var kvp in saved)
        {
            if (runtimeFlags.ContainsKey(kvp.Key))
                runtimeFlags[kvp.Key] = kvp.Value;
        }
    }
}
