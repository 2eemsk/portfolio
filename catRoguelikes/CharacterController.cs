using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] int _playerHP = 9;
    [SerializeField] Renderer[] _meshs;
    [SerializeField] RawImage _img;
    public float moveSpeed = 4f; // Character movement speed
    float isMove; //�����̰� �ִ��� �Ǵ� ���� (4f = �ȱ� , 8f = �ٱ�)
    Vector3 rollDirection;

    private Rigidbody rb;

    public bool isRolling = false; // ������ ���� �������� ��Ÿ���� ����
    private bool isBlocking = false;
    [SerializeField]private bool isLookEnemy = false;
    public float rollSpeed = 8f; // ������ ������ �ӵ��� ������ �� �ִ� �Ķ���͸� �����մϴ�.

    [SerializeField] private bool isAttacking = false; // ���� �������� ��Ÿ���� ����
    private float attackCooldown = 1.5f; // ���� ��ٿ� �ð� ���� (1��)
    public float comboTimeLimit = 0.5f; //�޺� ���� ���ѽð�
    public float attackTimer = 0.0f; //�޺� ���� ����ġ

    [SerializeField] private int _attackCount = 0; //���� Ƚ�� (1 -> 2 -> 3)

    private Vector2 moveInput; // Movement input vector
    private Camera mainCamera; // Main camera reference
    Animator anim;

    [SerializeField]
    private GameObject _attackCollision;
    [SerializeField] private GameObject _parryingBlock;
    [SerializeField] private GameObject _handleSword;
    [SerializeField] private GameObject _backSword;
    [SerializeField] private GameObject _endingText;

    public InputActionReference blockAction;
    [SerializeField] private MapGenerator _map;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        anim = GetComponent<Animator>();
        _meshs = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
        // InputAction�� �Է� �̺�Ʈ�� ���� �ݹ� ���
        blockAction.action.started += OnBlock;
        blockAction.action.canceled += OnBlockRelease;
    }

    private void OnDisable()
    {
        // InputAction�� �Է� �̺�Ʈ�� ���� �ݹ� ��� ����
        blockAction.action.started -= OnBlock;
        blockAction.action.canceled -= OnBlockRelease;
    }

    private void OnMove(InputValue value)
    {
        if (GetComponentInChildren<LobyCam>().isStart == false) { return; }

        moveInput = value.Get<Vector2>();
    }

    private void OnRoll()
    {
        if (!isRolling && GetComponentInChildren<LobyCam>().isStart == true)
        {
            anim.SetTrigger("rolling");
            isRolling = true;

            // ������ ���� ����
            isRolling = true;

            // ���� ĳ������ ���� ���͸� ���մϴ�.
            rollDirection = transform.forward;

            // Rigidbody�� ������ ������ �����մϴ�.
            rb.velocity = rollDirection * rollSpeed;
            print(rb.velocity);
        }
    }

    public void OnRollEnd()
    {
        print(rb.velocity);
        Debug.Log("RollEnd");
        rb.velocity = Vector3.zero;
        isRolling = false;
    }

    private void OnAttack()
    {
        if (GetComponentInChildren<LobyCam>().isStart == false) { return; }

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPosition = hit.point;
            Debug.Log("Attack! World Position: " + worldPosition);

            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f; // y �� ȸ�� ����
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = rotation;
        }
        if (isAttacking == false)
        {
            isAttacking = true;
            _attackCount = 1;
            attackTimer = comboTimeLimit;
            anim.SetInteger("AttackCount", _attackCount);
        }
        else if (_attackCount < 3)
        {
            _attackCount++;
            attackTimer = comboTimeLimit;
            anim.SetInteger("AttackCount", _attackCount);
        }
    }

    /*private void OnAttack()
    {
        isAttacking = true;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPosition = hit.point;
            Debug.Log("Attack! World Position: " + worldPosition);

            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f; // y �� ȸ�� ����
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = rotation;
        }

        //anim.SetTrigger("attacking");
        _attackCount += 1;

        if(_attackCount == 1)
        {
            anim.SetInteger("AttackCount", 1);
        }
        else if (_attackCount >= 10)
        {
            ResetAttackPhase();
        }

        // ���� ��ٿ� �ð� ���Ŀ� isAttacking�� false�� �����Ͽ� �ٽ� ������ �� �ֵ��� ����
        StartCoroutine(ResetAttackFlag());
    }*/

    /*public void AttackPhase()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
        {
            if (_attackCount > 1)
            {
                anim.SetInteger("AttackCount", 2);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            if (_attackCount > 2)
            {
                anim.SetInteger("AttackCount", 3);
                transform.Translate(Vector3.forward);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack3"))
        {
            if (_attackCount >= 3)
            {
                ResetAttackPhase();
            }
        }
    }

    private void ResetAttackPhase()
    {
        isAttacking = false;
        _attackCount = 0;
        anim.SetInteger("AttackCount", 0);
    }

    private IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }*/

    public void OnBlock(InputAction.CallbackContext contex)
    { 
        if(GetComponentInChildren<LobyCam>().isStart == false) { return; }


        // ���콺�� ���� ��ǥ ���� ����
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // ��ü�� forward ������ ���콺�� ���� ��ǥ ������ ȸ��
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f; // y �� ȸ�� ����
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = rotation;
            isBlocking = true;

            Debug.Log("���");
        }

        anim.SetTrigger("Blocking");
    }

    public void OnBlockRelease(InputAction.CallbackContext context)
    {
        // ���콺 ��ư�� ������ ������ �� ����Ǵ� �ڵ�
        // ���⿡ �ִϸ��̼� ���� �ڵ带 �߰��մϴ�.
        // ��: anim.StopPlayback();
        isBlocking = false;

        anim.SetTrigger("BlockingRelease");
        Debug.Log("��� ����");

    }

    public void ResetAttackAnim()
    {
        isAttacking = false;
        _attackCount = 0;
        anim.SetInteger("AttackCount", 0);
        anim.ResetTrigger("attacking");
    }

    public void BlockParryingEvent()
    {
        _parryingBlock.SetActive(true);
    }

    private void OnInteraction()
    {
        Debug.Log("��ȣ�ۿ�");
    }

    public void OnAttackCollision()
    {
        _attackCollision.SetActive(true);
    }

    public void OffAttackCollision()
    {
        _attackCollision.SetActive(false);
    }

    private void Update()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
        Vector3 cameraUp = Vector3.Cross(cameraForward, cameraRight);
        var moveDirX = Input.GetAxis("Horizontal");
        var moveDirY = Input.GetAxis("Vertical");

        if(Math.Abs((moveDirX)) >= Math.Abs((moveDirY)))
        {
            isMove = Math.Abs((moveDirX));
        }
        else
        {
            isMove = Math.Abs((moveDirY));
        }

        //�޸��� ���
        if (moveDirX != 0 || moveDirY != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed += 1;
                if (moveSpeed >= 8)
                {
                    moveSpeed = 8;
                    isMove = 2;
                }
            }
            else
            {
                if (Math.Abs((moveDirX)) >= Math.Abs((moveDirY)))
                {
                    isMove = Math.Abs((moveDirX));
                }
                else
                {
                    isMove = Math.Abs((moveDirY));
                }
                moveSpeed -= 1;
                if (moveSpeed <= 4)
                {
                    moveSpeed = 4;
                }
            }
        }
        else
        {
            isMove = 0f;
        }

        if (!isRolling && !isAttacking)
        {
            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }


            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection, cameraUp);
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            anim.SetFloat("moveX", moveDirX);
            anim.SetFloat("moveY", moveDirY);

            anim.SetFloat("moveSpeed", isMove);
        }

        /*if(isBlocking == true)
        {
            isLookEnemy = OnLookObject();
        }*/

        if (isAttacking == true)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0.0f)
            {
                ResetAttackAnim();
            }
        }

        if(_playerHP <= 0)
        {
            StartCoroutine(ResetGame());
            _endingText.SetActive(true);
            anim.SetTrigger("isDead");
            this.enabled = false;
        }

    }

    private bool OnLookObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            if(hit.transform.gameObject.layer == 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            other.GetComponentInParent<EnemyNavSystem>().BoxCol.enabled = false;
            var _dir = Vector3.Normalize(transform.position - other.transform.position);

            StartCoroutine(Blocking(_dir));
        }
        else if(other.gameObject.layer == 15)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        _img.enabled = true;
        // Image ������Ʈ�� ���� �� �о����.  
        Color color = _img.color;
        var time = 0f;
        var start = 0f;
        var end = 1f;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a < 1f)
        {
            // ��� �ð� ���.  
            // 2��(animTime)���� ����� �� �ֵ��� animTime���� ������.  
            time += Time.deltaTime / 1f;

            // ���� �� ���.  
            color.a = Mathf.Lerp(start, end, time);
            // ����� ���� �� �ٽ� ����.  
            _img.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        _map.InitializeMapTiles();
        _map.MapCollocate();
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator FadeOut()
    {
        yield return StartCoroutine(FadeIn());

        // Image ������Ʈ�� ���� �� �о����.  
        Color color = _img.color;
        var time = 1f;
        var start = 0f;
        var end = 1f;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a > 0f)
        {
            // ��� �ð� ���.  
            // 2��(animTime)���� ����� �� �ֵ��� animTime���� ������.  
            time -= Time.deltaTime / 1f;

            // ���� �� ���.  
            color.a = Mathf.Lerp(start, end, time);
            // ����� ���� �� �ٽ� ����.  
            _img.color = color;

            yield return null;
        }

        _img.enabled = false;

    }

    IEnumerator OnDamage(Vector3 _dir)
    {
        rb.isKinematic = true;

        foreach (Renderer _mesh in _meshs)
        {
            print("debug");
            _mesh.material.SetColor("_MainColor", Color.yellow);
        }

        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;
        rb.AddForce(_dir * 8f, ForceMode.Impulse);

        foreach (Renderer _mesh in _meshs)
        {
            _mesh.material.SetColor("_MainColor", new Color(0.1f,0.1f,0.1f));
        }

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
    }

    IEnumerator Blocking(Vector3 _dir)
    {
        rb.isKinematic = true;
        if(isBlocking == false)
        {
            _playerHP -= 3;

            foreach (Renderer _mesh in _meshs)
            {
                print("debug");
                _mesh.material.SetColor("_MainColor", Color.yellow);
            }
        }

        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;

        if(isBlocking == false)
        {
            rb.AddForce(_dir * 8f, ForceMode.Impulse);

            foreach (Renderer _mesh in _meshs)
            {
                _mesh.material.SetColor("_MainColor", new Color(0.1f, 0.1f, 0.1f));
            }
        }
        else
        {
            rb.AddForce(_dir * 5f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector3.zero;
        foreach (Renderer _mesh in _meshs)
        {
            _mesh.material.SetColor("_MainColor", new Color(0.1f, 0.1f, 0.1f));
        }
    }

    public void LobyAnimPickSword()
    {
        _handleSword.SetActive(true);
        _backSword.SetActive(false);
    }

    IEnumerator ResetGame()
    {
        GetComponentInChildren<LobyCam>().isStart = false;

        yield return new WaitForSeconds(5f);

        SceneLoadManager.currentStage = 0;
        SceneLoadManager.OpenStage();
    }

    public void MovePossible()
    {
        GetComponentInChildren<LobyCam>().isStart = true;
    }

}
