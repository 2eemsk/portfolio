using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Roaming,    // 배회 상태
    Chasing,    // 추적 상태
    Attacking,  // 공격 상태
    Hit         // 피격 상태
}

public class EnemyAI : MonoBehaviour
{
    [SerializeField] BoxCollider _boxCol;

    public float roamRadius = 10f;   // 배회 반경
    public float chaseRadius = 10f;  // 추적 반경
    public float attackRadius = 2f;  // 공격 반경
    public float attackDelay = 2f;   // 공격 딜레이
    public int damage = 1;          // 공격 데미지

    public int maxHP = 30; // 최대 체력
    private int currentHP; // 현재 체력

    private Animator anim;
    public Transform target;
    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.Roaming;
    private float originalSpeed;
    private float attackTimer;

    private bool isAttacking = false;
    private bool isDead = false;

    public Color hitColor = Color.red;  // 피격 시 사용할 색상
    public float hitFlashDuration = 0.1f;  // 피격 점멸 시간

    private Renderer enemyRenderer;  // 적 AI의 Renderer 컴포넌트
    private Color originalColor;     // 원래의 색상

    public BoxCollider BoxCol { get => _boxCol; set => _boxCol = value; }

    private void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        attackTimer = attackDelay;

        anim = gameObject.GetComponent<Animator>();

        currentHP = maxHP;

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        attackTimer += Time.deltaTime;  // attackTimer를 업데이트

        switch (currentState)
        {
            case EnemyState.Roaming:
                Roam();  // 배회 상태 동작
                if (distanceToTarget <= chaseRadius &&!isDead)
                    currentState = EnemyState.Chasing;  // 추적 상태로 전환
                break;
            case EnemyState.Chasing:
                Chase();  // 추적 상태 동작
                if (distanceToTarget <= attackRadius && !isDead)
                    currentState = EnemyState.Attacking;  // 공격 상태로 전환
                else if (distanceToTarget > chaseRadius && !isDead)
                {
                    currentState = EnemyState.Roaming;// 배회 상태로 전환
                    agent.isStopped = false;  // 이동을 재개
                }
                break;
            case EnemyState.Attacking:
                if (attackTimer >= attackDelay)  // 공격 딜레이가 지나면 공격 수행
                {
                    if (!isAttacking && !isDead)
                    {
                        agent.isStopped = true;
                        Attack();
                        isAttacking = true;
                    }
                    if (distanceToTarget > attackRadius)
                    {
                        isAttacking = false;
                        agent.isStopped = false;
                        currentState = EnemyState.Chasing;
                    }
                }
                break;
            case EnemyState.Hit:
                break;
        }

        if (currentHP <= 0)
        {
            Die();
            isDead = true;
        }

        if (agent.isStopped)
        {
            anim.SetFloat("move", 0);
        }

        attackTimer += Time.deltaTime;

    }

    private void Roam()
    {
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 randomPoint = Random.insideUnitSphere * roamRadius;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPoint, out navHit, roamRadius, -1);
            agent.SetDestination(navHit.position);
            agent.speed = 1.5f;
            anim.SetFloat("move", agent.speed); 
        }
    }

    private void Chase()
    {
        agent.SetDestination(target.position);
        agent.speed = 3f;
        anim.SetFloat("move", agent.speed);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        // 공격 로직 수행 (예: 공격 애니메이션 재생, 대상에게 데미지 적용 등)
        //target.GetComponent<PlayerHealth>().TakeDamage(damage);
        Invoke(nameof(ResetAttack), attackDelay);

    }
    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isDead == false)
        {
            if (other.gameObject.layer == 10)
            {
                currentState = EnemyState.Hit;  // 피격 상태로 전환
                anim.SetFloat("hit", Random.Range(0.0f, 2.0f));
                anim.SetTrigger("isHit");
                currentHP -= 10;
                print(currentHP);
                StartCoroutine(FlashOnHit());  // 피격 시 점멸 코루틴 실행
                StartCoroutine(RecoverFromHit()); // 피격 후 일정 시간이 지난 후 복구되도록 함
            }
        }
       
    }
    private IEnumerator FlashOnHit()
    {
        // 피격 시 색상을 변경하여 점멸 효과 적용
        enemyRenderer.material.color = hitColor;

        yield return new WaitForSeconds(hitFlashDuration);

        // 원래의 색상으로 되돌림
        enemyRenderer.material.color = originalColor;
    }
    private IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.1f); // 피격 후 일정 시간 대기

        currentState = EnemyState.Roaming; // 원하는 상태로 전환
                                           // 추가적인 초기화나 동작을 수행할 수 있습니다.
    }

    void Die()
    {
        anim.SetBool("death", true);
        agent.isStopped = true;

        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        StartCoroutine(DestroyAfterDelay(3f)); // 2초 뒤에 오브젝트 파괴
    }
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void OnAttackColl()
    {
        _boxCol.gameObject.SetActive(true);
    }

    void OffAttackColl()
    {
        _boxCol.gameObject.SetActive(false);
    }
}