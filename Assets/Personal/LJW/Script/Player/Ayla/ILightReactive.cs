using UnityEngine;

public interface ILightReactive
{
    public void ApplyLightReaction();

    // 빛 안에 있는지 확인하고 싶은 경우
    public bool IsInLight { get; set; }
}
