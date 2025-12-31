using UnityEngine;

public class Enemy_Light_AttackState : EnemyState
{
    private Enemy_Light enemyLight;
    private bool attackSuccess = false;
    
    private float attackTimer;
    private float attackDuration = 1f; // 공격 애니메이션 지속 시간
    private bool hasAttacked = false;
    
    public Enemy_Light_AttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Light _enemylight) 
        : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyLight = _enemylight;
    }

    public override void Enter()
    {
        base.Enter();
        
        attackSuccess = false;
        attackTimer = 0f;
        hasAttacked = false;
        
        // 모든 움직임 완전히 정지
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.SetZeroVelocity();
        
        // 공격 애니메이션 직접 트리거
        enemy.anim.SetBool("Move", false);
        enemy.anim.SetBool("Idle", false);
        enemy.anim.SetBool("Attack", true);
        
        Debug.Log("공격 시작!");
    }

    public override void Update()
    {
        base.Update();
        
        // 공격 중에는 계속 정지 상태 유지
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.SetZeroVelocity();

        if (enemyLight.isInLight)
        {
            return;
        }

        attackTimer += Time.deltaTime;
        
        // 공격 중간 지점에서 실제 공격 실행
        if (!hasAttacked && attackTimer >= attackDuration * 0.5f)
        {
            hasAttacked = true;
            enemy.HitPlayer();
            Debug.Log("공격 실행!");
        }
        
        // 공격 애니메이션이 끝나면 MoveState로 복귀
        if (attackTimer >= attackDuration)
        {
            stateMachine.ChangeState(enemyLight.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("Attack", false);
        // 종료 시에도 velocity 초기화
        enemy.rb.linearVelocity = Vector2.zero;
        Debug.Log("AttackState 종료");
    }

    public void SetAttackSuccess(bool success)
    {
        attackSuccess = success;
    }
}