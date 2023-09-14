using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon};
    public int id;
    public Type type;
    public int quantity;

    Rigidbody rb;
    SphereCollider sphereCollider;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 15 * Time.deltaTime);  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            rb.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
