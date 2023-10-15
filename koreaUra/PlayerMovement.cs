using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movePower = 1f; //이동 힘
    public float JumpPower = 1f; //점프 힘
    public float MyPos;

    Rigidbody2D rigid;
    public SpriteRenderer renderer;

    Vector3 movement;
    Animator animator;
    bool isJumping = false;
    bool isFire = false;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //점프 참값으로 변환
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        //총 발사 로직
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isFire = true;
            animator.SetTrigger("Shoot");
        }
        

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("isMoving", false);
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            animator.SetBool("isMoving", true);
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetBool("isMoving", true);

        }

    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Fire();
    }

     //점프 매서드
    void Jump()
    {
        if (!isJumping)
        {
            return;
        }

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, JumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
        
    }

    //캐릭터 이동 매서드
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw ("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    //총 발사 매서드
    void Fire()
    {
        if (!isFire)
        {
            return;
        }
    }
}
