using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D  };
    public Type type;
    public int maxHealth;
    public int curHealth;
    public int score;
    public GameObject[] coins;
    public Transform target;
    public BoxCollider meleeArea;
    public GameManager gameManager;
    public GameObject bullet;
    public Animator animator;

   public BoxCollider boxCollider;
    protected bool isAlive = true;
    protected NavMeshAgent nav; 

    MeshRenderer[] meshRenderers;
    bool isAttack = false;
    bool isChase = true;
    Rigidbody rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        if(type != Type.D)
            Invoke("ChaseStart", 2);
    }

    void FreezeVelocity()
    {
        if(isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    void ChaseStart()
    {
        animator.SetBool("isWalk", true);

    }
     
    public virtual void Update()
    {
        if(nav.enabled) 
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void Targeting()
    {
        if (type == Type.D)
            return;

        float targetRadius = 0f;
        float targetRange = 0f;

        switch(type)
        {
            case Type.A:
                targetRadius = 1.5f;
                targetRange = 3f;
                break; 
            case Type.B:
                targetRadius = 1f;
                targetRange = 12f;
                break;
            case Type.C:
                targetRadius = 0.5f;
                targetRange = 25f;
                break;
        }

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward , targetRange, LayerMask.GetMask("Player"));
        if(rayHits.Length > 0 && isAttack == false) 
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        animator.SetBool("isAttack", true);

        switch (type)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rb.AddForce(transform.forward*30, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rbBullet = instantBullet.GetComponent<Rigidbody>();
                rbBullet.velocity  = transform.forward*20;

                yield return new WaitForSeconds(2f);
                break;
        }

        isChase = true;
        isAttack=false;
        animator.SetBool("isAttack", false);
    }

    // Update is called once per frame
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamaged());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            if (isAlive == false)
                return;

            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            //Vector3 reactVec = (transform.position - other.transform.position).normalized;
            //reactVec += Vector3.back;
            rb.AddForce(Vector3.back * 5, ForceMode.Impulse);

            StartCoroutine(OnDamaged());
        }
        else if(other.tag == "Bullet")
        {
            if (isAlive == false)
                return;

            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamaged());
        }
    }

    IEnumerator OnDamaged()
    {
        foreach(MeshRenderer mesh in meshRenderers) 
            mesh.material.color = Color.yellow;

        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshRenderers)
                mesh.material.color = Color.white;
        }
            
        else
        {
            foreach (MeshRenderer mesh in meshRenderers)
                mesh.material.color = Color.gray;
    
            isAlive = false;
            isChase = false;
            nav.enabled = false;
            animator.SetTrigger("doDie");

            Player player = target.GetComponent<Player>();
            player.score += score;
            int ranCoin = Random.Range(0, 3);
            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);

            switch(type)
            {
                case Type.A:
                    gameManager.enemyCnt[0]++;
                    break;
                case Type.B:
                    gameManager.enemyCnt[1]++;
                    break;
                case Type.C:
                    gameManager.enemyCnt[2]++;
                    break;
                case Type.D:
                    gameManager.enemyCnt[3]++;
                    break;
            }
            //if(type != Type.D)
            //    Destroy(gameObject, 3);
        }

    }
}
