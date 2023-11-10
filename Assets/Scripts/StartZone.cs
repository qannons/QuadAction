using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    public GameManager gameManager;
    private Player player;
    private Camera cam;

    //private void Awake()
    //{
    //    player = GetComponent<Player>();
    //    cam = GetComponent<Camera>();        
    //}

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>().GetComponent<Player>();
        cam = GetComponent<Camera>();  
        player.transform.position = transform.position;

        cam.transform.position = new Vector3(transform.position.x,
            transform.position.y, cam.transform.position.z);

        player.transform.position = this.transform.position;
        Debug.Log("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //        gameManager.();
    //}
}
