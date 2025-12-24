/*using UnityEngine;

public class Player_PullState : PlayerState
{
	public Player_PullState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
		: base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		hit = player.GetObjectHitInfo();
		hit.collider.gameObject.GetComponent<InteractiveObject>()?.FreezeObject(false);
	}

	public override void Update()
	{
		base.Update();

		hit.collider.gameObject.GetComponent<InteractiveObject>()?.MoveObject(-player.facingDir);
		player.transform.position += new Vector3(player.grabSpeed * -player.facingDir * Time.deltaTime, 0, 0);

		if (Input.GetKeyDown(KeyCode.F) || !player.IsObjectDetected())
			stateMachine.ChangeState(player.inputState);
		else if (xInput > 0 && player.facingDir == 1)
			stateMachine.ChangeState(player.pushState);
		else if (xInput < 0 && player.facingDir == -1)
			stateMachine.ChangeState(player.pushState);
		else if (xInput == 0)
			stateMachine.ChangeState(player.grabState);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetZeroVelocity();
		hit.collider.gameObject.GetComponent<InteractiveObject>()?.FreezeObject(true);
	}
}
*/