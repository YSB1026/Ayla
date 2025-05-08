using UnityEngine;

public class TimeLineTrigger : BaseTrigger
{
    [SerializeField] GameObject timeLine;
    [SerializeField] Player player;

    private void OnEnable()
    {
        if(timeLine.activeSelf) timeLine.SetActive(false);
    }
    protected override void OnPlayerEnter()
    {
        player.SetControlEnabled(false);
        player.SetZeroVelocity();
        timeLine.SetActive(true);
    }
}
