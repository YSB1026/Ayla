using UnityEngine;

public class Enemy_Light : Enemy
{
    [Header("추가 정보")]
    public Transform player;
    public LayerMask lightLayer;
    public float detectRadius = 0.5f;

    public bool isInLight { get; private set; }

    #region States
    public Enemy_Light_MoveState moveState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        moveState = new Enemy_Light_MoveState(this, stateMachine, "Move");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public void SetInLight(bool inLight)
    {
        isInLight = inLight;

        if (anim != null)
        {
            anim.speed = isInLight ? 0 : 1;
        }
    }
}
