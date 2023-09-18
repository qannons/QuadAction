using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public GameObject target;

    bool isChase = true;
    bool isAlive = true;
    Rigidbody rb;
    BoxCollider boxCollider;
    Material material;
    NavMeshAgent nav; 
    Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        material = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        animator.SetBool("isWalk", true);
        //Invoke("ChaseStart", 2);
    }

    void FreezeVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    void ChaseStart()
    {
        animator.SetBool("isWalk", true);

    }
     
    private void Update()
    {
        if (isChase)
            nav.SetDestination(target.transform.position);
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
