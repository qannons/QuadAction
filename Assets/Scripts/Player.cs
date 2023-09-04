using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //인스펙터 창에서 설정하기 위해 public 
    public float speed;
    //Input Axsis 값을 받을 전역 변수 선언
    float hAxis;
    float vAxis;
    bool runDown;
    Vector3 moveVec;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Axis값을 정수로 반환하는 함수
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        runDown = Input.GetButton("Run");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (runDown ? 1.5f : 1.0f) * Time.deltaTime;

        //방향키 누르면 걸음
        animator.SetBool("isWalk", moveVec != Vector3.zero);

        //Shift 같이 누르면 달림
        animator.SetBool("isRun", runDown);

        transform.LookAt(moveVec + transform.position);

    }
}