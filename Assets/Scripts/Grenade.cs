using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effctObj;
    public Rigidbody rb;
      
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());  
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3); 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effctObj.SetActive(true);

        //방향은 상관없음
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, 35, Vector3.up, 0, LayerMask.GetMask("Enemy"));

        foreach(RaycastHit hit in raycastHits) 
        {
             hit.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 3);
    }

}
