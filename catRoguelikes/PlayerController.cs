using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5f;  // 캐릭터 이동 속도
    public float sprintSpeedMultiplier = 2f;  // 달리기 속도 배율
    public float rollSpeed = 10f;  // 구르기 속도
    public float rollDuration = 0.5f;  // 구르기 지속 시간
    public float attackCooldown = 0.5f;  // 공격 쿨다운 시간
    public int maxComboCount = 3;  // 최대 콤보 횟수

    private float currentSpeed;  // 현재 이동 속도
    private bool isSprinting;  // 달리기 여부
    private bool isAttacking;  // 공격 중인지 여부
    private bool isRolling;  // 구르기 중인지 여부
    private bool isDefending;  // 방어 중인지 여부
    private int currentComboCount;  // 현재 콤보 횟수
    private float lastAttackTime;  // 마지막 공격 시간

    Vector3 rollDirection;

    Rigidbody rb;

    private Animator anim;  // 애니메이터 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentSpeed = movementSpeed;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("MoveSpeed", movementSpeed);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * currentSpeed;
        transform.Translate(movement * Time.deltaTime);

        // 달리기 처리
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? movementSpeed * sprintSpeedMultiplier : movementSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            OnRoll();
        }

        // 공격 처리
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(Attack());
        }

        // 콤보 처리
        if (isAttacking && Input.GetMouseButtonDown(0) && currentComboCount < maxComboCount)
        {
            currentComboCount++;
            // 콤보 공격 실행
            // ...
        }
    }
    private void OnRoll()
    {
        if (!isRolling)
        {
            anim.SetTrigger("rolling");
            isRolling = true;

            // 구르는 동작 시작
            isRolling = true;

            // 현재 캐릭터의 방향 벡터를 구합니다.
            rollDirection = transform.forward;

            // Rigidbody에 구르는 동작을 적용합니다.
            rb.velocity = rollDirection * rollSpeed;
        }
    }
    public void OnRollEnd()
    {
        rb.velocity = Vector3.zero;
        isRolling = false;
        Debug.Log("RollEnd");
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");  // 공격 애니메이션

        // 공격 애니메이션 재생
        // ...

        // 공격 이펙트 생성
        // ...

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        currentComboCount = 0;
    }
}
