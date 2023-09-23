using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C };
    public Type type;
    public int maxHealth;
    public int curHealth;
    public GameObject target;
    public BoxCollider meleeArea;
    public GameObject bullet;

    bool isAttack = false;
    bool isChase = true;
    bool isAlive = true;
    Rigidbody rb;
    Material material;
    NavMeshAgent nav; 
    Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //meleeArea = GetComponent<BoxCollider>();
        material = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        animator.SetBool("isWalk", true);
        //Invoke("ChaseStart", 2);
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
     
    private void Update()
    {
        if(nav.enabled) 
        {
            nav.SetDestination(target.transform.position);
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
        StartCoroutine(OnDamage());
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

            StartCoroutine(OnDamage());
        }
        else if(other.tag == "Bullet")
        {
            if (isAlive == false)
                return;

            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
            material.color = Color.white;
        else
        {
            material.color = Color.gray;
            isAlive = false;
            isChase = false;
            nav.enabled = false;
            animator.SetTrigger("doDie");

            Destroy(gameObject, 3);
        }

    }
}
