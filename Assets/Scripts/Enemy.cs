using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    bool isAlive = true;
    Rigidbody rb;
    BoxCollider boxCollider;
    Material material;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame

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
            Destroy(gameObject, 4);
            isAlive = false;
        }

    }
}
