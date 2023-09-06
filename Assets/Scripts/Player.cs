using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //������� �迭 �Լ� 2�� ����
    public GameObject[] weapons;
    public bool[] hasWeapons;

    //�ν����� â���� �����ϱ� ���� public 
    public float speed;
    //Input Axsis ���� ���� ���� ���� ����
    float hAxis;
    float vAxis;
    bool runDown;
    bool jumpDown;
    bool interactDown;

    bool canMove = true;
    bool isJump;
    bool isDodge;

    Vector3 moveVec;

    Rigidbody rb;
    Animator animator;

    //Ʈ���ŵ� �������� �����ϱ� ���� ���� ����
    GameObject nearObject;

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
        Dodge();
    }

    void InputUpdate()
    {
        if (canMove == false)
            return;

        //Axis���� ������ ��ȯ�ϴ� �Լ�
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        runDown = Input.GetButton("Run");
        jumpDown = Input.GetButtonDown("Jump");
        interactDown = Input.GetButtonDown("Interaction");
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
                int weaponIndex = item.value;
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
        if(other.gameObject.tag == "Weapon")
            nearObject = other.gameObject;

        Debug.Log(nearObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}