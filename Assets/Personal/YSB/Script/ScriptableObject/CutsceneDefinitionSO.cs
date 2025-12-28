using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneDefinition", menuName = "Game/Cutscene Definition")]
public class CutsceneDefinitionSO : ScriptableObject
{
    public string cutsceneID;

    [Header("컷씬 재생 조건")]
    public string[] requiredFlags;
    public bool requireAll = true;

    [Header("한 번만 재생")]
    public bool playOnce = true;
    public string playedFlag;   // 컷씬 재생 후 true로 변경
}