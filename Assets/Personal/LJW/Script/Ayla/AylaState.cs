using UnityEngine;

public class AylaState : MonoBehaviour
{
    protected AylaStateMachine stateMachine;
    protected Ayla ayla;

    protected Rigidbody2D rb;
    private string animBoolName;
    protected float xInput;
    protected float yInput;

    // 애니메이션 이벤트(Trigger)가 호출되었는지 여부
    protected bool triggerCalled;

    // 생성자: 상태를 만들 때 에일라, 상태머신, 애니메이션 Bool 이름을 설정
    public AylaState(Ayla _ayla, AylaStateMachine _stateMachine, string _animBoolName)
    {
        this.ayla = _ayla;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    // 상태에 진입할 때 실행되는 함수
    public virtual void Enter()
    {
        // 해당 애니메이션 Bool 파라미터를 true로 설정하여 애니메이션 전환
        ayla.anim.SetBool(animBoolName, true);

        // Rigidbody2D 참조 가져오기
        rb = ayla.rb;

        // 애니메이션 이벤트 호출 여부 초기화
        triggerCalled = false;
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
        ayla.anim.SetBool(animBoolName, false);
    }

    // 애니메이션에서 Trigger 이벤트가 발생했을 때 호출됨
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
