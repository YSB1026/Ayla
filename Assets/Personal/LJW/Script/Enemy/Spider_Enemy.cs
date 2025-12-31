using UnityEngine;
using UnityEngine.AI;

public class Spider_Enemy : MonoBehaviour
{
    [Header("감지 설정")]
    public float detectionRange = 10f;      // 적 감지 범위
    public float attackRange = 2f;          // 공격 범위
    public LayerMask targetLayer;           // 적 레이어
    
    [Header("순찰 설정")]
    public float patrolSpeed = 2f;          // 순찰 속도
    public float patrolRadius = 10f;        // 순찰 반경
    public float patrolWaitTime = 2f;       // 순찰 대기 시간
    
    [Header("추적 설정")]
    public float chaseSpeed = 4f;           // 추적 속도
    
    [Header("공격 설정")]
    public float attackCooldown = 2f;       // 공격 쿨다운
    public int attackDamage = 10;           // 공격 데미지
    
    // 컴포넌트
    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;
    
    // 상태
    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;
    
    // 타이머
    private float patrolTimer;
    private float attackTimer;
    
    // 애니메이션 파라미터 이름
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agent.speed = patrolSpeed;
        SetNewPatrolDestination();
    }
    
    void Update()
    {
        // 적 감지
        target = DetectTarget();
        
        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
            case State.Attack:
                AttackBehavior();
                break;
        }
        
        // 애니메이션 업데이트
        UpdateAnimation();
    }
    
    // 적 감지
    Transform DetectTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
        
        if (hits.Length > 0)
        {
            // 가장 가까운 적 찾기
            Transform closest = null;
            float minDistance = Mathf.Infinity;
            
            foreach (Collider hit in hits)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = hit.transform;
                }
            }
            
            return closest;
        }
        
        return null;
    }
    
    // 순찰 행동
    void PatrolBehavior()
    {
        agent.speed = patrolSpeed;
        
        // 적 발견 시 추적 상태로 전환
        if (target != null)
        {
            currentState = State.Chase;
            return;
        }
        
        // 목적지 도착 시 대기 후 새로운 목적지 설정
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            
            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolDestination();
                patrolTimer = 0f;
            }
        }
    }
    
    // 추적 행동
    void ChaseBehavior()
    {
        agent.speed = chaseSpeed;
        
        // 적을 잃어버린 경우
        if (target == null)
        {
            currentState = State.Patrol;
            SetNewPatrolDestination();
            return;
        }
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        // 공격 범위 내 진입
        if (distanceToTarget <= attackRange)
        {
            currentState = State.Attack;
            agent.ResetPath();
        }
        else
        {
            // 적 추적
            agent.SetDestination(target.position);
        }
    }
    
    // 공격 행동
    void AttackBehavior()
    {
        // 적을 잃어버린 경우
        if (target == null)
        {
            currentState = State.Patrol;
            SetNewPatrolDestination();
            return;
        }
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        // 적이 공격 범위를 벗어난 경우
        if (distanceToTarget > attackRange)
        {
            currentState = State.Chase;
            return;
        }
        
        // 적을 바라보기
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 5f
            );
        }
        
        // 공격 쿨다운
        attackTimer += Time.deltaTime;
        
        if (attackTimer >= attackCooldown)
        {
            PerformAttack();
            attackTimer = 0f;
        }
    }
    
    // 공격 실행
    void PerformAttack()
    {
        animator.SetTrigger(hashAttack);
        
        // 적에게 데미지 주기 (적이 IDamageable 인터페이스를 구현한 경우)
        if (target != null)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }
    
    // 새로운 순찰 목적지 설정
    void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    // 애니메이션 업데이트
    void UpdateAnimation()
    {
        // 이동 속도에 따라 애니메이션 속도 조절
        float speed = agent.velocity.magnitude;
        animator.SetFloat(hashSpeed, speed);
    }
    
    // 디버그용 Gizmos
    void OnDrawGizmosSelected()
    {
        // 감지 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // 순찰 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}

// 데미지를 받을 수 있는 인터페이스 (별도 파일로 생성)
public interface IDamageable
{
    void TakeDamage(int damage);
}