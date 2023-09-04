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
        //Axis���� ������ ��ȯ�ϴ� �Լ�
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        runDown = Input.GetButton("Run");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (runDown ? 1.5f : 1.0f) * Time.deltaTime;

        //����Ű ������ ����
        animator.SetBool("isWalk", moveVec != Vector3.zero);

        //Shift ���� ������ �޸�
        animator.SetBool("isRun", runDown);

        transform.LookAt(moveVec + transform.position);

    }
}