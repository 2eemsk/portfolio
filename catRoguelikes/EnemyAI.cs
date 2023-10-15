using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Roaming,    // ��ȸ ����
    Chasing,    // ���� ����
    Attacking,  // ���� ����
    Hit         // �ǰ� ����
}

public class EnemyAI : MonoBehaviour
{
    [SerializeField] BoxCollider _boxCol;

    public float roamRadius = 10f;   // ��ȸ �ݰ�
    public float chaseRadius = 10f;  // ���� �ݰ�
    public float attackRadius = 2f;  // ���� �ݰ�
    public float attackDelay = 2f;   // ���� ������
    public int damage = 1;          // ���� ������

    public int maxHP = 30; // �ִ� ü��
    private int currentHP; // ���� ü��

    private Animator anim;
    public Transform target;
    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.Roaming;
    private float originalSpeed;
    private float attackTimer;

    private bool isAttacking = false;
    private bool isDead = false;

    public Color hitColor = Color.red;  // �ǰ� �� ����� ����
    public float hitFlashDuration = 0.1f;  // �ǰ� ���� �ð�

    private Renderer enemyRenderer;  // �� AI�� Renderer ������Ʈ
    private Color originalColor;     // ������ ����

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

        attackTimer += Time.deltaTime;  // attackTimer�� ������Ʈ

        switch (currentState)
        {
            case EnemyState.Roaming:
                Roam();  // ��ȸ ���� ����
                if (distanceToTarget <= chaseRadius &&!isDead)
                    currentState = EnemyState.Chasing;  // ���� ���·� ��ȯ
                break;
            case EnemyState.Chasing:
                Chase();  // ���� ���� ����
                if (distanceToTarget <= attackRadius && !isDead)
                    currentState = EnemyState.Attacking;  // ���� ���·� ��ȯ
                else if (distanceToTarget > chaseRadius && !isDead)
                {
                    currentState = EnemyState.Roaming;// ��ȸ ���·� ��ȯ
                    agent.isStopped = false;  // �̵��� �簳
                }
                break;
            case EnemyState.Attacking:
                if (attackTimer >= attackDelay)  // ���� �����̰� ������ ���� ����
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
        // ���� ���� ���� (��: ���� �ִϸ��̼� ���, ��󿡰� ������ ���� ��)
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
                currentState = EnemyState.Hit;  // �ǰ� ���·� ��ȯ
                anim.SetFloat("hit", Random.Range(0.0f, 2.0f));
                anim.SetTrigger("isHit");
                currentHP -= 10;
                print(currentHP);
                StartCoroutine(FlashOnHit());  // �ǰ� �� ���� �ڷ�ƾ ����
                StartCoroutine(RecoverFromHit()); // �ǰ� �� ���� �ð��� ���� �� �����ǵ��� ��
            }
        }
       
    }
    private IEnumerator FlashOnHit()
    {
        // �ǰ� �� ������ �����Ͽ� ���� ȿ�� ����
        enemyRenderer.material.color = hitColor;

        yield return new WaitForSeconds(hitFlashDuration);

        // ������ �������� �ǵ���
        enemyRenderer.material.color = originalColor;
    }
    private IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.1f); // �ǰ� �� ���� �ð� ���

        currentState = EnemyState.Roaming; // ���ϴ� ���·� ��ȯ
                                           // �߰����� �ʱ�ȭ�� ������ ������ �� �ֽ��ϴ�.
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

        StartCoroutine(DestroyAfterDelay(3f)); // 2�� �ڿ� ������Ʈ �ı�
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