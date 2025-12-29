using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void SaveGame()
    {
        var flagDict = FlagManager.Instance.GetAllFlags();
        FlagSaveWrapper wrapper = new FlagSaveWrapper();

        wrapper.list = new FlagSaveEntry[flagDict.Count];

        int i = 0;
        foreach (var kvp in flagDict)
        {
            wrapper.list[i] = new FlagSaveEntry
            {
                flagID = kvp.Key,
                value = kvp.Value
            };
            i++;
        }

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
            return;

        string json = File.ReadAllText(savePath);
        FlagSaveWrapper wrapper = JsonUtility.FromJson<FlagSaveWrapper>(json);

        Dictionary<string, bool> loaded = new Dictionary<string, bool>();

        foreach (var entry in wrapper.list)
            loaded[entry.flagID] = entry.value;

        FlagManager.Instance.LoadFlags(loaded);
    }
}
