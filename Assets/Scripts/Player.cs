using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //인스펙터 창에서 설정하기 위해 public 
    public float speed;
    //Input Axsis 값을 받을 전역 변수 선언
    float hAxis;
    float vAxis;
    bool runDown;
    bool jumpDown;

    bool canMove = true;
    bool isJump;
    bool isDodge;

    Vector3 moveVec;

    Rigidbody rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();

        MoveUpdate();

        //방향에 따라 회전
        transform.LookAt(moveVec + transform.position);

        Jump();
        Dodge();
    }

    void InputUpdate()
    {
        if (canMove == false)
            return;

        //Axis값을 정수로 반환하는 함수
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        runDown = Input.GetButton("Run");
        jumpDown = Input.GetButtonDown("Jump");
    }

    void MoveUpdate()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (runDown ? 1.5f : 1.0f) * Time.deltaTime;

        //방향키 누르면 걸음
        animator.SetBool("isWalk", moveVec != Vector3.zero);

        //Shift 같이 누르면 달림
        animator.SetBool("isRun", runDown);
    }

    void Jump()
    {
        if (jumpDown && moveVec == Vector3.zero && isJump == false) 
        {
            rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
             
            animator.SetBool("isJump", true);
            animator.SetTrigger("doJump");

            isJump = true;
        }
    }

    void Dodge()
    {
        if (jumpDown && moveVec != Vector3.zero && isDodge == false)
        {
            speed *= 2;
            animator.SetTrigger("doDodge");
            isDodge = true;

            //피하는 도중엔 방향을 전환하지 못함
            //DodgeOut에서 true로 바꿔줌
            canMove = false;

            //시간차 함수 호출
            Invoke("DodgeOut", 0.6f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
        canMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //태그를 활용해 바닥에만 작동하도록
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
            animator.SetBool("isJump", false);
        }
    }
}