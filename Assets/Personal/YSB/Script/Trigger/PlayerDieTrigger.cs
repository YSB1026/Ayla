using UnityEngine;

public class PlayerDieTrigger : BaseTrigger
{
    [SerializeField] Player player;
    protected override void OnPlayerEnter()
    {
        player.Die();
    }
}
