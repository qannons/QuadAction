using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    GameObject cursorObject;
    //������� �迭 �Լ� 2�� ����
    //�÷��̾ � ���⸦ �����ִ���
    public  Weapon[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public GameObject grenadeObj;
    public int ammo;
    public int health = 100;
    public int numGrenades = 0;
    public int coin = 0;
    public int score;
    Camera playerCamera;
    public GameManager gameManager;
    public float speed;
    //public Camera followCamera;
    public static int maxAmmo = 200;
    public  int maxHP = 100;
    static int maxGrenade = 4;
    public Weapon equipWeapon = null;

    //Input Axsis ���� ���� ���� ���� ����
    float hAxis;
    float vAxis;

    bool canMove = true;
    bool isRun;
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady;
    bool isImmune = false;
    bool isBorder;
    bool isShopping = false;
    bool isDead = false;

    bool jumpDown;
    bool interactDown;
    bool swapDown1;
    bool swapDown2;
    bool swapDown3;
    bool fireDown;
    bool reloadDown;
    bool grenadeDown;


    float fireDelay;

    float curTime = 0;
    Vector3 prevMoveVec;
    
    Vector3 moveVec;

    Rigidbody rb;
    Animator animator;
    MeshRenderer[] meshRenderers;

    //Ʈ���ŵ� �������� �����ϱ� ���� ���� ����
    GameObject nearObject;
    GameObject mapName;

    public RaycastHit hit;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        prevMoveVec = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        Debug.Log(PlayerPrefs.GetInt("MaxScore"));
        //PlayerPrefs.SetInt("MaxScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        InputUpdate();

        MoveUpdate();
        Raycast();

        Turn();
        if(isShopping == false)
        {
            Attack();
            
            Jump();
            Dodge();
            Interaction();
            SwapWeapon();
            Reload();
        }
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    public void Raycast()
    {
        cursorObject = null;
        gameManager.CloseMoveSceneTxt();
        //if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        //{
        //    Debug.Log("Player is on: " + hit.collider.gameObject.name);
        //}
        // ȭ�� �߾ӿ� Ray�� ��� ���� ȭ���� �߾��� �������� Ray�� �����մϴ�.
        // ȭ�� �߾ӿ� Ray�� ��� ���� ȭ���� �߾��� �������� Ray�� �����մϴ�.
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        

        // Raycast�� ���� �ε��� ��ü�� ������ ������ ����
        RaycastHit hitInfo;

        // Ray�� ���� �ε��� ��ü�� �ִ��� Ȯ��
        if (Physics.Raycast(ray, out hitInfo))
        {

            // �ε��� ���������� �Ÿ��� Ȯ��
            float distanceToHit = hitInfo.distance;

            // Ư�� �Ÿ� �̻����� �̵��� ��쿡�� �۾��� ����
            if (distanceToHit <= 5f)
            {
                // �ε��� ��ü�� ���� ó���� ����
                cursorObject = hitInfo.collider.gameObject;
                Debug.Log("Hit object: " + cursorObject.name + ", Distance: " + distanceToHit);
                fn();
                // ���⿡ �߰����� ó���� �߰��� �� �ֽ��ϴ�.
            }
        }
    }

    void fn()
    {
        if (cursorObject.CompareTag("MoveScene"))
        {
            gameManager.FloatMoveSceneTxt();
        }
    }

    //void Grenade()
    //{
    //    if (grenadeDown == false)
    //        return;

    //    if (numGrenades == 0)
    //        return;

    //    Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit rayHit;
    //    if (Physics.Raycast(ray, out rayHit, 100))
    //    {
    //        Vector3 nextVec = rayHit.point - transform.position;
    //        nextVec.y = 3f;

    //        GameObject instantGrenade = Instantiate(grenadeObj, transform.position+Vector3.forward, transform.rotation);
    //        Rigidbody rbGrenade = instantGrenade.GetComponent<Rigidbody>();
    //        rbGrenade.AddForce(nextVec*2, ForceMode.Impulse);
    //        rbGrenade.AddTorque(Vector3.back*10, ForceMode.Impulse);
    //    }

    //    numGrenades--;
    //    grenades[numGrenades].SetActive(false);
    //}

    void Turn()
    {
        //Ű���� ���⿡ ���� ȸ��
        //transform.LookAt(moveVec + transform.position);

        //���콺�� ���� ȸ��
        //if(fireDown)
        //{
        //    Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit rayHit;
        //    if(Physics.Raycast(ray, out rayHit, 100)) 
        //    {
        //        Vector3 nextVec = rayHit.point - transform.position;
        //        nextVec.y = 0f;
        //        transform.LookAt(transform.position +  nextVec);
        //    }
        //}
    }
    void Reload()
    {
        if (reloadDown == false)
            return;

        if (equipWeapon == null || equipWeapon.type == Weapon.Type.Melee)
            return;

        if (ammo == 0 || isJump || isSwap || isDodge || isReload || isFireReady == false)
            return;

        animator.SetTrigger("doReload");
        isReload = true;
        Invoke("ReloadOut", 2f);
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

        fireDown = Input.GetMouseButton(0);
        reloadDown = Input.GetButtonDown("Reload");
        grenadeDown = Input.GetMouseButtonDown(1);
    }
    
    void MoveUpdate()
    {
        prevMoveVec = moveVec;
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        moveVec = Camera.main.transform.TransformDirection(moveVec);
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
        

        if(isBorder == false)
            transform.position += moveVec * speed * (isRun ? 2f : 1.0f) * Time.deltaTime; ;

        //����Ű ������ ����
        animator.SetBool("isWalk", moveVec != Vector3.zero);;
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = (equipWeapon.rate < fireDelay);

        if (isFireReady && fireDown && isDodge == false && isSwap == false)
        {
            equipWeapon.Use();
            animator.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
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

        if (equipWeapon == weapons[weaponIndex])
            return;

        if(equipWeapon != null)
            equipWeapon.gameObject.SetActive(false);

        equipWeapon = weapons[weaponIndex];
        equipWeapon.gameObject.SetActive(true);

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

    void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
    }
    void Interaction()
    {
        //������ �������� �� ��ȣ�ۿ�X
        if(interactDown && isDodge == false) 
        {
            if(nearObject != null)
            {
                if(nearObject.tag == "Weapon")
                {
                    Item item = nearObject.GetComponent<Item>();
                    int weaponIndex = item.id;
                    hasWeapons[weaponIndex] = true;     
                    Destroy(nearObject);
                }
                else if(nearObject.tag =="Shop")
                {
                    Shop shop = nearObject.GetComponent<Shop>();
                    shop.Enter(this);
                    isShopping = true;
                }
            }
            if(cursorObject.name == "Home2Shop")
            {
                SceneManager.LoadScene("shop");
            }
            else if (cursorObject.name == "Shop2Home")
            {
                SceneManager.LoadScene("FarmScene");
            }
            else if (cursorObject.name == "Villege2Naebu")
            {
                SceneManager.LoadScene("NaeBu1");
            }
        }
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward*2.5f, Color.green);
        isBorder = Physics.Raycast(transform.position, moveVec, 2.5f, LayerMask.GetMask("Wall"));

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Item")
        {
            Item item = other.GetComponent<Item>();
            switch(item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.quantity;
                    if(ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;

                case Item.Type.Heart:
                    health += item.quantity;
                    if(health > maxHP)
                        health = maxHP;
                    break;

                case Item.Type.Grenade:
                    if (numGrenades >= maxGrenade)
                        return;
                    grenades[numGrenades].SetActive(true);
                    numGrenades += item.quantity;
                    break;
            }
            Destroy(other.gameObject);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Shop" || other.tag == "MoveScene")
        {
            nearObject = other.gameObject;
        }


        //Debug.Log(other.tag);


        if (other.gameObject.CompareTag("MoveScene"))
        {
            gameManager.FloatMoveSceneTxt();
        
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Shop")
        {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Exit();
                isShopping = false;
        }
        nearObject = null;

        if (other.tag == "MoveScene")
        {
            gameManager.CloseMoveSceneTxt();
        }
    }
}
