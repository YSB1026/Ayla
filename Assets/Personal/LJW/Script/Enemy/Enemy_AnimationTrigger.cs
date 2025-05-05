using UnityEngine;

public class Enemy_AnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponent<Enemy>();
    private void AnimationEndTrigger()
    {
        enemy.stateMachine.currentState.AnimationEndTrigger();
    }

    public void Hit_Player()
    {
        enemy.HitPlayer();
    }
}
