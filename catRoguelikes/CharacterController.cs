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
    float isMove; //움직이고 있는지 판단 변수 (4f = 걷기 , 8f = 뛰기)
    Vector3 rollDirection;

    private Rigidbody rb;

    public bool isRolling = false; // 구르는 동작 중인지를 나타내는 변수
    private bool isBlocking = false;
    [SerializeField]private bool isLookEnemy = false;
    public float rollSpeed = 8f; // 구르는 동작의 속도를 조절할 수 있는 파라미터를 설정합니다.

    [SerializeField] private bool isAttacking = false; // 공격 중인지를 나타내는 변수
    private float attackCooldown = 1.5f; // 공격 쿨다운 시간 설정 (1초)
    public float comboTimeLimit = 0.5f; //콤보 공격 제한시간
    public float attackTimer = 0.0f; //콤보 공격 스위치

    [SerializeField] private int _attackCount = 0; //어택 횟수 (1 -> 2 -> 3)

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
        // InputAction의 입력 이벤트에 대한 콜백 등록
        blockAction.action.started += OnBlock;
        blockAction.action.canceled += OnBlockRelease;
    }

    private void OnDisable()
    {
        // InputAction의 입력 이벤트에 대한 콜백 등록 해제
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

            // 구르는 동작 시작
            isRolling = true;

            // 현재 캐릭터의 방향 벡터를 구합니다.
            rollDirection = transform.forward;

            // Rigidbody에 구르는 동작을 적용합니다.
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
            lookDirection.y = 0f; // y 축 회전 방지
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
            lookDirection.y = 0f; // y 축 회전 방지
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

        // 공격 쿨다운 시간 이후에 isAttacking을 false로 변경하여 다시 공격할 수 있도록 설정
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


        // 마우스의 월드 좌표 값을 얻어옴
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // 물체의 forward 방향을 마우스의 월드 좌표 값으로 회전
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f; // y 축 회전 방지
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = rotation;
            isBlocking = true;

            Debug.Log("방어");
        }

        anim.SetTrigger("Blocking");
    }

    public void OnBlockRelease(InputAction.CallbackContext context)
    {
        // 마우스 버튼이 떼어진 상태일 때 실행되는 코드
        // 여기에 애니메이션 정지 코드를 추가합니다.
        // 예: anim.StopPlayback();
        isBlocking = false;

        anim.SetTrigger("BlockingRelease");
        Debug.Log("방어 해제");

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
        Debug.Log("상호작용");
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

        //달리기 기능
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
        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = _img.color;
        var time = 0f;
        var start = 0f;
        var end = 1f;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a < 1f)
        {
            // 경과 시간 계산.  
            // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
            time += Time.deltaTime / 1f;

            // 알파 값 계산.  
            color.a = Mathf.Lerp(start, end, time);
            // 계산한 알파 값 다시 설정.  
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

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = _img.color;
        var time = 1f;
        var start = 0f;
        var end = 1f;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a > 0f)
        {
            // 경과 시간 계산.  
            // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
            time -= Time.deltaTime / 1f;

            // 알파 값 계산.  
            color.a = Mathf.Lerp(start, end, time);
            // 계산한 알파 값 다시 설정.  
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
