using UnityEngine;

public class AylaState : MonoBehaviour
{
    protected AylaStateMachine stateMachine;
    protected Ayla ayla;

    protected Rigidbody2D rb;
    private string animBoolName;
    protected float xInput;
    protected float yInput;

    // �ִϸ��̼� �̺�Ʈ(Trigger)�� ȣ��Ǿ����� ����
    protected bool triggerCalled;

    // ������: ���¸� ���� �� ���϶�, ���¸ӽ�, �ִϸ��̼� Bool �̸��� ����
    public AylaState(Ayla _ayla, AylaStateMachine _stateMachine, string _animBoolName)
    {
        this.ayla = _ayla;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    // ���¿� ������ �� ����Ǵ� �Լ�
    public virtual void Enter()
    {
        // �ش� �ִϸ��̼� Bool �Ķ���͸� true�� �����Ͽ� �ִϸ��̼� ��ȯ
        ayla.anim.SetBool(animBoolName, true);

        // Rigidbody2D ���� ��������
        rb = ayla.rb;

        // �ִϸ��̼� �̺�Ʈ ȣ�� ���� �ʱ�ȭ
        triggerCalled = false;
    }

    // ���� ���� �� �� �����Ӹ��� ����Ǵ� �Լ�
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    // ���¿��� ���� �� ȣ��Ǵ� �Լ�
    public virtual void Exit()
    {
        // �ش� �ִϸ��̼� Bool �Ķ���͸� false�� �ǵ���
        ayla.anim.SetBool(animBoolName, false);
    }

    // �ִϸ��̼ǿ��� Trigger �̺�Ʈ�� �߻����� �� ȣ���
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
