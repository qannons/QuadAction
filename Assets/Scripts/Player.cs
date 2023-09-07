using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //무기관련 배열 함수 2개 선언
    //플레이어가 어떤 무기를 갖고있는지
    public GameObject[] weapons;
    public bool[] hasWeapons;

    //인스펙터 창에서 설정하기 위해 public 
    public float speed;
    //Input Axsis 값을 받을 전역 변수 선언
    float hAxis;
    float vAxis;
    bool jumpDown;
    bool interactDown;

    bool canMove = true;
    bool isRun;
    bool isJump;
    bool isDodge;

    //temp start
    float curTime = 0;
    Vector3 prevMoveVec;
    //temp end
    Vector3 moveVec;

    Rigidbody rb;
    Animator animator;

    //트리거된 아이템을 저장하기 위한 변수 선언
    GameObject nearObject;

    // Start is called before the first frame update
    void Start()
    {
        prevMoveVec = Vector3.zero;
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
        Interaction();
    }

    void InputUpdate()
    {
        if (canMove == false)
            return;

        //Axis값을 정수로 반환하는 함수
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        jumpDown = Input.GetButtonDown("Jump");
        interactDown = Input.GetButtonDown("Interaction");
    }
    bool CanRun()
    {
        //(-1, 0) -> (-1, 0) == o
        //(-0.7, 0.7) -> (-1, 0) == o
        //(0.7, 0.7) -> (-0.7, 0.7) == x
        //(-0.7, 0.7) -> (0, -1) == o
        if (prevMoveVec.x >= 0)
        {
            if(moveVec.x >= 0)
                return true;
            return false;
        }
        else if (prevMoveVec.x < 0)
        {
            if (moveVec.x <= 0)
                return true;
            return false;
        }

        if (prevMoveVec.z >= 0)
        {
            if (moveVec.z >= 0)
                return true;
            return false;
        }
        else if (prevMoveVec.z < 0)
        {
            if (moveVec.z <= 0)
                return true;
            return false;
        }
        return true;
    }
    void MoveUpdate()
    {
        float deltaTime = Time.deltaTime;

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if(prevMoveVec != Vector3.zero && CanRun())
        {
            Debug.Log("true");
            curTime += deltaTime;
            if (curTime > 1.0f) 
            {
                animator.SetBool("isRun", true);
                isRun = true;
            }
        }
        else
        {
            Debug.Log("false");
            curTime = 0;
            animator.SetBool("isRun", false);
            isRun = false;
        }

        transform.position += moveVec * speed * (isRun ? 3.5f : 1.0f) * deltaTime;

        //방향키 누르면 걸음
        animator.SetBool("isWalk", moveVec != Vector3.zero);

        //Shift 같이 누르면 달림
        //animator.SetBool("isRun", isRun);

        prevMoveVec = moveVec;
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

    void Interaction()
    {
        //점프나 구르기할 땐 상호작용X
        if(interactDown && nearObject != null && isDodge == false) 
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.id;
                hasWeapons[weaponIndex] = true;     
                Destroy(nearObject);
            }
        }
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

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }

        //Debug.Log(other.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        nearObject = null;   
    }
}