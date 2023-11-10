using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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

    private void Awake()
    {
        DontDestroyOnLoad(this);
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

    private void ReloadOut()
    {
        int reloadAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        ammo -= reloadAmmo;

        equipWeapon.curAmmo = reloadAmmo;
        isReload = false;

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
        if(interactDown && nearObject != null && isDodge == false) 
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
            else if(nearObject.tag == "MoveScene")
            {
                SceneManager.LoadScene("ItemShop");
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
        else if(other.gameObject.tag == "Bullet")
        {
            if (isImmune == false)
            {
                Bullet bullet = other.GetComponent<Bullet>();
                health -= bullet.damage;
                bool isBossAttack = other.name == "Boss Melee Area";
                
                StartCoroutine(OnDamaged(isBossAttack));
            }
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

    IEnumerator OnDamaged(bool isBossAttack)
    {
        if(health <= 0 && isDead == false)
        {
            OnDie();
        }

        isImmune = true;
        foreach(MeshRenderer mesh in meshRenderers) 
            mesh.material.color = Color.red;
        
        if(isBossAttack)
        {
            //Vector3 KnockBackPos = (transform.forward * -25);
            //transform.position = Vector3.Lerp(transform.position, KnockBackPos, 5 * Time.deltaTime);
            rb.AddForce(transform.forward*-25, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.4f);

        isImmune = false;
        foreach (MeshRenderer mesh in meshRenderers)
            mesh.material.color = Color.white;

        //if(isBossAttack )
            //rb.velocity = Vector3.zero;
    }

    void OnDie()
    {
        animator.SetTrigger("doDie");
        isDead = true;
        gameManager.GameOver();
    }


}
