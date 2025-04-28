using UnityEngine;

public class Enemy : Entity
{
    [Header("이동 정보")]
    public float moveSpeed = 12f;
    public float runSpeed;
    public float jumpForce;
    public float crawlSpeed;
    public float sitWalkSpeed;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    { 
        
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
