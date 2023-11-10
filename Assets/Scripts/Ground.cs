using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SocialPlatforms.Impl;

public class Ground : MonoBehaviour
{
    private Material[] mat = new Material[2];
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        mat = Resources.LoadAll<Material>("Materials");
        Debug.Log("s");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hoe")
        {
            Debug.Log("d");
            i = (i + 1) % 2;
            gameObject.GetComponent<MeshRenderer>().material = mat[i];
            
            //StartCoroutine(OnDamaged());
        }
        
    }

    IEnumerator OnDamaged()
    {
        
        yield return new WaitForSeconds(0.1f);

        

    }
}
