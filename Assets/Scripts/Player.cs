using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�ν����� â���� �����ϱ� ���� public 
    public float speed;
    //Input Axsis ���� ���� ���� ���� ����
    float hAxis;
    float vAxis;
    bool runDown;
    bool jumpDown;
    bool isJump;

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

        //���⿡ ���� ȸ��
        transform.LookAt(moveVec + transform.position);

        Jump();
    }

    void InputUpdate()
    {
        //Axis���� ������ ��ȯ�ϴ� �Լ�
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        runDown = Input.GetButton("Run");
        jumpDown = Input.GetButtonDown("Jump");
    }

    void MoveUpdate()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (runDown ? 1.5f : 1.0f) * Time.deltaTime;

        //����Ű ������ ����
        animator.SetBool("isWalk", moveVec != Vector3.zero);

        //Shift ���� ������ �޸�
        animator.SetBool("isRun", runDown);
    }

    void Jump()
    {
        if (jumpDown && isJump == false) 
        {
            rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
             
            animator.SetBool("isJump", true);
            animator.SetTrigger("doJump");

            isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�±׸� Ȱ���� �ٴڿ��� �۵��ϵ���
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
            animator.SetBool("isJump", false);
        }
    }
}