using System.Data;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Player_SitState : Player_GroundedState
{
    bool isCrawled = false;
    
    public Player_SitState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        player.SetSitCollider();
        isCrawled = !isCrawled;
    }

    public override void Update()
    {
        base.Update();

        CheckAnimationTransition();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetIdleCollider();
        player.anim.speed = 1;
	}
    
    void CheckAnimationTransition()
    {
        AnimatorStateInfo state = player.anim.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1f)
        {
            stateMachine.ChangeState(isCrawled ? player.crawlState : player.standState);
        }
    }
}
