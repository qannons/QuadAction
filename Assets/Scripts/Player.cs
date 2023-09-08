using System;
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
    bool isSwap;

    bool swapDown1;
    bool swapDown2;
    bool swapDown3;

    float curTime = 0;
    Vector3 prevMoveVec;
    
    Vector3 moveVec;

    Rigidbody rb;
    Animator animator;

    //Ʈ���ŵ� �������� �����ϱ� ���� ���� ����
    GameObject nearObject;

    int  equipWeaponIndex = -1;

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
        SwapWeapon();
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

        swapDown1 = Input.GetButtonDown("Swap1");
        swapDown2 = Input.GetButtonDown("Swap2");
        swapDown3 = Input.GetButtonDown("Swap3");

        //Debug.Log(swapDown1);
    }
    
    void MoveUpdate()
    {
        prevMoveVec = moveVec;
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        //�� �������� �̵��ϸ� �޸��� ���
        if (prevMoveVec != Vector3.zero && isDodge == false && prevMoveVec == moveVec)
        {
            curTime += Time.deltaTime; ;
            if (curTime > 0.5f) 
            {
                animator.SetBool("isRun", true);
                isRun = true;
            }
        }
        else
        {
            curTime = 0;
            animator.SetBool("isRun", false);
            isRun = false;
        }
        
        transform.position += moveVec * speed * (isRun ? 2f : 1.0f) * Time.deltaTime; ;

        //����Ű ������ ����
        animator.SetBool("isWalk", moveVec != Vector3.zero);;
    }

    void SwapWeapon()
    {
        int weaponIndex = -1;

        if (swapDown1 && hasWeapons[0])
            weaponIndex = 0;
        else if (swapDown2 && hasWeapons[1])
            weaponIndex = 1;
        else if (swapDown3 && hasWeapons[2])
            weaponIndex = 2;
        else
            return;

        if (weaponIndex == equipWeaponIndex)
            return;

        if (equipWeaponIndex >= 0)
            weapons[equipWeaponIndex].SetActive(false);

        equipWeaponIndex = weaponIndex;
        weapons[equipWeaponIndex].SetActive(true);

        animator.SetTrigger("doSwap");
        
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