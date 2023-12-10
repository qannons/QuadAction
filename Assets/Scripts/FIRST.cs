using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Cam : MonoBehaviour
{
    public float sensitivity = 500f; //���� ����
    public Player player;
    float rotationX = 0.0f;  //x�� ȸ����
    float rotationY = 0.0f;  //z�� ȸ����

    //private Player player;
    
    void Update()
    {
        MouseSencer();
    }
    void MouseSencer()
    {
        if (player.canMove == false)
            return;
        
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        rotationX += x * sensitivity * Time.deltaTime;
        rotationY += y * sensitivity * Time.deltaTime;

        if (rotationY > 30)
        {
            rotationY = 30;
        }
        else if (rotationY < -30)
        {
            rotationY = -30;
        }
        transform.eulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
        player.transform.eulerAngles = transform.eulerAngles;
    }
}
