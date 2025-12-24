using UnityEngine;

public class ShadowState
{
    protected ShadowStateMachine stateMachine;

    protected Shadow shadow;

    protected Rigidbody2D rb;
    protected string animBoolName { get; private set; }

    protected float xInput;
    protected float yInput;

    protected bool triggerCalled;
    protected RaycastHit2D hit;
    public ShadowState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName)
    {
        this.shadow = _shadow;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        shadow.anim.SetBool(animBoolName, true);

        rb = shadow.rb;

        triggerCalled = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        shadow.anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public virtual void Exit()
    {
        shadow.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    // [주석 처리] 벽 체크 기능
    // Shadow 스크립트에 IsWallDetected() 함수가 있다면 주석을 풀고 쓰시면 됩니다.
    /*
    protected bool IsBlockedByWall()
    {
        return shadow.IsWallDetected() && shadow.facingDir == xInput;
    }
    */
}