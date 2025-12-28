using UnityEngine;

[CreateAssetMenu(fileName = "GimmickDefinition", menuName = "Game/Gimmick Definition")]
public class GimmickDefinitionSO : ScriptableObject
{
    [Header("조건 플래그")]
    public string[] requiredFlags; 
    public bool requireAll = true;

    [Header("상호작용 시 활성화될 플래그 (optional)")]
    public string[] activateFlags;

    [Header("상호작용 시 비활성화될 플래그들 (optional)")]
    public string[] deactivateFlags;
}
