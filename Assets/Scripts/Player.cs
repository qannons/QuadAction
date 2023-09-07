using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //������� �迭 �Լ� 2�� ����
    //�÷��̾ � ���⸦ �����ִ���
    public GameObject[] weapons;
    public bool[] hasWeapons;

    //�ν����� â���� �����ϱ� ���� public 
    public float speed;
    //Input Axsis ���� ���� ���� ���� ����
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

    //Ʈ���ŵ� �������� �����ϱ� ���� ���� ����
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

        //���⿡ ���� ȸ��
        transform.LookAt(moveVec + transform.position);

        Jump();
        Dodge();
        Interaction();
    }

    void InputUpdate()
    {
        if (canMove == false)
            return;

        //Axis���� ������ ��ȯ�ϴ� �Լ�
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

        //����Ű ������ ����
        animator.SetBool("isWalk", moveVec != Vector3.zero);

        //Shift ���� ������ �޸�
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

            //���ϴ� ���߿� ������ ��ȯ���� ����
            //DodgeOut���� true�� �ٲ���
            canMove = false;

            //�ð��� �Լ� ȣ��
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
        //������ �������� �� ��ȣ�ۿ�X
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
        //�±׸� Ȱ���� �ٴڿ��� �۵��ϵ���
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