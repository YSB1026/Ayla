using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Game/Ability Definition")]
public class AbilityDefinitionSO : ScriptableObject
{
    public string abilityID;

    [Header("능력 해금 조건")]
    public string[] requiredFlags;
    public bool requireAll = true;

    [Header("능력 해금 플래그")]
    public string unlockFlag;

    [Header("Green 능력 전용 게이지")]
    public bool useGauge = false;
    public float maxGauge = 100f;
}
