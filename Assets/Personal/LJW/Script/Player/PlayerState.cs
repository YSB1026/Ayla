using UnityEngine;

// �÷��̾� ���� Ŭ����
public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;
    private string animBoolName;
    protected float xInput;
    protected float yInput;

    // �ִϸ��̼� �̺�Ʈ(Trigger)�� ȣ��Ǿ����� ����
    protected bool triggerCalled;

    // ������: ���¸� ���� �� �÷��̾�, ���¸ӽ�, �ִϸ��̼� Bool �̸��� ����
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    // ���¿� ������ �� ����Ǵ� �Լ�
    public virtual void Enter()
    {
        // �ش� �ִϸ��̼� Bool �Ķ���͸� true�� �����Ͽ� �ִϸ��̼� ��ȯ
        player.anim.SetBool(animBoolName, true);

        // Rigidbody2D ���� ��������
        rb = player.rb;

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
        player.anim.SetBool(animBoolName, false);
    }

    // �ִϸ��̼ǿ��� Trigger �̺�Ʈ�� �߻����� �� ȣ���
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
