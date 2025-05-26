using System.Data;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//SitState 안 쓸 예정
public class Player_SitState : PlayerState
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

        CheckAnimationTrasition();

        #region Not used
        //if (isCrawled)
        //{
        //    stateMachine.ChangeState(player.crawlState);
        //}
        //else
        //{
        //    stateMachine.ChangeState(player.standState);
        //}
        //if (xInput != 0)
        //    stateMachine.ChangeState(player.sitWalkState);

        //if (Input.GetKeyDown(KeyCode.W))
        //stateMachine.ChangeState(player.standState);

        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //    stateMachine.ChangeState(player.crawlState);
        #endregion
    }

    public override void Exit()
    {
        base.Exit();
        player.SetIdleCollider();
	}
    
    void CheckAnimationTrasition()
    {
        AnimatorStateInfo state = player.anim.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1f)
        {
            stateMachine.ChangeState(isCrawled ? player.crawlState : player.standState);
        }
    }
}
