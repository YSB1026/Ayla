using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    protected Rigidbody2D rb;
    private string animBoolName;

    protected float xInput;
    protected float yInput;

    // 생성자: 상태를 만들 때 플레이어, 상태머신, 애니메이션 Bool 이름을 설정
    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    // 상태에 진입할 때 실행되는 함수
    public virtual void Enter()
    {
        // 해당 애니메이션 Bool 파라미터를 true로 설정하여 애니메이션 전환
        enemy.anim.SetBool(animBoolName, true);

        // Rigidbody2D 참조 가져오기
        rb = enemy.rb;
    }

    // 상태 유지 중 매 프레임마다 실행되는 함수
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    // 상태에서 나갈 때 호출되는 함수
    public virtual void Exit()
    {
        // 해당 애니메이션 Bool 파라미터를 false로 되돌림
        enemy.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationEndTrigger()
    {
        
    }
}
