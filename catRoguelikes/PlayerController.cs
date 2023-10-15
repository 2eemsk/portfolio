using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5f;  // ĳ���� �̵� �ӵ�
    public float sprintSpeedMultiplier = 2f;  // �޸��� �ӵ� ����
    public float rollSpeed = 10f;  // ������ �ӵ�
    public float rollDuration = 0.5f;  // ������ ���� �ð�
    public float attackCooldown = 0.5f;  // ���� ��ٿ� �ð�
    public int maxComboCount = 3;  // �ִ� �޺� Ƚ��

    private float currentSpeed;  // ���� �̵� �ӵ�
    private bool isSprinting;  // �޸��� ����
    private bool isAttacking;  // ���� ������ ����
    private bool isRolling;  // ������ ������ ����
    private bool isDefending;  // ��� ������ ����
    private int currentComboCount;  // ���� �޺� Ƚ��
    private float lastAttackTime;  // ������ ���� �ð�

    Vector3 rollDirection;

    Rigidbody rb;

    private Animator anim;  // �ִϸ����� ������Ʈ

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

        // �޸��� ó��
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? movementSpeed * sprintSpeedMultiplier : movementSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            OnRoll();
        }

        // ���� ó��
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(Attack());
        }

        // �޺� ó��
        if (isAttacking && Input.GetMouseButtonDown(0) && currentComboCount < maxComboCount)
        {
            currentComboCount++;
            // �޺� ���� ����
            // ...
        }
    }
    private void OnRoll()
    {
        if (!isRolling)
        {
            anim.SetTrigger("rolling");
            isRolling = true;

            // ������ ���� ����
            isRolling = true;

            // ���� ĳ������ ���� ���͸� ���մϴ�.
            rollDirection = transform.forward;

            // Rigidbody�� ������ ������ �����մϴ�.
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
        anim.SetTrigger("Attack");  // ���� �ִϸ��̼�

        // ���� �ִϸ��̼� ���
        // ...

        // ���� ����Ʈ ����
        // ...

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        currentComboCount = 0;
    }
}
